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
        private static readonly FixedFieldMap<AsyncApiBindingKafkaOperation> _channelBindingKafkaOperationFixedFields = new FixedFieldMap<AsyncApiBindingKafkaOperation>
        {
            {
                AsyncApiConstants.GroupId, (o, n) =>
                {
                    o.GroupId = LoadSchema(n);;
                }
            },
            {
                AsyncApiConstants.ClientId, (o, n) =>
                {
                    o.ClientId = LoadSchema(n);
                }
            },
            {
                AsyncApiConstants.BindingVersion, (o, n) =>
                {
                    o.BindingVersion = n.GetScalarValue();
                }
            }
        };

        private static readonly PatternFieldMap<AsyncApiBindingKafkaOperation> _channelBindingKafkaOperationPatternFields = new PatternFieldMap<AsyncApiBindingKafkaOperation>
        {
            {
                s => s.StartsWith("x-"),
                (o, p, n) =>
                    o.AddExtension(p,
                        LoadExtension(p, n))
            }
        };

        public static AsyncApiBindingKafkaOperation LoadBindingKafkaOperation(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.BindingKafka);

            var operationBindingHttp = new AsyncApiBindingKafkaOperation();

            ParseMap(mapNode, operationBindingHttp, _channelBindingKafkaOperationFixedFields, _channelBindingKafkaOperationPatternFields);

            return operationBindingHttp;
        }
    }
}
