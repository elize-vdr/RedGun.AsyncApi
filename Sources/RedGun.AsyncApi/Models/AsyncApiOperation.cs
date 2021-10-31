// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Linq;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// Operation Object.
    /// </summary>
    public class AsyncApiOperation : IAsyncApiSerializable, IAsyncApiExtensible
    {
        
        /// <summary>
        /// Unique string used to identify the operation. The id MUST be unique among all operations described in the API.
        /// The operationId value is case-sensitive. Tools and libraries MAY use the operationId to uniquely identify an
        /// operation, therefore, it is RECOMMENDED to follow common programming naming conventions.
        /// </summary>
        public string OperationId { get; set; }
        
        /// <summary>
        /// A short summary of what the operation is about.
        /// </summary>
        public string Summary { get; set; }
        
        /// <summary>
        /// A verbose explanation of the operation behavior.
        /// CommonMark syntax MAY be used for rich text representation.
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
        /// A map where the keys describe the name of the protocol and the values describe protocol-specific
        /// definitions for the operation.
        /// </summary>
        public AsyncApiOperationBindings Bindings { get; set; }   
        
        /// <summary>
        /// A list of traits to apply to the operation object. T
        /// Traits MUST be merged into the operation object using the JSON Merge Patch algorithm in the same order they are defined here.
        /// </summary>
        public IList<AsyncApiOperationTrait> Traits { get; set; } = new List<AsyncApiOperationTrait>();

        /// <summary>
        /// A definition of the message that will be published or received on this channel.
        /// oneOf is allowed here to specify multiple messages, however, a message MUST be valid only against
        /// one of the referenced message objects.
        /// </summary>
        public IList<AsyncApiMessage> Message { get; set; } = new List<AsyncApiMessage>();
        
        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiOperation"/> to Async API v2.0.
        /// </summary>
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

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
            
            // traits
            writer.WriteOptionalCollection(AsyncApiConstants.Traits, Traits, (w, p) => p.SerializeAsV2(w));
            
            // message
            writer.WriteOptionalCollection(AsyncApiConstants.Message, Message, (w, p) => p.SerializeAsV2(w));

            // specification extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }
    }
}
