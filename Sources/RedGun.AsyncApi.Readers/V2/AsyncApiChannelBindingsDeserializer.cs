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
        private static FixedFieldMap<AsyncApiChannelBindings> _channelBindingsFixedFields = new FixedFieldMap<AsyncApiChannelBindings>()
        {
            // http: Not currently defined for Channel.
            
            // ws
            {
                AsyncApiConstants.BindingWebSockets, (o, n) =>
                {
                    o.BindingWebSockets = LoadBindingWebSocketsChannel(n);
                }
            },
            
            // TODO: Add rest of bindings here
        };

        private static PatternFieldMap<AsyncApiChannelBindings> _channelBindingsPatternFields =
            new PatternFieldMap<AsyncApiChannelBindings> {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p, n))}
            };

        public static AsyncApiChannelBindings LoadChannelBindings(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.ChannelBindings);
            
            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                return mapNode.GetReferencedObject<AsyncApiChannelBindings>(ReferenceType.ChannelBindings, pointer);
            }

            var domainObject = new AsyncApiChannelBindings();

            ParseMap(mapNode, domainObject, _channelBindingsFixedFields, _channelBindingsPatternFields);

            return domainObject;
        }
    }
}
