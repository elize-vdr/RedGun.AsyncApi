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
        private static FixedFieldMap<AsyncApiCorrelationId> _correlationIdFixedFields = new FixedFieldMap<AsyncApiCorrelationId>()
        {
            {
                AsyncApiConstants.Description, (o, n) =>
                {
                    o.Description = n.GetScalarValue();
                }
            },
            {
                AsyncApiConstants.Location, (o, n) =>
                {
                    o.Location = n.GetScalarValue();
                }
            },

        };

        private static PatternFieldMap<AsyncApiCorrelationId> _correlationIdPatternFields =
            new PatternFieldMap<AsyncApiCorrelationId> {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p, n))}
            };

        public static AsyncApiCorrelationId LoadCorrelationId(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.CorrelationId);
            
            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                return mapNode.GetReferencedObject<AsyncApiCorrelationId>(ReferenceType.CorrelationId, pointer);
            }

            var domainObject = new AsyncApiCorrelationId();

            ParseMap(mapNode, domainObject, _correlationIdFixedFields, _correlationIdPatternFields);

            return domainObject;
        }
    }
}
