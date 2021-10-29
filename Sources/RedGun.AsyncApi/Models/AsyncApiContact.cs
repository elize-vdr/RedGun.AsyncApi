// Copied from Microsoft AsyncApi.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// Contact Object.
    /// </summary>
    public class AsyncApiContact : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// The identifying name of the contact person/organization.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The URL pointing to the contact information. MUST be in the format of a URL.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// The email address of the contact person/organization.
        /// MUST be in the format of an email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();
        
        /// <summary>
        /// Serialize <see cref="AsyncApiContact"/> to Async API v2.0
        /// </summary>
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            WriteInternal(writer, AsyncApiSpecVersion.AsyncApi2_0);
        }

        private void WriteInternal(IAsyncApiWriter writer, AsyncApiSpecVersion specVersion)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            writer.WriteStartObject();

            // name
            writer.WriteProperty(AsyncApiConstants.Name, Name);

            // url
            writer.WriteProperty(AsyncApiConstants.Url, Url?.OriginalString);

            // email
            writer.WriteProperty(AsyncApiConstants.Email, Email);

            // extensions
            writer.WriteExtensions(Extensions, specVersion);

            writer.WriteEndObject();
        }
    }
}
