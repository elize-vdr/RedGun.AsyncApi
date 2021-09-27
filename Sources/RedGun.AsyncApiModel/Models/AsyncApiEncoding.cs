// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// ExternalDocs object.
    /// </summary>
    public class AsyncApiEncoding : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// The Content-Type for encoding a specific property.
        /// The value can be a specific media type (e.g. application/json),
        /// a wildcard media type (e.g. image/*), or a comma-separated list of the two types.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// A map allowing additional information to be provided as headers.
        /// </summary>
        public IDictionary<string, AsyncApiHeader> Headers { get; set; } = new Dictionary<string, AsyncApiHeader>();

        /// <summary>
        /// Describes how a specific property value will be serialized depending on its type.
        /// </summary>
        public ParameterStyle? Style { get; set; }

        /// <summary>
        /// When this is true, property values of type array or object generate separate parameters
        /// for each value of the array, or key-value-pair of the map. For other types of properties
        /// this property has no effect. When style is form, the default value is true.
        /// For all other styles, the default value is false.
        /// This property SHALL be ignored if the request body media type is not application/x-www-form-urlencoded.
        /// </summary>
        public bool? Explode { get; set; }

        /// <summary>
        /// Determines whether the parameter value SHOULD allow reserved characters,
        /// as defined by RFC3986 :/?#[]@!$&amp;'()*+,;= to be included without percent-encoding.
        /// The default value is false. This property SHALL be ignored
        /// if the request body media type is not application/x-www-form-urlencoded.
        /// </summary>
        public bool? AllowReserved { get; set; }

        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiExternalDocs"/> to Async API v3.0.
        /// </summary>
        public void SerializeAsV3(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull("writer");
            }

            writer.WriteStartObject();

            // contentType
            writer.WriteProperty(AsyncApiConstants.ContentType, ContentType);

            // headers
            writer.WriteOptionalMap(AsyncApiConstants.Headers, Headers, (w, h) => h.SerializeAsV3(w));

            // style
            writer.WriteProperty(AsyncApiConstants.Style, Style?.GetDisplayName());

            // explode
            writer.WriteProperty(AsyncApiConstants.Explode, Explode, false);

            // allowReserved
            writer.WriteProperty(AsyncApiConstants.AllowReserved, AllowReserved, false);

            // extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }

        /// <summary>
        /// Serialize <see cref="AsyncApiExternalDocs"/> to Async API v2.0.
        /// </summary>
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            // nothing here
        }
    }
}
