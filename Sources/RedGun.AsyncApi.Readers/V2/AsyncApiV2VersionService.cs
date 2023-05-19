// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK.
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Exceptions;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.Interface;
using RedGun.AsyncApi.Readers.ParseNodes;
using RedGun.AsyncApi.Readers.Properties;

namespace RedGun.AsyncApi.Readers.V2
{
    /// <summary>
    /// The version service for the Open API V2.0.
    /// </summary>
    internal class AsyncApiV2VersionService : IAsyncApiVersionService
    {
        private IDictionary<Type, Func<ParseNode, object>> _loaders = new Dictionary<Type, Func<ParseNode, object>>
        {
            [typeof(IAsyncApiAny)] = AsyncApiV2Deserializer.LoadAny,
            [typeof(AsyncApiInfo)] = AsyncApiV2Deserializer.LoadInfo,
            [typeof(AsyncApiLicense)] = AsyncApiV2Deserializer.LoadLicense,
            [typeof(AsyncApiServer)] = AsyncApiV2Deserializer.LoadServer,
            [typeof(AsyncApiServer)] = AsyncApiV2Deserializer.LoadServers,
            [typeof(AsyncApiServerVariable)] = AsyncApiV2Deserializer.LoadServerVariable,
            [typeof(AsyncApiTag)] = AsyncApiV2Deserializer.LoadTag,
            [typeof(AsyncApiExternalDocs)] = AsyncApiV2Deserializer.LoadExternalDocs,
            [typeof(AsyncApiSchema)] = AsyncApiV2Deserializer.LoadSchema,
            [typeof(AsyncApiChannels)] = AsyncApiV2Deserializer.LoadChannels,
            [typeof(AsyncApiChannelItem)] = AsyncApiV2Deserializer.LoadChannelItem,
            [typeof(AsyncApiChannelBindings)] = AsyncApiV2Deserializer.LoadChannelBindings,
            [typeof(AsyncApiBindingHttpOperation)] = AsyncApiV2Deserializer.LoadBindingWebSocketsChannel,
            [typeof(AsyncApiBindingHttpOperation)] = AsyncApiV2Deserializer.LoadBindingHttpOperation,
            [typeof(AsyncApiBindingHttpOperation)] = AsyncApiV2Deserializer.LoadBindingKafkaMessage,
            [typeof(AsyncApiBindingHttpOperation)] = AsyncApiV2Deserializer.LoadBindingKafkaOperation,
            [typeof(AsyncApiChannels)] = AsyncApiV2Deserializer.LoadOperation,
            [typeof(AsyncApiChannels)] = AsyncApiV2Deserializer.LoadOperationBindings,
            [typeof(AsyncApiChannels)] = AsyncApiV2Deserializer.LoadOperationTrait,
            [typeof(AsyncApiChannels)] = AsyncApiV2Deserializer.LoadMessage,
            [typeof(AsyncApiChannels)] = AsyncApiV2Deserializer.LoadMessageExample,
            [typeof(AsyncApiChannels)] = AsyncApiV2Deserializer.LoadMessageBindings,
            [typeof(AsyncApiChannels)] = AsyncApiV2Deserializer.LoadMessageTrait,
            [typeof(AsyncApiChannels)] = AsyncApiV2Deserializer.LoadCorrelationId,
            [typeof(AsyncApiComponents)] = AsyncApiV2Deserializer.LoadComponents,
            [typeof(AsyncApiOAuthFlow)] = AsyncApiV2Deserializer.LoadOAuthFlow,
            [typeof(AsyncApiOAuthFlows)] = AsyncApiV2Deserializer.LoadOAuthFlows,
            [typeof(AsyncApiParameter)] = AsyncApiV2Deserializer.LoadParameter,
            [typeof(AsyncApiSecurityRequirement)] = AsyncApiV2Deserializer.LoadSecurityRequirement,
            [typeof(AsyncApiSecurityScheme)] = AsyncApiV2Deserializer.LoadSecurityScheme,
            [typeof(AsyncApiXml)] = AsyncApiV2Deserializer.LoadXml
        };

        /// <summary>
        /// Parse the string to a <see cref="AsyncApiReference"/> object.
        /// </summary>
        public AsyncApiReference ConvertToAsyncApiReference(
            string reference,
            ReferenceType? type)
        {
            if (!string.IsNullOrWhiteSpace(reference))
            {
                // According to this post: https://swagger.io/docs/specification/using-ref/ the 'JSON Reference' may be:
                // 1. A reference to a local object starting from the root of the current document and this starts with '#'; e.g. '#/components/schemas/User'
                // 2. A reference in an external document from the root of that document thisconsists of the url followed by the reference starting with '#'; e.g. 
                //    'http://myexample.com/commonrefs.yaml#/components.schemas/category'
                // 3. A reference to an external document without referencing a specific component this means the entire document is the reference; e.g.
                //    'http://myexample.com/commonrefs.yaml'
                var segments = reference.Split('#');
                switch (segments.Length)
                {
                    // Either this is an external reference as an entire file
                    // or a simple string-style reference for tag and security scheme.
                    case 1 when type is ReferenceType.Tag or ReferenceType.SecurityScheme:
                        return new AsyncApiReference {
                            Type = type,
                            Id = reference
                            };
                    case 1:
                        // "$ref": "Pet.json"
                        return new AsyncApiReference {
                            Type = type,
                            ExternalResource = segments[0]
                            };
                    case 2 when reference.StartsWith("#"):
                        // "$ref": "#/components/schemas/Pet"
                        return ParseLocalReference(segments[1]);
                    // Where fragments point into a non-OpenAPI document, the id will be the complete fragment identifier
                    case 2:
                    {
                        string id = segments[1];
                        // $ref: externalSource.yaml#/Pet
                        if (id.StartsWith("/components/"))
                        {
                            id = segments[1].Split('/')[3];
                        } 
                        return new AsyncApiReference {
                            ExternalResource = segments[0],
                            Type = type,
                            Id = id
                            };
                    }
                }
            }

            throw new AsyncApiException(string.Format(SRResource.ReferenceHasInvalidFormat, reference));
        }

        public AsyncApiDocument LoadDocument(RootNode rootNode)
        {
            return AsyncApiV2Deserializer.LoadAsyncApi(rootNode);
        }

        public T LoadElement<T>(ParseNode node) where T : IAsyncApiElement
        {
            return (T)_loaders[typeof(T)](node);
        }

        private AsyncApiReference ParseLocalReference(string localReference)
        {
            if (string.IsNullOrWhiteSpace(localReference))
            {
                throw new ArgumentException(string.Format(SRResource.ArgumentNullOrWhiteSpace, nameof(localReference)));
            }

            var segments = localReference.Split('/');

            if (segments.Length == 4) // /components/{type}/pet
            {
                if (segments[1] == "components")
                {
                    var referenceType = segments[2].GetEnumFromDisplayName<ReferenceType>();
                    return new AsyncApiReference { Type = referenceType, Id = segments[3] };
                }
            }

            throw new AsyncApiException(string.Format(SRResource.ReferenceHasInvalidFormat, localReference));
        }
    }
}
