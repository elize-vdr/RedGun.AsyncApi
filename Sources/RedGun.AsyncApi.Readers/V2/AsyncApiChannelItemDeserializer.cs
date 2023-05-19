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
                AsyncApiConstants.DollarRef, (o,n) => {
                    o.Reference = new AsyncApiReference() { ExternalResource = n.GetScalarValue() };
                    o.UnresolvedReference =true;
                }  
            },
            {
                AsyncApiConstants.Description, (o, n) =>
                {
                    o.Description = n.GetScalarValue();
                }
            },
            {
                AsyncApiConstants.Servers, (o, n) =>
                {
                    o.Servers = n.CreateSimpleList(s => s.GetScalarValue());
                }
            },
            {
                AsyncApiConstants.Subscribe, (o, n) =>
                {
                    o.Subscribe = LoadOperation(n);
                }
            },
            {
                AsyncApiConstants.Publish, (o, n) =>
                {
                    o.Publish = LoadOperation(n);
                }
            },
            {
                AsyncApiConstants.Parameters, (o, n) =>
                {
                    o.Parameters = LoadParameters(n);
                }
            },
            {
                AsyncApiConstants.Bindings, (o, n) =>
                {
                    o.Bindings = LoadChannelBindings(n);
                }
            }
            
            
        };

        private static readonly PatternFieldMap<AsyncApiChannelItem> _channelItemPatternFields =
            new PatternFieldMap<AsyncApiChannelItem>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
            };

        public static AsyncApiChannelItem LoadChannelItem(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.Channel);

            var channelItem = new AsyncApiChannelItem();

            ParseMap(mapNode, channelItem, _channelItemFixedFields, _channelItemPatternFields);

            return channelItem;
        }
    }
}
