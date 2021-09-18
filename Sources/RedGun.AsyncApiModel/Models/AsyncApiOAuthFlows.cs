// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// OAuth Flows Object.
    /// </summary>
    public class AsyncApiOAuthFlows : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// Configuration for the OAuth Implicit flow
        /// </summary>
        public AsyncApiOAuthFlow Implicit { get; set; }

        /// <summary>
        /// Configuration for the OAuth Resource Owner Password flow.
        /// </summary>
        public AsyncApiOAuthFlow Password { get; set; }

        /// <summary>
        /// Configuration for the OAuth Client Credentials flow.
        /// </summary>
        public AsyncApiOAuthFlow ClientCredentials { get; set; }

        /// <summary>
        /// Configuration for the OAuth Authorization Code flow.
        /// </summary>
        public AsyncApiOAuthFlow AuthorizationCode { get; set; }

        /// <summary>
        /// Specification Extensions.
        /// </summary>
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiOAuthFlows"/> to Async API v3.0
        /// </summary>
        public void SerializeAsV3(IOpenApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            writer.WriteStartObject();

            // implicit
            writer.WriteOptionalObject(OpenApiConstants.Implicit, Implicit, (w, o) => o.SerializeAsV3(w));

            // password
            writer.WriteOptionalObject(OpenApiConstants.Password, Password, (w, o) => o.SerializeAsV3(w));

            // clientCredentials
            writer.WriteOptionalObject(
                OpenApiConstants.ClientCredentials,
                ClientCredentials,
                (w, o) => o.SerializeAsV3(w));

            // authorizationCode
            writer.WriteOptionalObject(
                OpenApiConstants.AuthorizationCode,
                AuthorizationCode,
                (w, o) => o.SerializeAsV3(w));

            // extensions
            writer.WriteExtensions(Extensions, OpenApiSpecVersion.OpenApi3_0);

            writer.WriteEndObject();
        }

        /// <summary>
        /// Serialize <see cref="AsyncApiOAuthFlows"/> to Async API v2.0
        /// </summary>
        public void SerializeAsV2(IOpenApiWriter writer)
        {
            // OAuthFlows object does not exist in V2.
        }
    }
}
