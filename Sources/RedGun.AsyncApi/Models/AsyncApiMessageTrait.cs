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
    /// MessageTrait Object.
    /// </summary>
    public class AsyncApiMessageTrait : IAsyncApiSerializable, IAsyncApiReferenceable, IAsyncApiExtensible
    {
        
        /// <summary>
        /// Schema definition of the application headers. Schema MUST be of type "object". It MUST NOT define the protocol headers.
        /// </summary>
        public AsyncApiSchema Headers { get; set; }
        
        /// <summary>
        /// Definition of the correlation ID used for message tracing or matching.
        /// </summary>
        public AsyncApiCorrelationId CorrelationId { get; set; }
    
        /// <summary>
        /// A string containing the name of the schema format/language used to define the message payload.
        /// If omitted, implementations should parse the payload as a Schema object.
        /// </summary>
        public string SchemaFormat { get; set; }
        
        /// <summary>
        /// The content type to use when encoding/decoding a message's payload.
        /// The value MUST be a specific media type (e.g. application/json).
        /// When omitted, the value MUST be the one specified on the defaultContentType field.
        /// </summary>
        public string ContentType { get; set; }
        
        /// <summary>
        /// A machine-friendly name for the message.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// A human-friendly title for the message.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A short summary of what the message is about.
        /// </summary>
        public string Summary { get; set; }
        
        /// <summary>
        /// A verbose explanation of the message. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// A list of tags for API documentation control.
        /// Tags can be used for logical grouping of messages.
        /// </summary>
        public IList<AsyncApiTag> Tags { get; set; } = new List<AsyncApiTag>();
        
        /// <summary>
        /// Additional external documentation for this message.
        /// </summary>
        public AsyncApiExternalDocs ExternalDocs { get; set; }
        
        /// <summary>
        /// A map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the message.
        /// </summary>
        public AsyncApiMessageBindings Bindings { get; set; }   
        
        /// <summary>
        /// List of examples.
        /// </summary>
        public IList<AsyncApiMessageExample> Examples { get; set; } = new List<AsyncApiMessageExample>();

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
        /// Serialize <see cref="AsyncApiMessageTrait"/> to Async API v2.0
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

            // headers
            writer.WriteOptionalObject(AsyncApiConstants.Headers, Headers, (w, s) => s.SerializeAsV2(w));
            
            // correlationId
            writer.WriteOptionalObject(AsyncApiConstants.CorrelationId, CorrelationId, (w, s) => s.SerializeAsV2(w));
            
            // schemaFormat
            writer.WriteProperty(AsyncApiConstants.SchemaFormat, SchemaFormat);
            
            // contentType
            writer.WriteProperty(AsyncApiConstants.ContentType, ContentType);
            
            // name
            writer.WriteProperty(AsyncApiConstants.Name, Name);
            
            // title
            writer.WriteProperty(AsyncApiConstants.Title, Title);
            
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
            
            // examples
            writer.WriteOptionalCollection(AsyncApiConstants.Examples, Examples, (w, t) => t.SerializeAsV2(w));

            // extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }
    }

    /// <summary>
    /// Body messageTrait class to propagate information needed for <see cref="AsyncApiMessageTrait.SerializeAsV2"/>
    /// </summary>
    internal class AsyncApiBodyMessageTrait : AsyncApiMessageTrait
    {
    }

    /// <summary>
    /// Form messageTrait class to propagate information needed for <see cref="AsyncApiMessageTrait.SerializeAsV2"/>
    /// </summary>
    internal class AsyncApiFormDataMessageTrait : AsyncApiMessageTrait
    {
    }
}
