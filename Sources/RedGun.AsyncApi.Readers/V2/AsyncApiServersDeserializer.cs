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
        private static FixedFieldMap<AsyncApiServers> _serversFixedFields = new FixedFieldMap<AsyncApiServers>();

        private static PatternFieldMap<AsyncApiServers> _serversPatternFields =
            new PatternFieldMap<AsyncApiServers> {
                                                     {s => !s.StartsWith("x-"), (o,  k, n) => o.Add(k, LoadServer(n))},
                                                     {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p, n))}
                                                 };

        public static AsyncApiServers LoadServers(ParseNode node)
        {
            var mapNode = node.CheckMapNode("Servers");

            var domainObject = new AsyncApiServers();

            ParseMap(mapNode, domainObject, _serversFixedFields, _serversPatternFields);

            return domainObject;
        }
    }
}