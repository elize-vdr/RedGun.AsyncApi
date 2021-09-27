// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// Example Object.
    /// </summary>
    public class AsyncApiExample : IAsyncApiSerializable, IAsyncApiReferenceable, IAsyncApiExtensible
    {
        /// <summary>
        /// Short description for the example.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Long description for the example.
        /// CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Embedded literal example. The value field and externalValue field are mutually
        /// exclusive. To represent examples of media types that cannot naturally represented
        /// in JSON or YAML, use a string value to contain the example, escaping where necessary.
        /// </summary>
        public IAsyncApiAny Value { get; set; }

        /// <summary>
        /// A URL that points to the literal example.
        /// This provides the capability to reference examples that cannot easily be
        /// included in JSON or YAML documents.
        /// The value field and externalValue field are mutually exclusive.
        /// </summary>
        public string ExternalValue { get; set; }

        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Reference object.
        /// </summary>
        public AsyncApiReference Reference { get; set; }

        /// <summary>
        /// Indicates object is a placeholder reference to an actual object and does not contain valid data.
        /// </summary>
        public bool UnresolvedReference { get; set; } = false;

        /// <summary>
        /// Serialize <see cref="AsyncApiExample"/> to Async API v3.0
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

            // summary
            writer.WriteProperty(AsyncApiConstants.Summary, Summary);

            // description
            writer.WriteProperty(AsyncApiConstants.Description, Description);

            // value
            writer.WriteOptionalObject(AsyncApiConstants.Value, Value, (w, v) => w.WriteAny(v));

            // externalValue
            writer.WriteProperty(AsyncApiConstants.ExternalValue, ExternalValue);

            // extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }

        /// <summary>
        /// Serialize <see cref="AsyncApiExample"/> to Async API v2.0
        /// </summary>
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            // Example object of this form does not exist in V2.
            // V2 Example object requires knowledge of media type and exists only
            // in Response object, so it will be serialized as a part of the Response object.
        }

        /// <summary>
        /// Serialize to OpenAPI V2 document without using reference.
        /// </summary>
        public void SerializeAsV2WithoutReference(IAsyncApiWriter writer)
        {
            // Example object of this form does not exist in V2.
            // V2 Example object requires knowledge of media type and exists only
            // in Response object, so it will be serialized as a part of the Response object.
        }
    }
}
