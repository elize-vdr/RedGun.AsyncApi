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
        private static readonly FixedFieldMap<AsyncApiMessageTrait> _messageTraitFixedFields =
            new FixedFieldMap<AsyncApiMessageTrait>
            {
                {
                    AsyncApiConstants.Headers, (o, n) =>
                    {
                        o.Headers = LoadSchema(n);
                    }
                },
                {
                    AsyncApiConstants.CorrelationId, (o, n) =>
                    {
                        o.CorrelationId = LoadCorrelationId(n);
                    }
                },
                {
                    AsyncApiConstants.SchemaFormat, (o, n) =>
                    {
                        o.SchemaFormat = n.GetScalarValue();
                    }
                },
                {
                    AsyncApiConstants.ContentType, (o, n) =>
                    {
                        o.ContentType = n.GetScalarValue();
                    }
                },
                {
                    AsyncApiConstants.Name, (o, n) =>
                    {
                        o.Name = n.GetScalarValue();
                    }
                },
                {
                    AsyncApiConstants.Title, (o, n) =>
                    {
                        o.Title = n.GetScalarValue();
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
                        o.Bindings = LoadMessageBindings(n);
                    }
                },
                {
                    AsyncApiConstants.Examples, (o, n) =>
                    {
                        o.Examples = n.CreateList(LoadMessageExample);
                    }
                }
            };

        private static readonly PatternFieldMap<AsyncApiMessageTrait> _messageTraitPatternFields =
            new PatternFieldMap<AsyncApiMessageTrait>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))},
            };

        internal static AsyncApiMessageTrait LoadMessageTrait(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.MessageTrait);

            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                return mapNode.GetReferencedObject<AsyncApiMessageTrait>(ReferenceType.MessageTrait, pointer);
            }

            var messageTrait = new AsyncApiMessageTrait();

            ParseMap(mapNode, messageTrait, _messageTraitFixedFields, _messageTraitPatternFields);

            return messageTrait;
        }
        
    }
}
