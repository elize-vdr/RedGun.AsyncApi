// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;


namespace RedGun.AsyncApi.Models
{
    // Defines the Server, Channel, Operation and Message Bindings for HTTP Bindings. Current version is 0.1.0.
    
    /// <summary>
    /// Channel Binding object.
    /// </summary>
    public class AsyncApiBindingWebSocketsChannel : IAsyncApiSerializable, IAsyncApiExtensible
    {
        
        /// <summary>
        /// The HTTP method to use when establishing the connection. Its value MUST be either GET or POST.
        /// </summary>
        // TODO: Add this to validation
        public string Method { get; set; }
        
        /// <summary>
        /// A Schema object containing the definitions for each query parameter.
        /// This schema MUST be of type object and have a properties key.
        /// </summary>
        // TODO: Add this to validation
        public AsyncApiSchema Query { get; set; }
        
        /// <summary>
        /// A Schema object containing the definitions of the HTTP headers to use when establishing the connection.
        /// This schema MUST be of type object and have a properties key.
        /// </summary>
        // TODO: Add this to validation
        public AsyncApiSchema Headers { get; set; }

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
            
            // method
            writer.WriteProperty(AsyncApiConstants.Method, Method);
            
            // query
            writer.WriteOptionalObject(AsyncApiConstants.Query, Query, (w, s) => s.SerializeAsV2(w));
            
            // headers
            writer.WriteOptionalObject(AsyncApiConstants.Headers, Headers, (w, s) => s.SerializeAsV2(w));
            
            // bindingVersion
            writer.WriteProperty(AsyncApiConstants.BindingVersion, BindingVersion);
            
            writer.WriteEndObject();
        }

    }

}