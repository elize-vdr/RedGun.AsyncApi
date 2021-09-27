// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.ParseNodes;

namespace RedGun.AsyncApi.Readers.V2
{
    /// <summary>
    /// Class containing logic to deserialize Async API V3 document into
    /// runtime Async API object model.
    /// </summary>
    internal static partial class AsyncApiV2Deserializer
    {
        private static FixedFieldMap<AsyncApiPaths> _pathsFixedFields = new FixedFieldMap<AsyncApiPaths>();

        private static PatternFieldMap<AsyncApiPaths> _pathsPatternFields = new PatternFieldMap<AsyncApiPaths>
        {
            {s => s.StartsWith("/"), (o, k, n) => o.Add(k, LoadPathItem(n))},
            {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
        };

        public static AsyncApiPaths LoadPaths(ParseNode node)
        {
            var mapNode = node.CheckMapNode("Paths");

            var domainObject = new AsyncApiPaths();

            ParseMap(mapNode, domainObject, _pathsFixedFields, _pathsPatternFields);

            return domainObject;
        }
    }
}
