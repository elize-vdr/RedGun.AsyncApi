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
        private static FixedFieldMap<AsyncApiMessageBindings> _messageBindingsFixedFields = new FixedFieldMap<AsyncApiMessageBindings>()
        {
            {
                AsyncApiConstants.BindingKafka, (o, n) =>
                {
                    o.BindingKafka = LoadBindingKafkaMessage(n);
                }
            }

        };

        private static PatternFieldMap<AsyncApiMessageBindings> _messageBindingsPatternFields =
            new PatternFieldMap<AsyncApiMessageBindings> {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p, n))}
            };

        public static AsyncApiMessageBindings LoadMessageBindings(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.MessageBindings);

            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                return mapNode.GetReferencedObject<AsyncApiMessageBindings>(ReferenceType.MessageBindings, pointer);
            }

            var domainObject = new AsyncApiMessageBindings();
            
            ParseMap(mapNode, domainObject, _messageBindingsFixedFields, _messageBindingsPatternFields);

            return domainObject;
        }
    }
}
