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

namespace RedGun.AsyncApi.Readers.V3
{
    /// <summary>
    /// The version service for the Async API V3.0.
    /// </summary>
    internal class AsyncApiV3VersionService : IAsyncApiVersionService
    {
        private IDictionary<Type, Func<ParseNode, object>> _loaders = new Dictionary<Type, Func<ParseNode, object>>
        {
            [typeof(IAsyncApiAny)] = AsyncApiV3Deserializer.LoadAny,
            [typeof(AsyncApiCallback)] = AsyncApiV3Deserializer.LoadCallback,
            [typeof(AsyncApiComponents)] = AsyncApiV3Deserializer.LoadComponents,
            [typeof(AsyncApiEncoding)] = AsyncApiV3Deserializer.LoadEncoding,
            [typeof(AsyncApiExample)] = AsyncApiV3Deserializer.LoadExample,
            [typeof(AsyncApiExternalDocs)] = AsyncApiV3Deserializer.LoadExternalDocs,
            [typeof(AsyncApiHeader)] = AsyncApiV3Deserializer.LoadHeader,
            [typeof(AsyncApiInfo)] = AsyncApiV3Deserializer.LoadInfo,
            [typeof(AsyncApiLicense)] = AsyncApiV3Deserializer.LoadLicense,
            [typeof(AsyncApiLink)] = AsyncApiV3Deserializer.LoadLink,
            [typeof(AsyncApiMediaType)] = AsyncApiV3Deserializer.LoadMediaType,
            [typeof(AsyncApiOAuthFlow)] = AsyncApiV3Deserializer.LoadOAuthFlow,
            [typeof(AsyncApiOAuthFlows)] = AsyncApiV3Deserializer.LoadOAuthFlows,
            [typeof(AsyncApiOperation)] = AsyncApiV3Deserializer.LoadOperation,
            [typeof(AsyncApiParameter)] = AsyncApiV3Deserializer.LoadParameter,
            [typeof(AsyncApiPathItem)] = AsyncApiV3Deserializer.LoadPathItem,
            [typeof(AsyncApiPaths)] = AsyncApiV3Deserializer.LoadPaths,
            [typeof(AsyncApiRequestBody)] = AsyncApiV3Deserializer.LoadRequestBody,
            [typeof(AsyncApiResponse)] = AsyncApiV3Deserializer.LoadResponse,
            [typeof(AsyncApiResponses)] = AsyncApiV3Deserializer.LoadResponses,
            [typeof(AsyncApiSchema)] = AsyncApiV3Deserializer.LoadSchema,
            [typeof(AsyncApiSecurityRequirement)] = AsyncApiV3Deserializer.LoadSecurityRequirement,
            [typeof(AsyncApiSecurityScheme)] = AsyncApiV3Deserializer.LoadSecurityScheme,
            [typeof(AsyncApiServer)] = AsyncApiV3Deserializer.LoadServer,
            [typeof(AsyncApiServerVariable)] = AsyncApiV3Deserializer.LoadServerVariable,
            [typeof(AsyncApiTag)] = AsyncApiV3Deserializer.LoadTag,
            [typeof(AsyncApiXml)] = AsyncApiV3Deserializer.LoadXml
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
            return AsyncApiV3Deserializer.LoadAsyncApi(rootNode);
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
