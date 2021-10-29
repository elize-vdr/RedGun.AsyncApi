// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK.
// Licensed under the MIT license. 

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
        private static readonly FixedFieldMap<AsyncApiTag> _tagFixedFields = new FixedFieldMap<AsyncApiTag>
        {
            {
                AsyncApiConstants.Name, (o, n) =>
                {
                    o.Name = n.GetScalarValue();
                }
            },
            {
                AsyncApiConstants.Description, (o, n) =>
                {
                    o.Description = n.GetScalarValue();
                }
            },
            {
                AsyncApiConstants.ExternalDocs, (o, n) =>
                {
                    o.ExternalDocs = LoadExternalDocs(n);
                }
            }
        };

        private static readonly PatternFieldMap<AsyncApiTag> _tagPatternFields = new PatternFieldMap<AsyncApiTag>
        {
            {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
        };

        public static AsyncApiTag LoadTag(ParseNode n)
        {
            var mapNode = n.CheckMapNode("tag");

            var domainObject = new AsyncApiTag();

            foreach (var propertyNode in mapNode)
            {
                propertyNode.ParseField(domainObject, _tagFixedFields, _tagPatternFields);
            }

            return domainObject;
        }
    }
}
