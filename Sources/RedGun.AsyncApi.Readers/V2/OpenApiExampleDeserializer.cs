// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.ParseNodes;

namespace RedGun.AsyncApi.Readers.V2
{
    /// <summary>
    /// Class containing logic to deserialize Async API V3 document into
    /// runtime Async API object model.
    /// </summary>
    internal static partial class AsyncApiV2Deserializer
    {
        private static readonly FixedFieldMap<AsyncApiExample> _exampleFixedFields = new FixedFieldMap<AsyncApiExample>
        {
            {
                "summary", (o, n) =>
                {
                    o.Summary = n.GetScalarValue();
                }
            },
            {
                "description", (o, n) =>
                {
                    o.Description = n.GetScalarValue();
                }
            },
            {
                "value", (o, n) =>
                {
                    o.Value = n.CreateAny();
                }
            },
            {
                "externalValue", (o, n) =>
                {
                    o.ExternalValue = n.GetScalarValue();
                }
            },

        };

        private static readonly PatternFieldMap<AsyncApiExample> _examplePatternFields =
            new PatternFieldMap<AsyncApiExample>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
            };

        public static AsyncApiExample LoadExample(ParseNode node)
        {
            var mapNode = node.CheckMapNode("example");

            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                return mapNode.GetReferencedObject<AsyncApiExample>(ReferenceType.Example, pointer);
            }

            var example = new AsyncApiExample();
            foreach (var property in mapNode)
            {
                property.ParseField(example, _exampleFixedFields, _examplePatternFields);
            }

            return example;
        }
    }
}
