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
        private static readonly FixedFieldMap<AsyncApiOperationBindings> _operationBindingsFixedFields = new FixedFieldMap<AsyncApiOperationBindings>()
        {
            {
                AsyncApiConstants.BindingHttp, (o, n) =>
                {
                    o.BindingHttp = LoadBindingHttpOperation(n);
                }
            },
            {
                AsyncApiConstants.BindingKafka, (o, n) =>
                {
                    o.BindingKafka = LoadBindingKafkaOperation(n);
                }
            }

        };

        private static readonly PatternFieldMap<AsyncApiOperationBindings> _operationBindingsPatternFields =
            new PatternFieldMap<AsyncApiOperationBindings> {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p, n))}
            };

        public static AsyncApiOperationBindings LoadOperationBindings(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.OperationBindings);
            
            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                return mapNode.GetReferencedObject<AsyncApiOperationBindings>(ReferenceType.OperationBindings, pointer);
            }

            var domainObject = new AsyncApiOperationBindings();

            ParseMap(mapNode, domainObject, _operationBindingsFixedFields, _operationBindingsPatternFields);

            return domainObject;
        }
    }
}
