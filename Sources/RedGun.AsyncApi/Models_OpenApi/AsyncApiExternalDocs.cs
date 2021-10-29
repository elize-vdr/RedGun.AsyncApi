// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// ExternalDocs object.
    /// </summary>
    public class AsyncApiExternalDocs : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// A short description of the target documentation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// REQUIRED. The URL for the target documentation. Value MUST be in the format of a URL.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiExternalDocs"/> to Async API v2.0.
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

            // description
            writer.WriteProperty(AsyncApiConstants.Description, Description);

            // url
            writer.WriteProperty(AsyncApiConstants.Url, Url?.OriginalString);

            // extensions
            writer.WriteExtensions(Extensions, specVersion);

            writer.WriteEndObject();
        }
    }
}
