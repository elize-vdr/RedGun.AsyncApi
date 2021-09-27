﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// Security Scheme Object.
    /// </summary>
    public class AsyncApiSecurityScheme : IAsyncApiSerializable, IAsyncApiReferenceable, IAsyncApiExtensible
    {
        /// <summary>
        /// REQUIRED. The type of the security scheme. Valid values are "apiKey", "http", "oauth2", "openIdConnect".
        /// </summary>
        public SecuritySchemeType Type { get; set; }

        /// <summary>
        /// A short description for security scheme. CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// REQUIRED. The name of the header, query or cookie parameter to be used.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// REQUIRED. The location of the API key. Valid values are "query", "header" or "cookie".
        /// </summary>
        public ParameterLocation In { get; set; }

        /// <summary>
        /// REQUIRED. The name of the HTTP Authorization scheme to be used
        /// in the Authorization header as defined in RFC7235.
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// A hint to the client to identify how the bearer token is formatted.
        /// Bearer tokens are usually generated by an authorization server,
        /// so this information is primarily for documentation purposes.
        /// </summary>
        public string BearerFormat { get; set; }

        /// <summary>
        /// REQUIRED. An object containing configuration information for the flow types supported.
        /// </summary>
        public AsyncApiOAuthFlows Flows { get; set; }

        /// <summary>
        /// REQUIRED. OpenId Connect URL to discover OAuth2 configuration values.
        /// </summary>
        public Uri OpenIdConnectUrl { get; set; }

        /// <summary>
        /// Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Indicates if object is populated with data or is just a reference to the data
        /// </summary>
        public bool UnresolvedReference { get; set; }

        /// <summary>
        /// Reference object.
        /// </summary>
        public AsyncApiReference Reference { get; set; }

        /// <summary>
        /// Serialize <see cref="AsyncApiSecurityScheme"/> to Async API v3.0
        /// </summary>
        public void SerializeAsV3(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            if (Reference != null)
            {
                Reference.SerializeAsV3(writer);
                return;
            }

            SerializeAsV3WithoutReference(writer);
        }

        /// <summary>
        /// Serialize to OpenAPI V3 document without using reference.
        /// </summary>
        public void SerializeAsV3WithoutReference(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();

            // type
            writer.WriteProperty(AsyncApiConstants.Type, Type.GetDisplayName());

            // description
            writer.WriteProperty(AsyncApiConstants.Description, Description);

            switch (Type)
            {
                case SecuritySchemeType.ApiKey:
                    // These properties apply to apiKey type only.
                    // name
                    // in
                    writer.WriteProperty(AsyncApiConstants.Name, Name);
                    writer.WriteProperty(AsyncApiConstants.In, In.GetDisplayName());
                    break;
                case SecuritySchemeType.Http:
                    // These properties apply to http type only.
                    // scheme
                    // bearerFormat
                    writer.WriteProperty(AsyncApiConstants.Scheme, Scheme);
                    writer.WriteProperty(AsyncApiConstants.BearerFormat, BearerFormat);
                    break;
                case SecuritySchemeType.OAuth2:
                    // This property apply to oauth2 type only.
                    // flows
                    writer.WriteOptionalObject(AsyncApiConstants.Flows, Flows, (w, o) => o.SerializeAsV3(w));
                    break;
                case SecuritySchemeType.OpenIdConnect:
                    // This property apply to openIdConnect only.
                    // openIdConnectUrl
                    writer.WriteProperty(AsyncApiConstants.OpenIdConnectUrl, OpenIdConnectUrl?.ToString());
                    break;
            }

            // extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }

        /// <summary>
        /// Serialize <see cref="AsyncApiSecurityScheme"/> to Async API v2.0
        /// </summary>
        // TODO: Remove
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            if (Reference != null)
            {
                Reference.SerializeAsV2(writer);
                return;
            }

            SerializeAsV2WithoutReference(writer);
        }

        /// <summary>
        /// Serialize to OpenAPI V2 document without using reference.
        /// </summary>
        // TODO: Remove
        public void SerializeAsV2WithoutReference(IAsyncApiWriter writer)
        {
            if (Type == SecuritySchemeType.Http && Scheme != AsyncApiConstants.Basic)
            {
                // Bail because V2 does not support non-basic HTTP scheme
                writer.WriteStartObject();
                writer.WriteEndObject();
                return;
            }

            if (Type == SecuritySchemeType.OpenIdConnect)
            {
                // Bail because V2 does not support OpenIdConnect
                writer.WriteStartObject();
                writer.WriteEndObject();
                return;
            }

            writer.WriteStartObject();

            // type
            switch (Type)
            {
                case SecuritySchemeType.Http:
                    writer.WriteProperty(AsyncApiConstants.Type, AsyncApiConstants.Basic);
                    break;

                case SecuritySchemeType.OAuth2:
                    // These properties apply to ouauth2 type only.
                    // flow
                    // authorizationUrl
                    // tokenUrl
                    // scopes
                    writer.WriteProperty(AsyncApiConstants.Type, Type.GetDisplayName());
                    WriteOAuthFlowForV2(writer, Flows);
                    break;

                case SecuritySchemeType.ApiKey:
                    // These properties apply to apiKey type only.
                    // name
                    // in
                    writer.WriteProperty(AsyncApiConstants.Type, Type.GetDisplayName());
                    writer.WriteProperty(AsyncApiConstants.Name, Name);
                    writer.WriteProperty(AsyncApiConstants.In, In.GetDisplayName());
                    break;
            }

            // description
            writer.WriteProperty(AsyncApiConstants.Description, Description);

            // extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.OpenApi2_0);

            writer.WriteEndObject();
        }

        /// <summary>
        /// Arbitrarily chooses one <see cref="AsyncApiOAuthFlow"/> object from the <see cref="AsyncApiOAuthFlows"/>
        /// to populate in V2 security scheme.
        /// </summary>
        // TODO: Remove
        private static void WriteOAuthFlowForV2(IAsyncApiWriter writer, AsyncApiOAuthFlows flows)
        {
            if (flows != null)
            {
                if (flows.Implicit != null)
                {
                    WriteOAuthFlowForV2(writer, AsyncApiConstants.Implicit, flows.Implicit);
                }
                else if (flows.Password != null)
                {
                    WriteOAuthFlowForV2(writer, AsyncApiConstants.Password, flows.Password);
                }
                else if (flows.ClientCredentials != null)
                {
                    WriteOAuthFlowForV2(writer, AsyncApiConstants.Application, flows.ClientCredentials);
                }
                else if (flows.AuthorizationCode != null)
                {
                    WriteOAuthFlowForV2(writer, AsyncApiConstants.AccessCode, flows.AuthorizationCode);
                }
            }
        }

        // TODO: Remove
        private static void WriteOAuthFlowForV2(IAsyncApiWriter writer, string flowValue, AsyncApiOAuthFlow flow)
        {
            // flow
            writer.WriteProperty(AsyncApiConstants.Flow, flowValue);

            // authorizationUrl
            writer.WriteProperty(AsyncApiConstants.AuthorizationUrl, flow.AuthorizationUrl?.ToString());

            // tokenUrl
            writer.WriteProperty(AsyncApiConstants.TokenUrl, flow.TokenUrl?.ToString());

            // scopes
            writer.WriteOptionalMap(AsyncApiConstants.Scopes, flow.Scopes, (w, s) => w.WriteValue(s));
        }
    }
}
