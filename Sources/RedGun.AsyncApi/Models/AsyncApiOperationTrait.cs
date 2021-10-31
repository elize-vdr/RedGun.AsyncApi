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
    /// OperationTrait Object.
    /// </summary>
    public class AsyncApiOperationTrait : IAsyncApiSerializable, IAsyncApiReferenceable, IAsyncApiExtensible
    {
        /// <summary>
        /// Unique string used to identify the operation. The id MUST be unique among all operations described in the API.
        /// The operationId value is case-sensitive. Tools and libraries MAY use the operationId to uniquely identify an operation,
        /// therefore, it is RECOMMENDED to follow common programming naming conventions.
        /// </summary>
        public string OperationId { get; set; }
        
        /// <summary>
        /// A short summary of what the operation is about.
        /// </summary>
        public string Summary { get; set; }
        
        /// <summary>
        /// A verbose explanation of the operation. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// A list of tags for API documentation control.
        /// Tags can be used for logical grouping of operations.
        /// </summary>
        public IList<AsyncApiTag> Tags { get; set; } = new List<AsyncApiTag>();

        /// <summary>
        /// Additional external documentation for this operation.
        /// </summary>
        public AsyncApiExternalDocs ExternalDocs { get; set; }
        
        /// <summary>
        /// A map where the keys describe the name of the protocol and the values describe
        /// protocol-specific definitions for the operation.
        /// </summary>
        public AsyncApiOperationBindings Bindings { get; set; }       

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
        /// Serialize <see cref="AsyncApiOperationTrait"/> to Async API v2.0
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

            // operationId
            writer.WriteProperty(AsyncApiConstants.OperationId, OperationId);
            
            // summary
            writer.WriteProperty(AsyncApiConstants.Summary, Summary);

            // description
            writer.WriteProperty(AsyncApiConstants.Description, Description);

            // tags
            writer.WriteOptionalCollection(AsyncApiConstants.Tags, Tags, (w, t) => t.SerializeAsV2(w));

            // externalDocs
            writer.WriteOptionalObject(AsyncApiConstants.ExternalDocs, ExternalDocs, (w, e) => e.SerializeAsV2(w));

            // bindings
            writer.WriteOptionalObject(AsyncApiConstants.Bindings, Bindings, (w, l) => l.SerializeAsV2(w));

            // extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }
    }

    /// <summary>
    /// Body operationTrait class to propagate information needed for <see cref="AsyncApiOperationTrait.SerializeAsV2"/>
    /// </summary>
    internal class AsyncApiBodyOperationTrait : AsyncApiOperationTrait
    {
    }

    /// <summary>
    /// Form operationTrait class to propagate information needed for <see cref="AsyncApiOperationTrait.SerializeAsV2"/>
    /// </summary>
    internal class AsyncApiFormDataOperationTrait : AsyncApiOperationTrait
    {
    }
}
