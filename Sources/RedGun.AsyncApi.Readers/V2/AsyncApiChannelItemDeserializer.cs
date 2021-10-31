// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK.
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
        private static readonly FixedFieldMap<AsyncApiChannelItem> _channelItemFixedFields = new FixedFieldMap<AsyncApiChannelItem>
        {
            
            {
                "$ref", (o,n) => {
                    o.Reference = new AsyncApiReference() { ExternalResource = n.GetScalarValue() };
                    o.UnresolvedReference =true;
                }  
            },
            {
                "description", (o, n) =>
                {
                    o.Description = n.GetScalarValue();
                }
            },
            {
                "servers", (o, n) =>
                {
                    o.Servers = n.CreateSimpleList(s => s.GetScalarValue());
                }
            },
            
            // TODO: add subscribe, publish, parameters, and bindings
          
            //{"parameters", (o, n) => o.Parameters = n.CreateList(LoadParameter)}
        };

        private static readonly PatternFieldMap<AsyncApiChannelItem> _channelItemPatternFields =
            new PatternFieldMap<AsyncApiChannelItem>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
            };

        public static AsyncApiChannelItem LoadChannelItem(ParseNode node)
        {
            var mapNode = node.CheckMapNode("Channel Item");

            var channelItem = new AsyncApiChannelItem();

            ParseMap(mapNode, channelItem, _channelItemFixedFields, _channelItemPatternFields);

            return channelItem;
        }
    }
}
