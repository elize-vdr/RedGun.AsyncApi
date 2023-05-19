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
        private static FixedFieldMap<AsyncApiDocument> _asyncApiFixedFields = new FixedFieldMap<AsyncApiDocument>
        {
            {
                AsyncApiConstants.AsyncApi, (o, n) =>
                {
                } /* Version is valid field but we already parsed it */
            },
            {AsyncApiConstants.Id, (o, n) =>
                {
                    o.Id = n.GetScalarValue();
                }
            },
            {AsyncApiConstants.Info, (o, n) =>
                {
                    o.Info = LoadInfo(n);
                }
            },
            {AsyncApiConstants.Servers, (o, n) =>
                {
                    o.Servers = LoadServers(n);
                }
            },
            {AsyncApiConstants.DefaultContentType, (o, n) =>
                {
                    o.DefaultContentType = n.GetScalarValue();
                }
            },
            {AsyncApiConstants.Channels, (o, n) =>
                {
                    o.Channels = LoadChannels(n);
                }
            },
            {AsyncApiConstants.Components, (o, n) =>
                {
                    o.Components = LoadComponents(n);
                }
            },
            {AsyncApiConstants.Tags, (o, n) => 
                {
                    o.Tags = n.CreateList(LoadTag);
                    foreach (var tag in o.Tags)
                    {
                        tag.Reference = new AsyncApiReference()
                        {
                            Id = tag.Name,
                            Type = ReferenceType.Tag
                        };
                    }
                } 
            },
            {AsyncApiConstants.ExternalDocs, (o, n) => o.ExternalDocs = LoadExternalDocs(n)}
        };

        private static PatternFieldMap<AsyncApiDocument> _asyncApiPatternFields = new PatternFieldMap<AsyncApiDocument>
        {
            // We have no semantics to verify X- nodes, therefore treat them as just values.
            {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p, n))}
        };

        public static AsyncApiDocument LoadAsyncApi(RootNode rootNode)
        {
            var asyncApiDocument = new AsyncApiDocument();

            var asyncApiNode = rootNode.GetMap();

            ParseMap(asyncApiNode, asyncApiDocument, _asyncApiFixedFields, _asyncApiPatternFields);

            return asyncApiDocument;
        }
    }
}
