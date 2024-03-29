﻿// Licensed under the MIT license. 

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
        private static readonly FixedFieldMap<AsyncApiOperation> _operationFixedFields =
            new FixedFieldMap<AsyncApiOperation>
            {
                {
                    AsyncApiConstants.OperationId, (o, n) =>
                    {
                        o.OperationId = n.GetScalarValue();
                    }
                },
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
                },
               {
                   AsyncApiConstants.Traits, (o, n) =>
                    {
                        o.Traits = n.CreateList(LoadOperationTrait);
                    }
                },
                {
                    AsyncApiConstants.Message, (o, n) =>
                    {
                        o.Message = LoadMessage(n);
                    }
                }
            };

        private static readonly PatternFieldMap<AsyncApiOperation> _operationPatternFields =
            new PatternFieldMap<AsyncApiOperation>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))},
            };

        internal static AsyncApiOperation LoadOperation(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.Operation);

            var operation = new AsyncApiOperation();

            ParseMap(mapNode, operation, _operationFixedFields, _operationPatternFields);

            return operation;
        }
        
    }
}
