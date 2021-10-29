// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK.
// Licensed under the MIT license. 

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
        private static readonly FixedFieldMap<AsyncApiDiscriminator> _discriminatorFixedFields =
            new FixedFieldMap<AsyncApiDiscriminator>
            {
                {
                    "propertyName", (o, n) =>
                    {
                        o.PropertyName = n.GetScalarValue();
                    }
                },
                {
                    "mapping", (o, n) =>
                    {
                        o.Mapping = n.CreateSimpleMap(LoadString);
                    }
                }
            };

        private static readonly PatternFieldMap<AsyncApiDiscriminator> _discriminatorPatternFields =
            new PatternFieldMap<AsyncApiDiscriminator>();

        public static AsyncApiDiscriminator LoadDiscriminator(ParseNode node)
        {
            var mapNode = node.CheckMapNode("discriminator");

            var discriminator = new AsyncApiDiscriminator();
            foreach (var property in mapNode)
            {
                property.ParseField(discriminator, _discriminatorFixedFields, _discriminatorPatternFields);
            }

            return discriminator;
        }
    }
}
