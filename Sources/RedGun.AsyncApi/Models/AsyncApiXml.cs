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
    /// XML Object.
    /// </summary>
    public class AsyncApiXml : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// Replaces the name of the element/attribute used for the described schema property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The URI of the namespace definition. Value MUST be in the form of an absolute URI.
        /// </summary>
        public Uri Namespace { get; set; }

        /// <summary>
        /// The prefix to be used for the name
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Declares whether the property definition translates to an attribute instead of an element.
        /// Default value is false.
        /// </summary>
        public bool Attribute { get; set; }

        /// <summary>
        /// Signifies whether the array is wrapped.
        /// Default value is false.
        /// </summary>
        public bool Wrapped { get; set; }

        /// <summary>
        /// Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiXml"/> to Async API v2.0
        /// </summary>
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            Write(writer, AsyncApiSpecVersion.AsyncApi2_0);
        }

        private void Write(IAsyncApiWriter writer, AsyncApiSpecVersion specVersion)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            writer.WriteStartObject();

            // name
            writer.WriteProperty(AsyncApiConstants.Name, Name);

            // namespace
            writer.WriteProperty(AsyncApiConstants.Namespace, Namespace?.AbsoluteUri);

            // prefix
            writer.WriteProperty(AsyncApiConstants.Prefix, Prefix);

            // attribute
            writer.WriteProperty(AsyncApiConstants.Attribute, Attribute, false);

            // wrapped
            writer.WriteProperty(AsyncApiConstants.Wrapped, Wrapped, false);

            // extensions
            writer.WriteExtensions(Extensions, specVersion);

            writer.WriteEndObject();
        }
    }
}
