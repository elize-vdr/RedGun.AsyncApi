// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.ParseNodes;

namespace RedGun.AsyncApi.Readers.V3
{
    /// <summary>
    /// Class containing logic to deserialize Async API V3 document into
    /// runtime Async API object model.
    /// </summary>
    internal static partial class AsyncApiV3Deserializer
    {
        private static FixedFieldMap<AsyncApiDocument> _asyncApiFixedFields = new FixedFieldMap<AsyncApiDocument>
        {
            {
                "asyncapi", (o, n) =>
                {
                } /* Version is valid field but we already parsed it */
            },
            {"info", (o, n) => o.Info = LoadInfo(n)},
            {"servers", (o, n) => o.Servers = n.CreateList(LoadServer)},
            {"paths", (o, n) => o.Paths = LoadPaths(n)},
            {"components", (o, n) => o.Components = LoadComponents(n)},
            {"tags", (o, n) => {o.Tags = n.CreateList(LoadTag);
                foreach (var tag in o.Tags)
    {
                    tag.Reference = new AsyncApiReference()
                    {
                        Id = tag.Name,
                        Type = ReferenceType.Tag
                    };
    }
            } },
            {"externalDocs", (o, n) => o.ExternalDocs = LoadExternalDocs(n)},
            {"security", (o, n) => o.SecurityRequirements = n.CreateList(LoadSecurityRequirement)}
        };

        private static PatternFieldMap<AsyncApiDocument> _asyncApiPatternFields = new PatternFieldMap<AsyncApiDocument>
        {
            // We have no semantics to verify X- nodes, therefore treat them as just values.
            {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p, n))}
        };

        public static AsyncApiDocument LoadAsyncApi(RootNode rootNode)
        {
            var asyncApiDoc = new AsyncApiDocument();

            var asyncApiNode = rootNode.GetMap();

            ParseMap(asyncApiNode, asyncApiDoc, _asyncApiFixedFields, _asyncApiPatternFields);

            return asyncApiDoc;
        }
    }
}
