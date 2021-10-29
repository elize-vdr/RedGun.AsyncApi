// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK.
// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.ParseNodes;

namespace RedGun.AsyncApi.Readers.V2
{
    /// <summary>
    /// Class containing logic to deserialize Open API V2 document into
    /// runtime Open API object model.
    /// </summary>
    internal static partial class AsyncApiV2Deserializer
    {
        private static FixedFieldMap<AsyncApiDocument> _openApiFixedFields = new FixedFieldMap<AsyncApiDocument>
        {
            {
                "openapi", (o, n) =>
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

        private static PatternFieldMap<AsyncApiDocument> _openApiPatternFields = new PatternFieldMap<AsyncApiDocument>
        {
            // We have no semantics to verify X- nodes, therefore treat them as just values.
            {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p, n))}
        };

        public static AsyncApiDocument LoadAsyncApi(RootNode rootNode)
        {
            var openApidoc = new AsyncApiDocument();

            var openApiNode = rootNode.GetMap();

            ParseMap(openApiNode, openApidoc, _openApiFixedFields, _openApiPatternFields);

            return openApidoc;
        }
    }
}
