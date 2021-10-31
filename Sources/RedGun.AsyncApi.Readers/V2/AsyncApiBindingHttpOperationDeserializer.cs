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
        private static readonly FixedFieldMap<AsyncApiBindingHttpOperation> _channelBindingHttpOperationFixedFields = new FixedFieldMap<AsyncApiBindingHttpOperation>
        {
            {
                "type", (o, n) =>
                {
                    o.Type = n.GetScalarValue();
                }
            },
            {
                "method", (o, n) =>
                {
                    o.Method = n.GetScalarValue();
                }
            },
            {
                "query", (o, n) =>
                {
                    o.Query = LoadSchema(n);;
                }
            },
            {
                "bindingVersion", (o, n) =>
                {
                    o.BindingVersion = n.GetScalarValue();
                }
            }
        };

        private static readonly PatternFieldMap<AsyncApiBindingHttpOperation> _channelBindingHttpOperationPatternFields = new PatternFieldMap<AsyncApiBindingHttpOperation>
        {
            {
                s => s.StartsWith("x-"),
                (o, p, n) =>
                    o.AddExtension(p,
                        LoadExtension(p, n))
            }
        };

        public static AsyncApiBindingHttpOperation LoadBindingHttpOperation(ParseNode node)
        {
            var mapNode = node.CheckMapNode("Operation Binding HTTP");

            var operationBindingHttp = new AsyncApiBindingHttpOperation();

            ParseMap(mapNode, operationBindingHttp, _channelBindingHttpOperationFixedFields, _channelBindingHttpOperationPatternFields);

            return operationBindingHttp;
        }
    }
}
