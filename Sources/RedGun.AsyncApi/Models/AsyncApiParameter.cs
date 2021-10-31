// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// Parameter Object.
    /// </summary>
    public class AsyncApiParameter : IAsyncApiSerializable, IAsyncApiReferenceable, IAsyncApiExtensible
    {
         /// <summary>
        /// A verbose explanation of the parameter. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A runtime expression that specifies the location of the parameter value. Even when a definition for the
        /// target field exists, it MUST NOT be used to validate this parameter but, instead, the schema property MUST be used.
        /// </summary>
        public string In { get; set; }

        /// <summary>
        /// The schema defining the type used for the parameter.
        /// </summary>
        public AsyncApiSchema Schema { get; set; }

         /// <summary>
         /// Indicates if object is populated with data or is just a reference to the data
         /// </summary>
         public bool UnresolvedReference { get; set; }

         /// <summary>
         /// Reference object.
         /// </summary>
         public AsyncApiReference Reference { get; set; }

        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiParameter"/> to Async API v2.0
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
        /// Serialize to AsyncAPI V2 document without using reference.
        /// </summary>
        public void SerializeAsV2WithoutReference(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();

            // in
            writer.WriteProperty(AsyncApiConstants.In, In);

            // description
            writer.WriteProperty(AsyncApiConstants.Description, Description);

            // schema
            writer.WriteOptionalObject(AsyncApiConstants.Schema, Schema, (w, s) => s.SerializeAsV2(w));

            // extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }
    }

    /// <summary>
    /// Body parameter class to propagate information needed for <see cref="AsyncApiParameter.SerializeAsV2"/>
    /// </summary>
    internal class AsyncApiBodyParameter : AsyncApiParameter
    {
    }

    /// <summary>
    /// Form parameter class to propagate information needed for <see cref="AsyncApiParameter.SerializeAsV2"/>
    /// </summary>
    internal class AsyncApiFormDataParameter : AsyncApiParameter
    {
    }
}
