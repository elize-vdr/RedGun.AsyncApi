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
        private static FixedFieldMap<AsyncApiOperationBindings> _operationBindingsFixedFields = new FixedFieldMap<AsyncApiOperationBindings>()
        {
            // http
            {
                "http", (o, n) =>
                {
                    o.BindingHttp = LoadBindingHttpOperation(n);
                }
            },
            
            // ws: Not currently defined for Operations
            
            // TODO: Add rest of bindings here
        };

        private static PatternFieldMap<AsyncApiOperationBindings> _operationBindingsPatternFields =
            new PatternFieldMap<AsyncApiOperationBindings> {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p, n))}
            };

        public static AsyncApiOperationBindings LoadOperationBindings(ParseNode node)
        {
            var mapNode = node.CheckMapNode("Operation Bindings");

            var domainObject = new AsyncApiOperationBindings();

            ParseMap(mapNode, domainObject, _operationBindingsFixedFields, _operationBindingsPatternFields);

            return domainObject;
        }
    }
}
