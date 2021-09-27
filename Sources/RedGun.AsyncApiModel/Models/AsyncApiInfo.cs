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
    /// Async API Info Object, it provides the metadata about the Async API.
    /// </summary>
    public class AsyncApiInfo : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// REQUIRED. The title of the application.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A short description of the application.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// REQUIRED. The version of the AsyncAPI document.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// A URL to the Terms of Service for the API. MUST be in the format of a URL.
        /// </summary>
        public Uri TermsOfService { get; set; }

        /// <summary>
        /// The contact information for the exposed API.
        /// </summary>
        public AsyncApiContact Contact { get; set; }

        /// <summary>
        /// The license information for the exposed API.
        /// </summary>
        public AsyncApiLicense License { get; set; }

        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiInfo"/> to Async API v3.0
        /// </summary>
        public void SerializeAsV3(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            writer.WriteStartObject();

            // title
            writer.WriteProperty(AsyncApiConstants.Title, Title);

            // description
            writer.WriteProperty(AsyncApiConstants.Description, Description);

            // termsOfService
            writer.WriteProperty(AsyncApiConstants.TermsOfService, TermsOfService?.OriginalString);

            // contact object
            writer.WriteOptionalObject(AsyncApiConstants.Contact, Contact, (w, c) => c.SerializeAsV3(w));

            // license object
            writer.WriteOptionalObject(AsyncApiConstants.License, License, (w, l) => l.SerializeAsV3(w));

            // version
            writer.WriteProperty(AsyncApiConstants.Version, Version);

            // specification extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }

        /// <summary>
        /// Serialize <see cref="AsyncApiInfo"/> to Async API v2.0
        /// </summary>
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            writer.WriteStartObject();

            // title
            writer.WriteProperty(AsyncApiConstants.Title, Title);

            // description
            writer.WriteProperty(AsyncApiConstants.Description, Description);

            // termsOfService
            writer.WriteProperty(AsyncApiConstants.TermsOfService, TermsOfService?.OriginalString);

            // contact object
            writer.WriteOptionalObject(AsyncApiConstants.Contact, Contact, (w, c) => c.SerializeAsV2(w));

            // license object
            writer.WriteOptionalObject(AsyncApiConstants.License, License, (w, l) => l.SerializeAsV2(w));

            // version
            writer.WriteProperty(AsyncApiConstants.Version, Version);

            // specification extensions
            // TODO: Remove
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.OpenApi2_0);

            writer.WriteEndObject();
        }
    }
}
