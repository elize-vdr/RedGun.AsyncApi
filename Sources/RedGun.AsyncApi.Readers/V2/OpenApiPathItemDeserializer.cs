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
        private static readonly FixedFieldMap<AsyncApiPathItem> _pathItemFixedFields = new FixedFieldMap<AsyncApiPathItem>
        {
            
            {
                "$ref", (o,n) => {
                    o.Reference = new AsyncApiReference() { ExternalResource = n.GetScalarValue() };
                    o.UnresolvedReference =true;
                }  
            },
            {
                "summary", (o, n) =>
                {
                    o.Summary = n.GetScalarValue();
                }
            },
            {
                "description", (o, n) =>
                {
                    o.Description = n.GetScalarValue();
                }
            },
            {"get", (o, n) => o.AddOperation(OperationType.Get, LoadOperation(n))},
            {"put", (o, n) => o.AddOperation(OperationType.Put, LoadOperation(n))},
            {"post", (o, n) => o.AddOperation(OperationType.Post, LoadOperation(n))},
            {"delete", (o, n) => o.AddOperation(OperationType.Delete, LoadOperation(n))},
            {"options", (o, n) => o.AddOperation(OperationType.Options, LoadOperation(n))},
            {"head", (o, n) => o.AddOperation(OperationType.Head, LoadOperation(n))},
            {"patch", (o, n) => o.AddOperation(OperationType.Patch, LoadOperation(n))},
            {"trace", (o, n) => o.AddOperation(OperationType.Trace, LoadOperation(n))},
            {"servers", (o, n) => o.Servers = n.CreateList(LoadServer)},
            {"parameters", (o, n) => o.Parameters = n.CreateList(LoadParameter)}
        };

        private static readonly PatternFieldMap<AsyncApiPathItem> _pathItemPatternFields =
            new PatternFieldMap<AsyncApiPathItem>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
            };

        public static AsyncApiPathItem LoadPathItem(ParseNode node)
        {
            var mapNode = node.CheckMapNode("PathItem");

            var pathItem = new AsyncApiPathItem();

            ParseMap(mapNode, pathItem, _pathItemFixedFields, _pathItemPatternFields);

            return pathItem;
        }
    }
}
