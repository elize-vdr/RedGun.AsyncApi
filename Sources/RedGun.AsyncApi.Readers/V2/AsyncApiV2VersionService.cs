// Copyright (c) Microsoft Corporation. All rights reserved.
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
    /// The version service for the Async API V3.0.
    /// </summary>
    internal class AsyncApiV2VersionService : IAsyncApiVersionService
    {
        private IDictionary<Type, Func<ParseNode, object>> _loaders = new Dictionary<Type, Func<ParseNode, object>>
        {
            [typeof(IAsyncApiAny)] = AsyncApiV2Deserializer.LoadAny,
            [typeof(AsyncApiCallback)] = AsyncApiV2Deserializer.LoadCallback,
            [typeof(AsyncApiComponents)] = AsyncApiV2Deserializer.LoadComponents,
            [typeof(AsyncApiEncoding)] = AsyncApiV2Deserializer.LoadEncoding,
            [typeof(AsyncApiExample)] = AsyncApiV2Deserializer.LoadExample,
            [typeof(AsyncApiExternalDocs)] = AsyncApiV2Deserializer.LoadExternalDocs,
            [typeof(AsyncApiHeader)] = AsyncApiV2Deserializer.LoadHeader,
            [typeof(AsyncApiInfo)] = AsyncApiV2Deserializer.LoadInfo,
            [typeof(AsyncApiLicense)] = AsyncApiV2Deserializer.LoadLicense,
            [typeof(AsyncApiLink)] = AsyncApiV2Deserializer.LoadLink,
            [typeof(AsyncApiMediaType)] = AsyncApiV2Deserializer.LoadMediaType,
            [typeof(AsyncApiOAuthFlow)] = AsyncApiV2Deserializer.LoadOAuthFlow,
            [typeof(AsyncApiOAuthFlows)] = AsyncApiV2Deserializer.LoadOAuthFlows,
            [typeof(AsyncApiOperation)] = AsyncApiV2Deserializer.LoadOperation,
            [typeof(AsyncApiParameter)] = AsyncApiV2Deserializer.LoadParameter,
            [typeof(AsyncApiPathItem)] = AsyncApiV2Deserializer.LoadPathItem,
            [typeof(AsyncApiPaths)] = AsyncApiV2Deserializer.LoadPaths,
            [typeof(AsyncApiRequestBody)] = AsyncApiV2Deserializer.LoadRequestBody,
            [typeof(AsyncApiResponse)] = AsyncApiV2Deserializer.LoadResponse,
            [typeof(AsyncApiResponses)] = AsyncApiV2Deserializer.LoadResponses,
            [typeof(AsyncApiSchema)] = AsyncApiV2Deserializer.LoadSchema,
            [typeof(AsyncApiSecurityRequirement)] = AsyncApiV2Deserializer.LoadSecurityRequirement,
            [typeof(AsyncApiSecurityScheme)] = AsyncApiV2Deserializer.LoadSecurityScheme,
            [typeof(AsyncApiServer)] = AsyncApiV2Deserializer.LoadServer,
            [typeof(AsyncApiServerVariable)] = AsyncApiV2Deserializer.LoadServerVariable,
            [typeof(AsyncApiTag)] = AsyncApiV2Deserializer.LoadTag,
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
                var segments = reference.Split('#');
                if (segments.Length == 1)
                {
                    // Either this is an external reference as an entire file
                    // or a simple string-style reference for tag and security scheme.
                    if (type == null)
                    {
                        // "$ref": "Pet.json"
                        return new AsyncApiReference
                        {
                            Type = type,
                            ExternalResource = segments[0]
                        };
                    }

                    if (type == ReferenceType.Tag || type == ReferenceType.SecurityScheme)
                    {
                        return new AsyncApiReference
                        {
                            Type = type,
                            Id = reference
                        };
                    }
                }
                else if (segments.Length == 2)
                {
                    if (reference.StartsWith("#"))
                    {
                        // "$ref": "#/components/schemas/Pet"
                        return ParseLocalReference(segments[1]);
                    }
                    // Where fragments point into a non-AsyncAPI document, the id will be the complete fragment identifier
                    string id = segments[1];
                    // $ref: externalSource.yaml#/Pet
                    if (id.StartsWith("/components/"))
                    {
                        id = segments[1].Split('/')[3];
                    } 
                    return new AsyncApiReference
                    {
                        ExternalResource = segments[0],
                        Type = type,
                        Id = id
                    };
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
