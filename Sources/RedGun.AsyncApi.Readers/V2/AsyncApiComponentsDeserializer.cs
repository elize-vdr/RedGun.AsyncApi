// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK.
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Interfaces;
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
        private static FixedFieldMap<AsyncApiComponents> _componentsFixedFields = new FixedFieldMap<AsyncApiComponents>
        {
            {
                AsyncApiConstants.Schemas, (o, n) =>
                {
                    o.Schemas = n.CreateMapWithReference(ReferenceType.Schema, LoadSchema);
                }
            },
            {
                AsyncApiConstants.Messages, (o, n) =>
                {
                    o.Messages = n.CreateMapWithReference(ReferenceType.Message, LoadMessage);
                }
            },
            {
                AsyncApiConstants.Parameters, (o, n) =>
                {
                    o.Parameters = n.CreateMapWithReference(ReferenceType.Parameter, LoadParameter);
                }
            },
            {
                AsyncApiConstants.CorrelationIds, (o, n) =>
                {
                    o.CorrelationIds = n.CreateMapWithReference(ReferenceType.CorrelationId, LoadCorrelationId);
                }
            },
            {
                AsyncApiConstants.OperationTraits, (o, n) =>
                {
                    o.OperationTraits = n.CreateMapWithReference(ReferenceType.OperationTrait, LoadOperationTrait);
                }
            },
            {
                AsyncApiConstants.MessageTraits, (o, n) =>
                {
                    o.MessageTraits = n.CreateMapWithReference(ReferenceType.MessageTrait, LoadMessageTrait);
                }
            },
            {
                AsyncApiConstants.ServerBindings, (o, n) =>
                {
                    o.ServerBindings = n.CreateMapWithReference(ReferenceType.ServerBindings, LoadServerBindings);
                }
            },
            {
                AsyncApiConstants.ChannelBindings, (o, n) =>
                {
                    o.ChannelBindings = n.CreateMapWithReference(ReferenceType.ChannelBindings, LoadChannelBindings);
                }
            },
            {
                AsyncApiConstants.OperationBindings, (o, n) =>
                {
                    o.OperationBindings = n.CreateMapWithReference(ReferenceType.OperationBindings, LoadOperationBindings);
                }
            },
            {
                AsyncApiConstants.MessageBindings, (o, n) =>
                {
                    o.MessageBindings = n.CreateMapWithReference(ReferenceType.MessageBindings, LoadMessageBindings);
                }
            },
        };


        private static PatternFieldMap<AsyncApiComponents> _componentsPatternFields =
            new PatternFieldMap<AsyncApiComponents>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p, n))}
            };

        public static AsyncApiComponents LoadComponents(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.Components);
            var components = new AsyncApiComponents();

            ParseMap(mapNode, components, _componentsFixedFields, _componentsPatternFields);

            return components;
        }
    }
}
