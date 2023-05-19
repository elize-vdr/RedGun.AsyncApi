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
        private static readonly FixedFieldMap<AsyncApiBindingWebSocketsChannel> _channelBindingWebSocketsChannelFixedFields = new FixedFieldMap<AsyncApiBindingWebSocketsChannel>
        {
            {
                AsyncApiConstants.Method, (o, n) =>
                {
                    o.Method = n.GetScalarValue();
                }
            },
            {
                AsyncApiConstants.Query, (o, n) =>
                {
                    o.Query = LoadSchema(n);;
                }
            },
            {
                AsyncApiConstants.Headers, (o, n) =>
                {
                    o.Headers = LoadSchema(n);;
                }
            },
            {
                AsyncApiConstants.BindingVersion, (o, n) =>
                {
                    o.BindingVersion = n.GetScalarValue();
                }
            }
        };

        private static readonly PatternFieldMap<AsyncApiBindingWebSocketsChannel> _channelBindingWebSocketsChannelPatternFields = new PatternFieldMap<AsyncApiBindingWebSocketsChannel>
        {
            {
                s => s.StartsWith("x-"),
                (o, p, n) =>
                    o.AddExtension(p,
                        LoadExtension(p, n))
            }
        };

        public static AsyncApiBindingWebSocketsChannel LoadBindingWebSocketsChannel(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.BindingWebSockets);

            var channelBindingWebSockets = new AsyncApiBindingWebSocketsChannel();

            ParseMap(mapNode, channelBindingWebSockets, _channelBindingWebSocketsChannelFixedFields, _channelBindingWebSocketsChannelPatternFields);

            return channelBindingWebSockets;
        }
    }
}
