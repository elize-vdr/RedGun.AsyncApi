﻿// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Linq;
using RedGun.AsyncApi.Exceptions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Services;

namespace RedGun.AsyncApi.Services
{
    /// <summary>
    /// This class is used to walk an AsyncApiDocument and convert unresolved references to references to populated objects
    /// </summary>
    internal class AsyncApiReferenceResolver : AsyncApiVisitorBase
    {
        private AsyncApiDocument _currentDocument;
        private bool _resolveRemoteReferences;
        private List<AsyncApiError> _errors = new List<AsyncApiError>();

        public AsyncApiReferenceResolver(AsyncApiDocument currentDocument, bool resolveRemoteReferences = true)
        {
            _currentDocument = currentDocument;
            _resolveRemoteReferences = resolveRemoteReferences;
        }

        public IEnumerable<AsyncApiError> Errors
        {
            get
            {
                return _errors;
            }
        }
        
        // TODO: Remove here what we no longer need

        public override void Visit(AsyncApiDocument doc)
        {
            if (doc.Tags != null)
            {
                ResolveTags(doc.Tags);
            }
        }

        public override void Visit(AsyncApiComponents components)
        {
            ResolveMap(components.Schemas);
            ResolveMap(components.Messages);
            ResolveMap(components.SecuritySchemes);
            ResolveMap(components.Parameters);
            ResolveMap(components.CorrelationIds);
            ResolveMap(components.OperationTraits);
            ResolveMap(components.MessageTraits);
            ResolveMap(components.ServerBindings);
            ResolveMap(components.ChannelBindings);
            ResolveMap(components.OperationBindings);
            ResolveMap(components.MessageBindings);
        }

        public override void Visit(IDictionary<string, AsyncApiCallback> callbacks)
        {
            ResolveMap(callbacks);
        }
        
        /// <summary>
        /// Resolve all references used in a Channel Item
        /// </summary>
        public override void Visit(AsyncApiChannelItem channelItem)
        {
            ResolveObject(channelItem.Bindings, r => channelItem.Bindings = r);
        }

        /// <summary>
        /// Resolve all references used in an operation
        /// </summary>
        public override void Visit(AsyncApiOperation operation)
        {
            ResolveObject(operation.Bindings, r => operation.Bindings = r);
            ResolveList(operation.Traits);
            ResolveObject(operation.Message, m => operation.Message = m);

            if (operation.Tags != null)
            {
                ResolveTags(operation.Tags);
            }
        }
        
        /// <summary>
        /// Resolve all references used in an operation trait
        /// </summary>
        public override void Visit(AsyncApiOperationTrait operationTrait)
        {
            ResolveObject(operationTrait.Bindings, r => operationTrait.Bindings = r);

            if (operationTrait.Tags != null)
            {
                ResolveTags(operationTrait.Tags);
            }
        }
        
        /// <summary>
        /// Resolve all references used in a message
        /// </summary>
        public override void Visit(AsyncApiMessage message)
        {
            ResolveObject(message.Headers, r => message.Headers = r);
            ResolveObject(message.CorrelationId, r => message.CorrelationId = r);
            ResolveObject(message.Bindings, r => message.Bindings = r);
            ResolveList(message.Traits);

            if (message.Tags != null)
            {
                ResolveTags(message.Tags);
            }
        }
        
        /// <summary>
        /// Resolve all references used in a message
        /// </summary>
        public override void Visit(AsyncApiMessageTrait messageTrait)
        {
            ResolveObject(messageTrait.Headers, r => messageTrait.Headers = r);
            ResolveObject(messageTrait.CorrelationId, r => messageTrait.CorrelationId = r);
            ResolveObject(messageTrait.Bindings, r => messageTrait.Bindings = r);

            if (messageTrait.Tags != null)
            {
                ResolveTags(messageTrait.Tags);
            }
        }

        /// <summary>
        /// Resolve all references using in mediaType object
        /// </summary>
        /// <param name="mediaType"></param>
        public override void Visit(AsyncApiMediaType mediaType)
        {
            ResolveObject(mediaType.Schema, r => mediaType.Schema = r);
        }

        /// <summary>
        /// Resolve all references to examples
        /// </summary>
        /// <param name="examples"></param>
        public override void Visit(IDictionary<string, AsyncApiExample> examples)
        {
            ResolveMap(examples);
        }

        /// <summary>
        /// Resolve all references to responses
        /// </summary>
        public override void Visit(AsyncApiResponses responses)
        {
            ResolveMap(responses);
        }

        /// <summary>
        /// Resolve all references to headers
        /// </summary>
        /// <param name="headers"></param>
        public override void Visit(IDictionary<string, AsyncApiHeader> headers)
        {
            ResolveMap(headers);
        }

        /// <summary>
        /// Resolve all references to SecuritySchemes
        /// </summary>
        public override void Visit(AsyncApiSecurityRequirement securityRequirement)
        {
            foreach (var scheme in securityRequirement.Keys.ToList())
            {
                ResolveObject(scheme, (resolvedScheme) =>
                {
                    if (resolvedScheme != null)
                    {
                        // If scheme was unresolved
                        // copy Scopes and remove old unresolved scheme
                        var scopes = securityRequirement[scheme];
                        securityRequirement.Remove(scheme);
                        securityRequirement.Add(resolvedScheme, scopes);
                    }
                });
            }
        }

        /// <summary>
        /// Resolve all references to parameters
        /// </summary>
        public override void Visit(IList<AsyncApiParameter> parameters)
        {
            ResolveList(parameters);
        }

        /// <summary>
        /// Resolve all references used in a parameter
        /// </summary>
        public override void Visit(AsyncApiParameter parameter)
        {
            ResolveObject(parameter.Schema, r => parameter.Schema = r);
        }


        /// <summary>
        /// Resolve all references to links
        /// </summary>
        public override void Visit(IDictionary<string, AsyncApiLink> links)
        {
            ResolveMap(links);
        }

        /// <summary>
        /// Resolve all references used in a schema
        /// </summary>
        public override void Visit(AsyncApiSchema schema)
        {
            ResolveObject(schema.Items, r => schema.Items = r);
            ResolveList(schema.OneOf);
            ResolveList(schema.AllOf);
            ResolveList(schema.AnyOf);
            ResolveMap(schema.Properties);
            ResolveObject(schema.AdditionalProperties, r => schema.AdditionalProperties = r);
        }

        /// <summary>
        /// Replace references to tags with either tag objects declared in components, or inline tag object
        /// </summary>
        private void ResolveTags(IList<AsyncApiTag> tags)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                var tag = tags[i];
                if (IsUnresolvedReference(tag))
                {
                    var resolvedTag = ResolveReference<AsyncApiTag>(tag.Reference);

                    if (resolvedTag == null)
                    {
                        resolvedTag = new AsyncApiTag()
                        {
                            Name = tag.Reference.Id
                        };
                    }
                    tags[i] = resolvedTag;
                }
            }
        }

        private void ResolveObject<T>(T entity, Action<T> assign) where T : class, IAsyncApiReferenceable, new()
        {
            if (entity == null) return;

            if (IsUnresolvedReference(entity))
            {
                assign(ResolveReference<T>(entity.Reference));
            }
        }

        private void ResolveList<T>(IList<T> list) where T : class, IAsyncApiReferenceable, new()
        {
            if (list == null) return;

            for (int i = 0; i < list.Count; i++)
            {
                var entity = list[i];
                if (IsUnresolvedReference(entity))
                {
                    list[i] = ResolveReference<T>(entity.Reference);
                }
            }
        }

        private void ResolveMap<T>(IDictionary<string, T> map) where T : class, IAsyncApiReferenceable, new()
        {
            if (map == null) return;

            foreach (var key in map.Keys.ToList())
            {
                var entity = map[key];
                if (IsUnresolvedReference(entity))
                {
                    map[key] = ResolveReference<T>(entity.Reference);
                }
            }
        }

        private T ResolveReference<T>(AsyncApiReference reference) where T : class, IAsyncApiReferenceable, new()
        {
            if (string.IsNullOrEmpty(reference.ExternalResource))
            {
                try
                {
                    return _currentDocument.ResolveReference(reference) as T;
                }
                catch (AsyncApiException ex)
                {
                    _errors.Add(new AsyncApiReferenceError(ex));
                    return null;
                }
            }
            else if (_resolveRemoteReferences == true)
            {
                if (_currentDocument.Workspace == null)
                {
                    _errors.Add(new AsyncApiReferenceError(reference,"Cannot resolve external references for documents not in workspaces."));
                    // Leave as unresolved reference
                    return new T()
                    {
                        UnresolvedReference = true,
                        Reference = reference
                    };
                }
                var target = _currentDocument.Workspace.ResolveReference(reference);

                // TODO:  If it is a document fragment, then we should resolve it within the current context

                return target as T;
            }
            else
            {
                // Leave as unresolved reference
                return new T()
                {
                    UnresolvedReference = true,
                    Reference = reference
                };
            }
        }

        private bool IsUnresolvedReference(IAsyncApiReferenceable possibleReference)
        {
            return (possibleReference != null && possibleReference.UnresolvedReference);
        }
    }
}
