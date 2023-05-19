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
        private static FixedFieldMap<AsyncApiMessageExample> _messageExampleFixedFields = new FixedFieldMap<AsyncApiMessageExample>()
        {
            {
                AsyncApiConstants.Headers, (o, n) =>
                {
                    // TODO: Check if LoadAny will work on this CreateMap?
                    o.Headers = n.CreateMap(LoadAny);
                }
            },
            {
                // TODO: not sure if this will work, how to deserialize "any"?
                AsyncApiConstants.Payload, (o, n) =>
                {
                    o.Payload = LoadAny(n);
                }
            },
            {
                AsyncApiConstants.Name, (o, n) =>
                {
                    o.Name = n.GetScalarValue();
                }
            },
            {
                AsyncApiConstants.Summary, (o, n) =>
                {
                    o.Summary = n.GetScalarValue();
                }
            },

        };

        private static PatternFieldMap<AsyncApiMessageExample> _messageExamplePatternFields =
            new PatternFieldMap<AsyncApiMessageExample> {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p, n))}
            };

        public static AsyncApiMessageExample LoadMessageExample(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.Examples);

            var domainObject = new AsyncApiMessageExample();

            ParseMap(mapNode, domainObject, _messageExampleFixedFields, _messageExamplePatternFields);

            return domainObject;
        }
    }
}
