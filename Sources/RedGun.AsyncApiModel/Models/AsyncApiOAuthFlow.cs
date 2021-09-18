// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// OAuth Flow Object.
    /// </summary>
    public class AsyncApiOAuthFlow : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// REQUIRED. The authorization URL to be used for this flow.
        /// Applies to implicit and authorizationCode OAuthFlow.
        /// </summary>
        public Uri AuthorizationUrl { get; set; }

        /// <summary>
        /// REQUIRED. The token URL to be used for this flow.
        /// Applies to password, clientCredentials, and authorizationCode OAuthFlow.
        /// </summary>
        public Uri TokenUrl { get; set; }

        /// <summary>
        /// The URL to be used for obtaining refresh tokens.
        /// </summary>
        public Uri RefreshUrl { get; set; }

        /// <summary>
        /// REQUIRED. A map between the scope name and a short description for it.
        /// </summary>
        public IDictionary<string, string> Scopes { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Specification Extensions.
        /// </summary>
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiOAuthFlow"/> to Async API v3.0
        /// </summary>
        public void SerializeAsV3(IOpenApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            writer.WriteStartObject();

            // authorizationUrl
            writer.WriteProperty(OpenApiConstants.AuthorizationUrl, AuthorizationUrl?.ToString());

            // tokenUrl
            writer.WriteProperty(OpenApiConstants.TokenUrl, TokenUrl?.ToString());

            // refreshUrl
            writer.WriteProperty(OpenApiConstants.RefreshUrl, RefreshUrl?.ToString());

            // scopes
            writer.WriteRequiredMap(OpenApiConstants.Scopes, Scopes, (w, s) => w.WriteValue(s));

            // extensions
            writer.WriteExtensions(Extensions, OpenApiSpecVersion.OpenApi3_0);

            writer.WriteEndObject();
        }

        /// <summary>
        /// Serialize <see cref="AsyncApiOAuthFlow"/> to Async API v2.0
        /// </summary>
        public void SerializeAsV2(IOpenApiWriter writer)
        {
            // OAuthFlow object does not exist in V2.
        }
    }
}
