// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Extensions;

namespace RedGun.AsyncApi.Services
{
    /// <summary>
    /// The walker to visit multiple Async API elements.
    /// </summary>
    public class AsyncApiWalker
    {
        private readonly AsyncApiVisitorBase _visitor;
        private readonly Stack<AsyncApiSchema> _schemaLoop = new Stack<AsyncApiSchema>();
        private readonly Stack<AsyncApiPathItem> _pathItemLoop = new Stack<AsyncApiPathItem>();

        /// <summary>
        /// Initializes the <see cref="AsyncApiWalker"/> class.
        /// </summary>
        public AsyncApiWalker(AsyncApiVisitorBase visitor)
        {
            _visitor = visitor;
        }

        /// <summary>
        /// Visits list of <see cref="AsyncApiDocument"/> and child objects
        /// </summary>
        /// <param name="doc">AsyncApiDocument to be walked</param>
        public void Walk(AsyncApiDocument doc)
        {
            if (doc == null)
            {
                return;
            }

            _schemaLoop.Clear();
            _pathItemLoop.Clear();

            _visitor.Visit(doc);

            Walk(AsyncApiConstants.Servers, () => Walk(doc.Servers));
            Walk(AsyncApiConstants.Info, () => Walk(doc.Info));
            Walk(AsyncApiConstants.Tags, () => Walk(doc.Tags));
            Walk(AsyncApiConstants.Channels, () => Walk(doc.Channels));
            
            // TODO: Marker for old OpenApi walkers -----------------------------------------------------
            
            Walk(AsyncApiConstants.Components, () => Walk(doc.Components));
            Walk(AsyncApiConstants.ExternalDocs, () => Walk(doc.ExternalDocs));
            Walk(doc as IAsyncApiExtensible);

        }

        /// <summary>
        /// Visits list of <see cref="AsyncApiTag"/> and child objects
        /// </summary>
        internal void Walk(IList<AsyncApiTag> tags)
        {
            if (tags == null)
            {
                return;
            }

            _visitor.Visit(tags);

            // Visit tags
            if (tags != null)
            {
                for (int i = 0; i < tags.Count; i++)
                {
                    Walk(i.ToString(), () => Walk(tags[i]));
                }
            }
        }
        
       /// <summary>
        /// Visits <see cref="AsyncApiInfo"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiInfo info)
        {
            if (info == null)
            {
                return;
            }

            _visitor.Visit(info);
            if (info != null)
            {
                Walk(AsyncApiConstants.Contact, () => Walk(info.Contact));
                Walk(AsyncApiConstants.License, () => Walk(info.License));
            }
            Walk(info as IAsyncApiExtensible);
        }

        /// <summary>
        /// Visits <see cref="AsyncApiLicense"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiLicense license)
        {
            if (license == null)
            {
                return;
            }

            _visitor.Visit(license);
        }

        /// <summary>
        /// Visits <see cref="AsyncApiContact"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiContact contact)
        {
            if (contact == null)
            {
                return;
            }

            _visitor.Visit(contact);
        }

        /// <summary>
        /// Visits <see cref="AsyncApiTag"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiTag tag)
        {
            if (tag == null || ProcessAsReference(tag))
            {
                return;
            }

            _visitor.Visit(tag);
            _visitor.Visit(tag.ExternalDocs);
            _visitor.Visit(tag as IAsyncApiExtensible);
        }

        /// <summary>
        /// Visits list of  <see cref="AsyncApiServer"/> and child objects
        /// </summary>
        internal void Walk(IList<AsyncApiServer> servers)
        {
            if (servers == null)
            {
                return;
            }

            _visitor.Visit(servers);

            // Visit Servers
            if (servers != null)
            {
                for (int i = 0; i < servers.Count; i++)
                {
                    Walk(i.ToString(), () => Walk(servers[i]));
                }
            }
        }

        /// <summary>
        /// Visits <see cref="AsyncApiServer"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiServer server)
        {
            if (server == null)
            {
                return;
            }

            _visitor.Visit(server);
            Walk(AsyncApiConstants.Variables, () => Walk(server.Variables));
            _visitor.Visit(server as IAsyncApiExtensible);
        }
        
        /// <summary>
        /// Visits dictionary of <see cref="AsyncApiServerVariable"/>
        /// </summary>
        internal void Walk(IDictionary<string, AsyncApiServerVariable> serverVariables)
        {
            if (serverVariables == null)
            {
                return;
            }

            _visitor.Visit(serverVariables);

            if (serverVariables != null)
            {
                foreach (var variable in serverVariables)
                {
                    _visitor.CurrentKeys.ServerVariable = variable.Key;
                    Walk(variable.Key, () => Walk(variable.Value));
                    _visitor.CurrentKeys.ServerVariable = null;
                }
            }
        }

        /// <summary>
        /// Visits <see cref="AsyncApiServerVariable"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiServerVariable serverVariable)
        {
            if (serverVariable == null)
            {
                return;
            }

            _visitor.Visit(serverVariable);
            _visitor.Visit(serverVariable as IAsyncApiExtensible);
        }

        /// <summary>
        /// Visits <see cref="AsyncApiExternalDocs"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiExternalDocs externalDocs)
        {
            if (externalDocs == null)
            {
                return;
            }

            _visitor.Visit(externalDocs);
        }

        /// <summary>
        /// Visits <see cref="AsyncApiSecurityScheme"/> and child objects
        /// </summary>
        internal void Walk(IAsyncApiReferenceable referenceable)
        {
            _visitor.Visit(referenceable);
        }

        /// <summary>
        /// Visits dictionary of extensions
        /// </summary>
        internal void Walk(IAsyncApiExtensible asyncApiExtensible)
        {
            if (asyncApiExtensible == null)
            {
                return;
            }

            _visitor.Visit(asyncApiExtensible);

            if (asyncApiExtensible != null)
            {
                foreach (var item in asyncApiExtensible.Extensions)
                {
                    _visitor.CurrentKeys.Extension = item.Key;
                    Walk(item.Key, () => Walk(item.Value));
                    _visitor.CurrentKeys.Extension = null;
                }
            }
        }

        /// <summary>
        /// Visits <see cref="IAsyncApiExtension"/> 
        /// </summary>
        internal void Walk(IAsyncApiExtension extension)
        {
            if (extension == null)
            {
                return;
            }

            _visitor.Visit(extension);
        }


        #region OldOpenApiWalkers
        

        /// <summary>
        /// Visits <see cref="AsyncApiComponents"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiComponents components)
        {
            if (components == null)
            {
                return;
            }

            _visitor.Visit(components);

            if (components == null)
            {
                return;
            }

            Walk(AsyncApiConstants.Schemas, () =>
            {
                if (components.Schemas != null)
                {
                    foreach (var item in components.Schemas)
                    {
                        Walk(item.Key, () => Walk(item.Value, isComponent: true));
                    }
                }
            });

            Walk(AsyncApiConstants.Callbacks, () =>
            {
                if (components.Callbacks != null)
                {
                    foreach (var item in components.Callbacks)
                    {
                        Walk(item.Key, () => Walk(item.Value, isComponent: true));
                    }
                }
            });

            Walk(AsyncApiConstants.Parameters, () =>
            {
                if (components.Parameters != null)
                {
                    foreach (var item in components.Parameters)
                    {
                        Walk(item.Key, () => Walk(item.Value, isComponent: true));
                    }
                }
            });

            Walk(AsyncApiConstants.Examples, () =>
            {
                if (components.Examples != null)
                {
                    foreach (var item in components.Examples)
                    {
                        Walk(item.Key, () => Walk(item.Value, isComponent: true));
                    }
                }
            });

            Walk(AsyncApiConstants.Headers, () =>
            {
                if (components.Headers != null)
                {
                    foreach (var item in components.Headers)
                    {
                        Walk(item.Key, () => Walk(item.Value, isComponent: true));
                    }
                }
            });

            Walk(AsyncApiConstants.Links, () =>
            {
                if (components.Links != null)
                {
                    foreach (var item in components.Links)
                    {
                        Walk(item.Key, () => Walk(item.Value, isComponent: true));
                    }
                }
            });

            Walk(AsyncApiConstants.RequestBodies, () =>
            {
                if (components.RequestBodies != null)
                {
                    foreach (var item in components.RequestBodies)
                    {
                        Walk(item.Key, () => Walk(item.Value, isComponent: true));
                    }
                }
            });

            Walk(AsyncApiConstants.Responses, () =>
            {
                if (components.Responses != null)
                {
                    foreach (var item in components.Responses)
                    {
                        Walk(item.Key, () => Walk(item.Value, isComponent: true));
                    }
                }
            });

            Walk(components as IAsyncApiExtensible);
        }
        
        /// <summary>
        /// Visits <see cref="AsyncApiChannels"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiChannels channels)
        {
            if (channels == null)
            {
                return;
            }

            _visitor.Visit(channels);

            // Visit Channels
            if (channels != null)
            {
                foreach (var channelItem in channels)
                {
                    _visitor.CurrentKeys.Channel = channelItem.Key;
                    Walk(channelItem.Key, () => Walk(channelItem.Value));
                    _visitor.CurrentKeys.Channel = null;
                }
            }
        }
        
        /// <summary>
        /// Visits <see cref="AsyncApiChannelItem"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiChannelItem channelItem)
        {
            if (channelItem == null)
            {
                return;
            }

            _visitor.Visit(channelItem);

            if (channelItem != null)
            {
                // TODO: Walk subscribe, publish, parameters bindings
                //Walk(AsyncApiConstants.Parameters, () => Walk(channelItem.Parameters));
                //Walk(channelItem.Operations);
                Walk(channelItem.Bindings);
            }
            Walk(channelItem as IAsyncApiExtensible);
        }
        
        /// <summary>
        /// Visits <see cref="AsyncApiChannelBindings"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiChannelBindings channelBindings)
        {
            if (channelBindings == null)
            {
                return;
            }

            _visitor.Visit(channelBindings);

            if (channelBindings != null)
            {
                // TODO: Walk subscribe, publish, parameters bindings
                //Walk(AsyncApiConstants.Parameters, () => Walk(channelItem.Parameters));
                //Walk(channelItem.Operations);
                Walk(channelBindings.BindingHttp);
                // TODO: Add rest of bindings
            }
            Walk(channelBindings as IAsyncApiExtensible);
        }
        
        /// <summary>
        /// Visits <see cref="AsyncApiBindingHttpOperation"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiBindingHttpOperation bindingHttpOperation)
        {
            if (bindingHttpOperation == null)
            {
                return;
            }

            _visitor.Visit(bindingHttpOperation);

            if (bindingHttpOperation != null)
            {
                Walk(AsyncApiConstants.Query, () => Walk(bindingHttpOperation.Query));
            }
            Walk(bindingHttpOperation as IAsyncApiExtensible);
        }


        /// <summary>
        /// Visits <see cref="AsyncApiPaths"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiPaths paths)
        {
            if (paths == null)
            {
                return;
            }

            _visitor.Visit(paths);

            // Visit Paths
            if (paths != null)
            {
                foreach (var pathItem in paths)
                {
                    _visitor.CurrentKeys.Path = pathItem.Key;
                    Walk(pathItem.Key, () => Walk(pathItem.Value));// JSON Pointer uses ~1 as an escape character for /
                    _visitor.CurrentKeys.Path = null;
                }
            }
        }

        /// <summary>
        /// Visits <see cref="AsyncApiCallback"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiCallback callback, bool isComponent = false)
        {
            if (callback == null || ProcessAsReference(callback, isComponent))
            {
                return;
            }

            _visitor.Visit(callback);

            if (callback != null)
            {
                foreach (var item in callback.PathItems)
                {
                    _visitor.CurrentKeys.Callback = item.Key.ToString();
                    var pathItem = item.Value;
                    Walk(item.Key.ToString(), () => Walk(pathItem));
                    _visitor.CurrentKeys.Callback = null;
                }
            }
        }

        /// <summary>
        /// Visits <see cref="AsyncApiPathItem"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiPathItem pathItem)
        {
            if (pathItem == null)
            {
                return;
            }

            if (_pathItemLoop.Contains(pathItem))
            {
                return;  // Loop detected, this pathItem has already been walked.
            }
            else
            {
                _pathItemLoop.Push(pathItem);
            }

            _visitor.Visit(pathItem);

            if (pathItem != null)
            {
                Walk(AsyncApiConstants.Parameters, () => Walk(pathItem.Parameters));
                Walk(pathItem.Operations);
            }
            _visitor.Visit(pathItem as IAsyncApiExtensible);

            _pathItemLoop.Pop();
        }

        /// <summary>
        /// Visits dictionary of <see cref="AsyncApiOperation"/>
        /// </summary>
        internal void Walk(IDictionary<OperationType, AsyncApiOperation> operations)
        {
            if (operations == null)
            {
                return;
            }

            _visitor.Visit(operations);
            if (operations != null)
            {
                foreach (var operation in operations)
                {
                    _visitor.CurrentKeys.Operation = operation.Key;
                    Walk(operation.Key.GetDisplayName(), () => Walk(operation.Value));
                    _visitor.CurrentKeys.Operation = null;
                }
            }
        }

        /// <summary>
        /// Visits <see cref="AsyncApiOperation"/> and child objects
        /// </summary>
        /// <param name="operation"></param>
        internal void Walk(AsyncApiOperation operation)
        {
            if (operation == null)
            {
                return;
            }

            _visitor.Visit(operation);

            Walk(AsyncApiConstants.Parameters, () => Walk(operation.Parameters));
            Walk(AsyncApiConstants.RequestBody, () => Walk(operation.RequestBody));
            Walk(AsyncApiConstants.Responses, () => Walk(operation.Responses));
            Walk(AsyncApiConstants.Callbacks, () => Walk(operation.Callbacks));
            Walk(AsyncApiConstants.Tags, () => Walk(operation.Tags));
            Walk(AsyncApiConstants.Security, () => Walk(operation.Security));
            Walk(operation as IAsyncApiExtensible);
        }

        /// <summary>
        /// Visits list of <see cref="AsyncApiSecurityRequirement"/>
        /// </summary>
        internal void Walk(IList<AsyncApiSecurityRequirement> securityRequirements)
        {
            if (securityRequirements == null)
            {
                return;
            }

            _visitor.Visit(securityRequirements);

            if (securityRequirements != null)
            {
                for (int i = 0; i < securityRequirements.Count; i++)
                {
                    Walk(i.ToString(), () => Walk(securityRequirements[i]));
                }
            }
        }


        /// <summary>
        /// Visits list of <see cref="AsyncApiParameter"/>
        /// </summary>
        internal void Walk(IList<AsyncApiParameter> parameters)
        {
            if (parameters == null)
            {
                return;
            }

            _visitor.Visit(parameters);

            if (parameters != null)
            {
                for (int i = 0; i < parameters.Count; i++)
                {
                    Walk(i.ToString(), () => Walk(parameters[i]));
                }
            }
        }

        /// <summary>
        /// Visits <see cref="AsyncApiParameter"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiParameter parameter, bool isComponent = false)
        {
            if (parameter == null || ProcessAsReference(parameter, isComponent))
            {
                return;
            }

            _visitor.Visit(parameter);
            Walk(AsyncApiConstants.Schema, () => Walk(parameter.Schema));
            
            Walk(parameter as IAsyncApiExtensible);
        }

        /// <summary>
        /// Visits <see cref="AsyncApiResponses"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiResponses responses)
        {
            if (responses == null)
            {
                return;
            }

            _visitor.Visit(responses);

            if (responses != null)
            {
                foreach (var response in responses)
                {
                    _visitor.CurrentKeys.Response = response.Key;
                    Walk(response.Key, () => Walk(response.Value));
                    _visitor.CurrentKeys.Response = null;
                }
            }
            Walk(responses as IAsyncApiExtensible);
        }

        /// <summary>
        /// Visits <see cref="AsyncApiResponse"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiResponse response, bool isComponent = false)
        {
            if (response == null || ProcessAsReference(response, isComponent))
            {
                return;
            }

            _visitor.Visit(response);
            Walk(AsyncApiConstants.Content, () => Walk(response.Content));
            Walk(AsyncApiConstants.Links, () => Walk(response.Links));
            Walk(AsyncApiConstants.Headers, () => Walk(response.Headers));
            Walk(response as IAsyncApiExtensible);
        }

        /// <summary>
        /// Visits <see cref="AsyncApiRequestBody"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiRequestBody requestBody, bool isComponent = false)
        {
            if (requestBody == null || ProcessAsReference(requestBody, isComponent))
            {
                return;
            }

            _visitor.Visit(requestBody);

            if (requestBody != null)
            {
                if (requestBody.Content != null)
                {
                    Walk(AsyncApiConstants.Content, () => Walk(requestBody.Content));
                }
            }
            Walk(requestBody as IAsyncApiExtensible);
        }

        /// <summary>
        /// Visits dictionary of <see cref="AsyncApiHeader"/>
        /// </summary>
        internal void Walk(IDictionary<string, AsyncApiHeader> headers)
        {
            if (headers == null)
            {
                return;
            }

            _visitor.Visit(headers);
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    _visitor.CurrentKeys.Header = header.Key;
                    Walk(header.Key, () => Walk(header.Value));
                    _visitor.CurrentKeys.Header = null;
                }
            }
        }

        /// <summary>
        /// Visits dictionary of <see cref="AsyncApiCallback"/>
        /// </summary>
        internal void Walk(IDictionary<string, AsyncApiCallback> callbacks)
        {
            if (callbacks == null)
            {
                return;
            }

            _visitor.Visit(callbacks);
            if (callbacks != null)
            {
                foreach (var callback in callbacks)
                {
                    _visitor.CurrentKeys.Callback = callback.Key;
                    Walk(callback.Key, () => Walk(callback.Value));
                    _visitor.CurrentKeys.Callback = null;
                }
            }
        }

        /// <summary>
        /// Visits dictionary of <see cref="AsyncApiMediaType"/>
        /// </summary>
        internal void Walk(IDictionary<string, AsyncApiMediaType> content)
        {
            if (content == null)
            {
                return;
            }

            _visitor.Visit(content);
            if (content != null)
            {
                foreach (var mediaType in content)
                {
                    _visitor.CurrentKeys.Content = mediaType.Key;
                    Walk(mediaType.Key, () => Walk(mediaType.Value));
                    _visitor.CurrentKeys.Content = null;
                }
            }
        }

        /// <summary>
        /// Visits <see cref="AsyncApiMediaType"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiMediaType mediaType)
        {
            if (mediaType == null)
            {
                return;
            }

            _visitor.Visit(mediaType);

            Walk(AsyncApiConstants.Example, () => Walk(mediaType.Examples));
            Walk(AsyncApiConstants.Schema, () => Walk(mediaType.Schema));
            Walk(AsyncApiConstants.Encoding, () => Walk(mediaType.Encoding));
            Walk(mediaType as IAsyncApiExtensible);
        }

        /// <summary>
        /// Visits dictionary of <see cref="AsyncApiEncoding"/>
        /// </summary>
        internal void Walk(IDictionary<string, AsyncApiEncoding> encodings)
        {
            if (encodings == null)
            {
                return;
            }

            _visitor.Visit(encodings);

            if (encodings != null)
            {
                foreach (var item in encodings)
                {
                    _visitor.CurrentKeys.Encoding = item.Key;
                    Walk(item.Key, () => Walk(item.Value));
                    _visitor.CurrentKeys.Encoding = null;
                }
            }
        }

        /// <summary>
        /// Visits <see cref="AsyncApiEncoding"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiEncoding encoding)
        {
            if (encoding == null)
            {
                return;
            }

            _visitor.Visit(encoding);

            if (encoding.Headers != null)
            {
                Walk(encoding.Headers);
            }
            Walk(encoding as IAsyncApiExtensible);
        }

        /// <summary>
        /// Visits <see cref="AsyncApiSchema"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiSchema schema, bool isComponent = false)
        {
            if (schema == null || ProcessAsReference(schema, isComponent))
            {
                return;
            }

            if (_schemaLoop.Contains(schema))
            {
                return;  // Loop detected, this schema has already been walked.
            }
            else
            {
                _schemaLoop.Push(schema);
            }

            _visitor.Visit(schema);

            if (schema.Items != null)
            {
                Walk("items", () => Walk(schema.Items));
            }

            if (schema.AllOf != null)
            {
                Walk("allOf", () => Walk(schema.AllOf));
            }

            if (schema.AnyOf != null)
            {
                Walk("anyOf", () => Walk(schema.AnyOf));
            }

            if (schema.OneOf != null)
            {
                Walk("oneOf", () => Walk(schema.OneOf));
            }

            if (schema.Properties != null)
            {
                Walk("properties", () =>
                {
                    foreach (var item in schema.Properties)
                    {
                        Walk(item.Key, () => Walk(item.Value));
                    }
                });
            }

            if (schema.AdditionalProperties != null)
            {
                Walk("additionalProperties", () => Walk(schema.AdditionalProperties));
            }

            Walk(AsyncApiConstants.ExternalDocs, () => Walk(schema.ExternalDocs));

            Walk(schema as IAsyncApiExtensible);

            _schemaLoop.Pop();
        }

        /// <summary>
        /// Visits dictionary of <see cref="AsyncApiExample"/>
        /// </summary>
        internal void Walk(IDictionary<string, AsyncApiExample> examples)
        {
            if (examples == null)
            {
                return;
            }

            _visitor.Visit(examples);

            if (examples != null)
            {
                foreach (var example in examples)
                {
                    _visitor.CurrentKeys.Example = example.Key;
                    Walk(example.Key, () => Walk(example.Value));
                    _visitor.CurrentKeys.Example = null;
                }
            }
        }

        /// <summary>
        /// Visits <see cref="IAsyncApiAny"/> and child objects
        /// </summary>
        internal void Walk(IAsyncApiAny example)
        {
            if (example == null)
            {
                return;
            }

            _visitor.Visit(example);
        }

        /// <summary>
        /// Visits <see cref="AsyncApiExample"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiExample example, bool isComponent = false)
        {
            if (example == null || ProcessAsReference(example, isComponent))
            {
                return;
            }

            _visitor.Visit(example);
            Walk(example as IAsyncApiExtensible);
        }

        /// <summary>
        /// Visits the list of <see cref="AsyncApiExample"/> and child objects
        /// </summary>
        internal void Walk(IList<AsyncApiExample> examples)
        {
            if (examples == null)
            {
                return;
            }

            _visitor.Visit(examples);

            // Visit Examples
            if (examples != null)
            {
                for (int i = 0; i < examples.Count; i++)
                {
                    Walk(i.ToString(), () => Walk(examples[i]));
                }
            }
        }

        /// <summary>
        /// Visits a list of <see cref="AsyncApiSchema"/> and child objects
        /// </summary>
        internal void Walk(IList<AsyncApiSchema> schemas)
        {
            if (schemas == null)
            {
                return;
            }

            // Visit Schemass
            if (schemas != null)
            {
                for (int i = 0; i < schemas.Count; i++)
                {
                    Walk(i.ToString(), () => Walk(schemas[i]));
                }
            }
        }

        /// <summary>
        /// Visits <see cref="AsyncApiOAuthFlows"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiOAuthFlows flows)
        {
            if (flows == null)
            {
                return;
            }
            _visitor.Visit(flows);
            Walk(flows as IAsyncApiExtensible);
        }

        /// <summary>
        /// Visits <see cref="AsyncApiOAuthFlow"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiOAuthFlow oAuthFlow)
        {
            if (oAuthFlow == null)
            {
                return;
            }

            _visitor.Visit(oAuthFlow);
            Walk(oAuthFlow as IAsyncApiExtensible);
        }

        /// <summary>
        /// Visits dictionary of <see cref="AsyncApiLink"/> and child objects
        /// </summary>
        internal void Walk(IDictionary<string, AsyncApiLink> links)
        {
            if (links == null)
            {
                return;
            }

            _visitor.Visit(links);

            if (links != null)
            {
                foreach (var item in links)
                {
                    _visitor.CurrentKeys.Link = item.Key;
                    Walk(item.Key, () => Walk(item.Value));
                    _visitor.CurrentKeys.Link = null;
                }
            }
        }

        /// <summary>
        /// Visits <see cref="AsyncApiLink"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiLink link, bool isComponent = false)
        {
            if (link == null || ProcessAsReference(link, isComponent))
            {
                return;
            }

            _visitor.Visit(link);
            Walk(AsyncApiConstants.Server, () => Walk(link.Server));
            Walk(link as IAsyncApiExtensible);
        }

        /// <summary>
        /// Visits <see cref="AsyncApiHeader"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiHeader header, bool isComponent = false)
        {
            if (header == null || ProcessAsReference(header, isComponent))
            {
                return;
            }

            _visitor.Visit(header);
            Walk(AsyncApiConstants.Content, () => Walk(header.Content));
            Walk(AsyncApiConstants.Example, () => Walk(header.Example));
            Walk(AsyncApiConstants.Examples, () => Walk(header.Examples));
            Walk(AsyncApiConstants.Schema, () => Walk(header.Schema));
            Walk(header as IAsyncApiExtensible);
        }

        /// <summary>
        /// Visits <see cref="AsyncApiSecurityRequirement"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiSecurityRequirement securityRequirement)
        {
            if (securityRequirement == null)
            {
                return;
            }

            _visitor.Visit(securityRequirement);
            Walk(securityRequirement as IAsyncApiExtensible);
        }

        /// <summary>
        /// Visits <see cref="AsyncApiSecurityScheme"/> and child objects
        /// </summary>
        internal void Walk(AsyncApiSecurityScheme securityScheme)
        {
            if (securityScheme == null || ProcessAsReference(securityScheme))
            {
                return;
            }

            _visitor.Visit(securityScheme);
            Walk(securityScheme as IAsyncApiExtensible);
        }
       
         #endregion OldOpenApiWalkers

        /// <summary>
        /// Dispatcher method that enables using a single method to walk the model
        /// starting from any <see cref="IAsyncApiElement"/>
        /// </summary>
        internal void Walk(IAsyncApiElement element)
        {
            if (element == null)
            {
                return;
            }

            switch (element)
            {
                case AsyncApiDocument e: Walk(e); break;
                case AsyncApiContact e: Walk(e); break;
                case AsyncApiLicense e: Walk(e); break;
                case AsyncApiInfo e: Walk(e); break;
                case AsyncApiServers e: Walk(e); break;
                case AsyncApiServer e: Walk(e); break;
                case AsyncApiChannels e: Walk(e); break;
                case AsyncApiChannelItem e: Walk(e); break;
                case AsyncApiChannelBindings e: Walk(e); break;
                case AsyncApiServerVariable e: Walk(e); break;
                case AsyncApiTag e: Walk(e); break;
                case IList<AsyncApiTag> e: Walk(e); break;
                case AsyncApiExternalDocs e: Walk(e); break;
                case AsyncApiSchema e: Walk(e); break;
                case AsyncApiOperation e: Walk(e); break;
                case AsyncApiOperationBindings e: Walk(e); break;
                case AsyncApiOperationTrait e: Walk(e); break;
                case IList<AsyncApiOperationTrait> e: Walk(e); break;
                case AsyncApiMessage e: Walk(e); break;
                case IList<AsyncApiMessage> e: Walk(e); break;
                case IList<AsyncApiMessageExample> e: Walk(e); break;
                case AsyncApiCorrelationId e: Walk(e); break;
                case AsyncApiMessageExample e: Walk(e); break;
                case AsyncApiMessageTrait e: Walk(e); break;
                case AsyncApiMessageBindings e: Walk(e); break;
                case AsyncApiParameters e: Walk(e); break;
                case AsyncApiParameter e: Walk(e); break;
            
                // TODO: Marker for old OpenAPI walkers --------------------------------------------
                case AsyncApiComponents e: Walk(e); break;
                case AsyncApiCallback e: Walk(e); break;
                case AsyncApiEncoding e: Walk(e); break;
                case AsyncApiExample e: Walk(e); break;
                case IDictionary<string, AsyncApiExample> e: Walk(e); break;
                case AsyncApiHeader e: Walk(e); break;
                case AsyncApiLink e: Walk(e); break;
                case IDictionary<string, AsyncApiLink> e: Walk(e); break;
                case AsyncApiMediaType e: Walk(e); break;
                case AsyncApiOAuthFlows e: Walk(e); break;
                case AsyncApiOAuthFlow e: Walk(e); break;
                case AsyncApiRequestBody e: Walk(e); break;
                case AsyncApiResponse e: Walk(e); break;
                case AsyncApiSecurityRequirement e: Walk(e); break;
                case AsyncApiSecurityScheme e: Walk(e); break;
                //-----------------------------------
                
                case IAsyncApiExtensible e: Walk(e); break;
                case IAsyncApiExtension e: Walk(e); break;
            }
        }

        /// <summary>
        /// Adds a segment to the context path to enable pointing to the current location in the document
        /// </summary>
        /// <param name="context">An identifier for the context.</param>
        /// <param name="walk">An action that walks objects within the context.</param>
        private void Walk(string context, Action walk)
        {
            _visitor.Enter(context.Replace("/", "~1"));
            walk();
            _visitor.Exit();
        }

        /// <summary>
        /// Identify if an element is just a reference to a component, or an actual component
        /// </summary>
        private bool ProcessAsReference(IAsyncApiReferenceable referenceable, bool isComponent = false)
        {
            var isReference = referenceable.Reference != null && !isComponent;
            if (isReference)
            {
                Walk(referenceable);
            }
            return isReference;
        }
    }

    /// <summary>
    /// Object containing contextual information based on where the walker is currently referencing in an AsyncApiDocument
    /// </summary>
    public class CurrentKeys
    {
        
        // TODO : Marker for old OpenApi keys --------------------------
        /// <summary>
        /// Current Path key
        /// </summary>
        public string Path { get; set; }
        
        /// <summary>
        /// Current Channel key
        /// </summary>
        public string Channel { get; set; }
        
        /// <summary>
        /// Current ChannelBinding key
        /// </summary>
        public string ChannelBinding { get; set; }

        /// <summary>
        /// Current Operation Type
        /// </summary>
        public OperationType? Operation { get; set; }

        /// <summary>
        /// Current Response Status Code
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Current Content Media Type
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Current Callback Key
        /// </summary>
        public string Callback { get; set; }

        /// <summary>
        /// Current Link Key
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Current Header Key
        /// </summary>
        public string Header { get; internal set; }

        /// <summary>
        /// Current Encoding Key
        /// </summary>
        public string Encoding { get; internal set; }

        /// <summary>
        /// Current Example Key
        /// </summary>
        public string Example { get; internal set; }

        /// <summary>
        /// Current Extension Key
        /// </summary>
        public string Extension { get; internal set; }

        /// <summary>
        /// Current ServerVariable
        /// </summary>
        public string ServerVariable { get; internal set; }
    }
}
