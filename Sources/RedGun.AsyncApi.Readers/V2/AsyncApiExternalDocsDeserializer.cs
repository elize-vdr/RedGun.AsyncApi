// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK.
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Extensions;
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
        private static readonly FixedFieldMap<AsyncApiExternalDocs> _externalDocsFixedFields =
            new FixedFieldMap<AsyncApiExternalDocs>
            {
                {
                    AsyncApiConstants.Description, (o, n) =>
                    {
                        o.Description = n.GetScalarValue();
                    }
                },
                {
                    AsyncApiConstants.Url, (o, n) =>
                    {
                        o.Url = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute);
                    }
                },
            };

    private static readonly PatternFieldMap<AsyncApiExternalDocs> _externalDocsPatternFields =
            new PatternFieldMap<AsyncApiExternalDocs> {

                    {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p, n))}
                };

    public static AsyncApiExternalDocs LoadExternalDocs(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.ExternalDocs);

            var externalDocs = new AsyncApiExternalDocs();

            ParseMap(mapNode, externalDocs, _externalDocsFixedFields, _externalDocsPatternFields);

            return externalDocs;
        }
    }
}
