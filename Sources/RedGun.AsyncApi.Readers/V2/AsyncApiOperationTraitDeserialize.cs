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
        private static readonly FixedFieldMap<AsyncApiOperationTrait> _operationTraitFixedFields =
            new FixedFieldMap<AsyncApiOperationTrait>
            {
                {
                    AsyncApiConstants.Summary, (o, n) =>
                    {
                        o.Summary = n.GetScalarValue();
                    }
                },
                {
                    AsyncApiConstants.Description, (o, n) =>
                    {
                        o.Description = n.GetScalarValue();
                    }
                },
                {
                    AsyncApiConstants.Tags, (o, n) =>
                    {
                        o.Tags = n.CreateList(LoadTagByReference);
                    }
                },
                {
                    AsyncApiConstants.ExternalDocs, (o, n) =>
                    {
                        o.ExternalDocs = LoadExternalDocs(n);
                    }
                },
                {
                    AsyncApiConstants.Bindings, (o, n) =>
                    {
                        o.Bindings = LoadOperationBindings(n);
                    }
                }
            };

        private static readonly PatternFieldMap<AsyncApiOperationTrait> _operationTraitPatternFields =
            new PatternFieldMap<AsyncApiOperationTrait>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))},
            };

        internal static AsyncApiOperationTrait LoadOperationTrait(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.OperationTrait);
            
            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                return mapNode.GetReferencedObject<AsyncApiOperationTrait>(ReferenceType.OperationTrait, pointer);
            }

            var operationTrait = new AsyncApiOperationTrait();

            ParseMap(mapNode, operationTrait, _operationTraitFixedFields, _operationTraitPatternFields);

            return operationTrait;
        }
    }
}
