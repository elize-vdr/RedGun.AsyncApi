// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;


namespace RedGun.AsyncApi.Models
{
    // Defines the Server, Channel, Operation and Message Bindings for HTTP Bindings. Current version is 0.1.0.
    
    /// <summary>
    /// Server Binding object.
    /// </summary>
    public class AsyncApiBindingKafkaServer : IAsyncApiSerializable, IAsyncApiExtensible
    {
        // This object MUST NOT contain any properties. Its name is reserved for future use.
        
        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiBindingKafkaServer"/> to Async API v2.0
        /// </summary>
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }
            
            // writer.WriteStartObject();

            
            // writer.WriteEndObject();
        }
    }
    
    /// <summary>
    /// Channel Binding object.
    /// </summary>
    public class AsyncApiBindingKafkaChannel : IAsyncApiSerializable, IAsyncApiExtensible
    {
        // This object MUST NOT contain any properties. Its name is reserved for future use.
        
        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiBindingKafkaChannel"/> to Async API v2.0
        /// </summary>
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }
            
            // writer.WriteStartObject();

            
            // writer.WriteEndObject();
        }
    }
    
    /// <summary>
    /// Operation Binding object.
    /// </summary>
    public class AsyncApiBindingKafkaOperation : IAsyncApiSerializable, IAsyncApiExtensible
    {
        // This object MUST NOT contain any properties. Its name is reserved for future use.
        
        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiBindingKafkaChannel"/> to Async API v2.0
        /// </summary>
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }
            
            // writer.WriteStartObject();

            
            // writer.WriteEndObject();
        }
    }
    
    /// <summary>
    /// Message Binding object.
    /// </summary>
    public class AsyncApiBindingKafkaMessage : IAsyncApiSerializable, IAsyncApiExtensible
    {
       
        /// <summary>
        /// The message key. NOTE: You can also use the reference object way.
        /// </summary>
        // TODO: This can also be an AVRO Schema Object ??????
        public AsyncApiSchema Key { get; set; }

        /// <summary>
        /// The version of this binding. If omitted, "latest" MUST be assumed.
        /// </summary>
        public string BindingVersion { get; set; }
        
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
            
            writer.WriteStartObject();
            
            // query
            writer.WriteOptionalObject(AsyncApiConstants.Key, Key, (w, s) => s.SerializeAsV2(w));
            
            // bindingVersion
            writer.WriteProperty(AsyncApiConstants.BindingVersion, BindingVersion);
            
            writer.WriteEndObject();
        }
    }
}