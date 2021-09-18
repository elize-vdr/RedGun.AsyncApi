// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// Media Type Object.
    /// </summary>
    public class AsyncApiMediaType : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// The schema defining the type used for the request body.
        /// </summary>
        public AsyncApiSchema Schema { get; set; }

        /// <summary>
        /// Example of the media type.
        /// The example object SHOULD be in the correct format as specified by the media type.
        /// </summary>
        public IAsyncApiAny Example { get; set; }

        /// <summary>
        /// Examples of the media type.
        /// Each example object SHOULD match the media type and specified schema if present.
        /// </summary>
        public IDictionary<string, AsyncApiExample> Examples { get; set; } = new Dictionary<string, AsyncApiExample>();

        /// <summary>
        /// A map between a property name and its encoding information.
        /// The key, being the property name, MUST exist in the schema as a property.
        /// The encoding object SHALL only apply to requestBody objects
        /// when the media type is multipart or application/x-www-form-urlencoded.
        /// </summary>
        public IDictionary<string, AsyncApiEncoding> Encoding { get; set; } = new Dictionary<string, AsyncApiEncoding>();

        /// <summary>
        /// Serialize <see cref="AsyncApiExternalDocs"/> to Async API v3.0.
        /// </summary>
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiMediaType"/> to Async API v3.0.
        /// </summary>
        public void SerializeAsV3(IOpenApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            writer.WriteStartObject();

            // schema
            writer.WriteOptionalObject(OpenApiConstants.Schema, Schema, (w, s) => s.SerializeAsV3(w));

            // example
            writer.WriteOptionalObject(OpenApiConstants.Example, Example, (w, e) => w.WriteAny(e));

            // examples
            writer.WriteOptionalMap(OpenApiConstants.Examples, Examples, (w, e) => e.SerializeAsV3(w));

            // encoding
            writer.WriteOptionalMap(OpenApiConstants.Encoding, Encoding, (w, e) => e.SerializeAsV3(w));

            // extensions
            writer.WriteExtensions(Extensions, OpenApiSpecVersion.OpenApi3_0);

            writer.WriteEndObject();
        }

        /// <summary>
        /// Serialize <see cref="AsyncApiMediaType"/> to Async API v2.0.
        /// </summary>
        public void SerializeAsV2(IOpenApiWriter writer)
        {
            // Media type does not exist in V2.
        }
    }
}
