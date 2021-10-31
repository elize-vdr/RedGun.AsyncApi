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
    public class AsyncApiBindingHttpServer : IAsyncApiSerializable, IAsyncApiExtensible
    {
        // This object MUST NOT contain any properties. Its name is reserved for future use.
        
        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiBindingHttpServer"/> to Async API v2.0
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
    public class AsyncApiBindingHttpChannel : IAsyncApiSerializable, IAsyncApiExtensible
    {
        // This object MUST NOT contain any properties. Its name is reserved for future use.
        
        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiBindingHttpChannel"/> to Async API v2.0
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
    public class AsyncApiBindingHttpOperation : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// REQUIRED. Type of operation. Its value MUST be either request or response.
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// When type is request, this is the HTTP method, otherwise it MUST be ignored.
        /// Its value MUST be one of GET, POST, PUT, PATCH, DELETE, HEAD, OPTIONS, CONNECT, and TRACE.
        /// </summary>
        public string Method { get; set; }
        
        /// <summary>
        /// A Schema object containing the definitions for each query parameter.
        /// This schema MUST be of type object and have a properties key.
        /// </summary>
        // TODO: Add this to validation
        public AsyncApiSchema Query { get; set; }

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

            // type
            writer.WriteProperty(AsyncApiConstants.Type, Type);
            
            // method
            writer.WriteProperty(AsyncApiConstants.Method, Method);
            
            // query
            writer.WriteOptionalObject(AsyncApiConstants.Query, Query, (w, s) => s.SerializeAsV2(w));
            
            // bindingVersion
            writer.WriteProperty(AsyncApiConstants.BindingVersion, BindingVersion);
            
            writer.WriteEndObject();
        }

    }
    
    /// <summary>
    /// Message Binding object.
    /// </summary>
    public class AsyncApiBindingHttpMessage : IAsyncApiSerializable, IAsyncApiExtensible
    {
        // This object MUST NOT contain any properties. Its name is reserved for future use.
        
        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiBindingHttpMessage"/> to Async API v2.0
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
}