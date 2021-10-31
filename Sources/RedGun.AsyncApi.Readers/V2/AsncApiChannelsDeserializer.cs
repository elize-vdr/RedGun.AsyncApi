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
        private static FixedFieldMap<AsyncApiChannels> _channelsFixedFields = new FixedFieldMap<AsyncApiChannels>();

        private static PatternFieldMap<AsyncApiChannels> _channelsPatternFields =
            new PatternFieldMap<AsyncApiChannels> {
                {s => !s.StartsWith("x-"), (o,  k, n) => o.Add(k, LoadChannelItem(n))},
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p, n))}
            };

        public static AsyncApiChannels LoadChannels(ParseNode node)
        {
            var mapNode = node.CheckMapNode("Channels");

            var domainObject = new AsyncApiChannels();

            ParseMap(mapNode, domainObject, _channelsFixedFields, _channelsPatternFields);

            return domainObject;
        }
    }
}
