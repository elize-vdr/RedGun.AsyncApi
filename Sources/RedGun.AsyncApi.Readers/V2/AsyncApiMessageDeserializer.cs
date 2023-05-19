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
        private static readonly FixedFieldMap<AsyncApiMessage> _messageFixedFields =
            new FixedFieldMap<AsyncApiMessage>
            {
                {
                    AsyncApiConstants.Headers, (o, n) =>
                    {
                        o.Headers = LoadSchema(n);
                    }
                },
                {
                    // TODO: not sure if this will work, how to deserialize "any"?
                    AsyncApiConstants.Payload, (o, n) =>
                    {
                        o.Payload = LoadAny(n);
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
                },
                {
                    AsyncApiConstants.Traits, (o, n) =>
                    {
                        o.Traits = n.CreateList(LoadMessageTrait);
                    }
                },
                {
                    AsyncApiConstants.OneOf, (o, n) =>
                    {
                        o.OneOf = n.CreateList(LoadMessage);
                    }
                }
            };

        private static readonly PatternFieldMap<AsyncApiMessage> _messagePatternFields =
            new PatternFieldMap<AsyncApiMessage>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))},
            };

        internal static AsyncApiMessage LoadMessage(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.Message);
            
            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                return mapNode.GetReferencedObject<AsyncApiMessage>(ReferenceType.Message, pointer);
            }

            var message = new AsyncApiMessage();

            ParseMap(mapNode, message, _messageFixedFields, _messagePatternFields);

            return message;
        }
        
    }
}
