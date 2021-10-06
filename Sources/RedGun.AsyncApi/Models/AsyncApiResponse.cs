// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using System.Linq;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// Response object.
    /// </summary>
    public class AsyncApiResponse : IAsyncApiSerializable, IAsyncApiReferenceable, IAsyncApiExtensible
    {
        /// <summary>
        /// REQUIRED. A short description of the response.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Maps a header name to its definition.
        /// </summary>
        public IDictionary<string, AsyncApiHeader> Headers { get; set; } = new Dictionary<string, AsyncApiHeader>();

        /// <summary>
        /// A map containing descriptions of potential response payloads.
        /// The key is a media type or media type range and the value describes it.
        /// </summary>
        public IDictionary<string, AsyncApiMediaType> Content { get; set; } = new Dictionary<string, AsyncApiMediaType>();

        /// <summary>
        /// A map of operations links that can be followed from the response.
        /// The key of the map is a short name for the link,
        /// following the naming constraints of the names for Component Objects.
        /// </summary>
        public IDictionary<string, AsyncApiLink> Links { get; set; } = new Dictionary<string, AsyncApiLink>();

        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Indicates if object is populated with data or is just a reference to the data
        /// </summary>
        public bool UnresolvedReference { get; set; }

        /// <summary>
        /// Reference pointer.
        /// </summary>
        public AsyncApiReference Reference { get; set; }

        /// <summary>
        /// Serialize <see cref="AsyncApiResponse"/> to Async API v3.0.
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
            writer.WriteRequiredProperty(AsyncApiConstants.Description, Description);

            // headers
            writer.WriteOptionalMap(AsyncApiConstants.Headers, Headers, (w, h) => h.SerializeAsV3(w));

            // content
            writer.WriteOptionalMap(AsyncApiConstants.Content, Content, (w, c) => c.SerializeAsV3(w));

            // links
            writer.WriteOptionalMap(AsyncApiConstants.Links, Links, (w, l) => l.SerializeAsV3(w));

            // extension
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }

        /// <summary>
        /// Serialize <see cref="AsyncApiResponse"/> to Async API v2.0.
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
        // TODO: Remove
        public void SerializeAsV2WithoutReference(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();

            // description
            writer.WriteRequiredProperty(AsyncApiConstants.Description, Description);

            var extensionsClone = new Dictionary<string, IAsyncApiExtension>(Extensions);

            if (Content != null)
            {
                var mediatype = Content.FirstOrDefault();
                if (mediatype.Value != null)
                {
                    // schema
                    writer.WriteOptionalObject(
                        AsyncApiConstants.Schema,
                        mediatype.Value.Schema,
                        (w, s) => s.SerializeAsV2(w));

                    // examples
                    if (Content.Values.Any(m => m.Example != null))
                    {
                        writer.WritePropertyName(AsyncApiConstants.Examples);
                        writer.WriteStartObject();

                        foreach (var mediaTypePair in Content)
                        {
                            if (mediaTypePair.Value.Example != null)
                            {
                                writer.WritePropertyName(mediaTypePair.Key);
                                writer.WriteAny(mediaTypePair.Value.Example);
                            }
                        }

                        writer.WriteEndObject();
                    }

                    // TODO: Remove
                    writer.WriteExtensions(mediatype.Value.Extensions, AsyncApiSpecVersion.OpenApi2_0);

                    foreach (var key in mediatype.Value.Extensions.Keys)
                    {
                        // The extension will already have been serialized as part of the call above,
                        // so remove it from the cloned collection so we don't write it again.
                        extensionsClone.Remove(key);
                    }
                }
            }

            // headers
            writer.WriteOptionalMap(AsyncApiConstants.Headers, Headers, (w, h) => h.SerializeAsV2(w));

            // extension
            // TODO: Remove
            writer.WriteExtensions(extensionsClone, AsyncApiSpecVersion.OpenApi2_0);

            writer.WriteEndObject();
        }
    }
}
