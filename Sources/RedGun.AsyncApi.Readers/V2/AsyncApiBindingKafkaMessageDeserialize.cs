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
        private static readonly FixedFieldMap<AsyncApiBindingKafkaMessage> _channelBindingKafkaMessageFixedFields = new FixedFieldMap<AsyncApiBindingKafkaMessage>
        {
            {
                AsyncApiConstants.Key, (o, n) =>
                {
                    o.Key = LoadSchema(n);
                }
            },
            {
                AsyncApiConstants.BindingVersion, (o, n) =>
                {
                    o.BindingVersion = n.GetScalarValue();
                }
            }
        };

        private static readonly PatternFieldMap<AsyncApiBindingKafkaMessage> _channelBindingKafkaMessagePatternFields = new PatternFieldMap<AsyncApiBindingKafkaMessage>
        {
            {
                s => s.StartsWith("x-"),
                (o, p, n) =>
                    o.AddExtension(p,
                        LoadExtension(p, n))
            }
        };

        public static AsyncApiBindingKafkaMessage LoadBindingKafkaMessage(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.BindingKafka);

            var operationBindingHttp = new AsyncApiBindingKafkaMessage();

            ParseMap(mapNode, operationBindingHttp, _channelBindingKafkaMessageFixedFields, _channelBindingKafkaMessagePatternFields);

            return operationBindingHttp;
        }
    }
}
