﻿// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// Server Variable Object.
    /// </summary>
    public class AsyncApiServerVariable : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// An optional description for the server variable. CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// REQUIRED. The default value to use for substitution, and to send, if an alternate value is not supplied.
        /// Unlike the Schema Object's default, this value MUST be provided by the consumer.
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// An enumeration of string values to be used if the substitution options are from a limited set.
        /// </summary>
        public List<string> Enum { get; set; } = new List<string>();

        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();
        
        /// <summary>
        /// An array of examples of the server variable.
        /// </summary>
        public List<string> Examples { get; set; } = new List<string>();

        /// <summary>
        /// Serialize <see cref="AsyncApiServerVariable"/> to Async API v2.0
        /// </summary>
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            writer.WriteStartObject();

            // default
            writer.WriteProperty(AsyncApiConstants.Default, Default);

            // description
            writer.WriteProperty(AsyncApiConstants.Description, Description);

            // enums
            writer.WriteOptionalCollection(AsyncApiConstants.Enum, Enum, (w, s) => w.WriteValue(s));

            // specification extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);
            
            // examples
            writer.WriteOptionalCollection(AsyncApiConstants.Examples, Examples, (w, s) => w.WriteValue(s));

            writer.WriteEndObject();
        }
    }
}
