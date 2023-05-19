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
        private static readonly FixedFieldMap<AsyncApiServerBindings> _serverBindingsFixedFields = new FixedFieldMap<AsyncApiServerBindings>()
        {
            // TODO: No bindings currently required to implement here
        };

        private static readonly PatternFieldMap<AsyncApiServerBindings> _serverBindingsPatternFields =
            new PatternFieldMap<AsyncApiServerBindings> {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p, n))}
            };

        public static AsyncApiServerBindings LoadServerBindings(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.ServerBindings);
            
            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                return mapNode.GetReferencedObject<AsyncApiServerBindings>(ReferenceType.ServerBindings, pointer);
            }

            var domainObject = new AsyncApiServerBindings();

            ParseMap(mapNode, domainObject, _serverBindingsFixedFields, _serverBindingsPatternFields);

            return domainObject;
        }
    }
}
