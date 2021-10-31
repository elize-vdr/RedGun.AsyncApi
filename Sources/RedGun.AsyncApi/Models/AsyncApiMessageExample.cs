// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// MessageExampled under the MIT messageExample. 

using System;
using System.Collections.Generic;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// MessageExample Object.
    /// </summary>
    public class AsyncApiMessageExample : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// The value of this field MUST validate against the Message Object's headers field.
        /// </summary>
        // TODO: should we add validation for this???
        public IDictionary<string, IAsyncApiAny> Headers { get; set; } = new Dictionary<string, IAsyncApiAny>();

        /// <summary>
        /// The value of this field MUST validate against the Message Object's payload field.
        /// </summary>
        // TODO: should we add validation for this???
        public IAsyncApiAny Payload { get; set; }

        /// <summary>
        /// A machine-friendly name.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// A short summary of what the example is about.
        /// </summary>
        public string Summary { get; set; }
        
        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiMessageExample"/> to Async API v2.0
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
            
            // headers
            writer.WriteOptionalMap(
                AsyncApiConstants.Headers,
                Headers,
                (w, key, header) =>
                {
                    w.WriteAny(header);
                });
            
            // payload
            writer.WriteOptionalObject(AsyncApiConstants.Payload, Payload, (w, d) => w.WriteAny(d));

            // name
            writer.WriteProperty(AsyncApiConstants.Name, Name);

            // url
            writer.WriteProperty(AsyncApiConstants.Summary, Summary);

            // specification extensions
            writer.WriteExtensions(Extensions, specVersion);

            writer.WriteEndObject();
        }
    }
}
