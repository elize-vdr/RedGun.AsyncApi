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
    /// Header Object.
    /// The Header Object follows the structure of the Parameter Object.
    /// </summary>
    public class AsyncApiHeader : IAsyncApiSerializable, IAsyncApiReferenceable, IAsyncApiExtensible
    {
        /// <summary>
        /// Indicates if object is populated with data or is just a reference to the data
        /// </summary>
        public bool UnresolvedReference { get; set; }

        /// <summary>
        /// Reference pointer.
        /// </summary>
        public AsyncApiReference Reference { get; set; }

        /// <summary>
        /// A brief description of the header.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Determines whether this header is mandatory.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Specifies that a header is deprecated and SHOULD be transitioned out of usage.
        /// </summary>
        public bool Deprecated { get; set; }

        /// <summary>
        /// Sets the ability to pass empty-valued headers.
        /// </summary>
        public bool AllowEmptyValue { get; set; }

        /// <summary>
        /// Describes how the header value will be serialized depending on the type of the header value.
        /// </summary>
        public ParameterStyle? Style { get; set; }

        /// <summary>
        /// When this is true, header values of type array or object generate separate parameters
        /// for each value of the array or key-value pair of the map.
        /// </summary>
        public bool Explode { get; set; }

        /// <summary>
        /// Determines whether the header value SHOULD allow reserved characters, as defined by RFC3986.
        /// </summary>
        public bool AllowReserved { get; set; }

        /// <summary>
        /// The schema defining the type used for the header.
        /// </summary>
        public AsyncApiSchema Schema { get; set; }

        /// <summary>
        /// Example of the media type.
        /// </summary>
        public IAsyncApiAny Example { get; set; }

        /// <summary>
        /// Examples of the media type.
        /// </summary>
        public IDictionary<string, AsyncApiExample> Examples { get; set; } = new Dictionary<string, AsyncApiExample>();

        /// <summary>
        /// A map containing the representations for the header.
        /// </summary>
        public IDictionary<string, AsyncApiMediaType> Content { get; set; } = new Dictionary<string, AsyncApiMediaType>();

        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiHeader"/> to Async API v3.0
        /// </summary>
        public void SerializeAsV3(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            if (Reference != null && writer.GetSettings().ReferenceInline != ReferenceInlineSetting.InlineLocalReferences)
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

            // description
            writer.WriteProperty(AsyncApiConstants.Description, Description);

            // required
            writer.WriteProperty(AsyncApiConstants.Required, Required, false);

            // deprecated
            writer.WriteProperty(AsyncApiConstants.Deprecated, Deprecated, false);

            // allowEmptyValue
            writer.WriteProperty(AsyncApiConstants.AllowEmptyValue, AllowEmptyValue, false);

            // style
            writer.WriteProperty(AsyncApiConstants.Style, Style?.GetDisplayName());

            // explode
            writer.WriteProperty(AsyncApiConstants.Explode, Explode, false);

            // allowReserved
            writer.WriteProperty(AsyncApiConstants.AllowReserved, AllowReserved, false);

            // schema
            writer.WriteOptionalObject(AsyncApiConstants.Schema, Schema, (w, s) => s.SerializeAsV3(w));

            // example
            writer.WriteOptionalObject(AsyncApiConstants.Example, Example, (w, s) => w.WriteAny(s));

            // examples
            writer.WriteOptionalMap(AsyncApiConstants.Examples, Examples, (w, e) => e.SerializeAsV3(w));

            // content
            writer.WriteOptionalMap(AsyncApiConstants.Content, Content, (w, c) => c.SerializeAsV3(w));

            // extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }

        /// <summary>
        /// Serialize <see cref="AsyncApiHeader"/> to Async API v2.0
        /// </summary>
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            if (Reference != null && writer.GetSettings().ReferenceInline != ReferenceInlineSetting.InlineLocalReferences)
            {
                Reference.SerializeAsV2(writer);
                return;
            }

            SerializeAsV2WithoutReference(writer);
        }

        /// <summary>
        /// Serialize to OpenAPI V2 document without using reference.
        /// </summary>
        public void SerializeAsV2WithoutReference(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();

            // description
            writer.WriteProperty(AsyncApiConstants.Description, Description);

            // required
            writer.WriteProperty(AsyncApiConstants.Required, Required, false);

            // deprecated
            writer.WriteProperty(AsyncApiConstants.Deprecated, Deprecated, false);

            // allowEmptyValue
            writer.WriteProperty(AsyncApiConstants.AllowEmptyValue, AllowEmptyValue, false);

            // style
            writer.WriteProperty(AsyncApiConstants.Style, Style?.GetDisplayName());

            // explode
            writer.WriteProperty(AsyncApiConstants.Explode, Explode, false);

            // allowReserved
            writer.WriteProperty(AsyncApiConstants.AllowReserved, AllowReserved, false);

            // schema
            Schema?.WriteAsItemsProperties(writer);

            // example
            writer.WriteOptionalObject(AsyncApiConstants.Example, Example, (w, s) => w.WriteAny(s));

            // extensions
            // TODO: Remove
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.OpenApi2_0);

            writer.WriteEndObject();
        }
    }
}
