// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// Channel Item Object: Holds the relative paths to the individual channel and their operations. Channel paths are relative to servers.
    /// Channels are also known as "topics", "routing keys", "event types" or "paths".
    /// </summary>
    public class AsyncApiChannelItem : IAsyncApiSerializable, IAsyncApiExtensible, IAsyncApiReferenceable
    {
        /// <summary>
        /// An optional, string description, intended to apply to all operations in this path.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The servers on which this channel is available, specified as an optional unordered list of names (string keys) of Server
        /// Objects defined in the Servers Object (a map). If servers is absent or empty then this channel must be available
        /// on all servers defined in the Servers Object.
        /// </summary>
        // TODO: Must add a resolver/validation for the servers listed here, they must exist in ServersObject
        public IList<string> Servers { get; set; } = new List<string>();    
        
        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Indicates if object is populated with data or is just a reference to the data
        /// </summary>
        public bool UnresolvedReference { get; set; }

        /// <summary>
        /// Reference object.
        /// </summary>
        public AsyncApiReference Reference { get; set; }

        /// <summary>
        /// A definition of the SUBSCRIBE operation, which defines the messages produced by the application and sent to the channel.
        /// </summary>
        public AsyncApiOperation Subscribe { get; set; }

        /// <summary>
        /// A definition of the PUBLISH operation, which defines the messages consumed by the application from the channel.
        /// </summary>
        public AsyncApiOperation Publish { get; set; }
        
        /// <summary>
        /// A map of the parameters included in the channel name.
        /// It SHOULD be present only when using channels with expressions (as defined by RFC 6570 section 2.2).
        /// </summary>
        public AsyncApiParameters Parameters { get; set; } = new AsyncApiParameters();

        /// <summary>
        /// A map where the keys describe the name of the protocol and the values describe protocol-specific
        /// definitions for the channel.
        /// </summary>
        public AsyncApiChannelBindings Bindings { get; set; }

        /// <summary>
        /// Serialize <see cref="AsyncApiChannelItem"/> to Async API v2.0
        /// </summary>
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            if (Reference != null && writer.GetSettings().ReferenceInline != ReferenceInlineSetting.InlineAllReferences)
            {
                Reference.SerializeAsV2(writer);
                return;
            }

            SerializeAsV2WithoutReference(writer);

        }
        
        /// <summary>
        /// Serialize inline PathItem in AsyncAPI V2
        /// </summary>
        /// <param name="writer"></param>
        public void SerializeAsV2WithoutReference(IAsyncApiWriter writer)
        {

            writer.WriteStartObject();

            // description
            writer.WriteProperty(AsyncApiConstants.Description, Description);

            // servers
            writer.WriteOptionalCollection(AsyncApiConstants.Servers, Servers, (w, s) => w.WriteValue(s));
            
            // subscribe
            writer.WriteOptionalObject(AsyncApiConstants.Subscribe, Subscribe, (w, l) => l.SerializeAsV2(w));
            
            // publish
            writer.WriteOptionalObject(AsyncApiConstants.Publish, Publish, (w, l) => l.SerializeAsV2(w));
            
            // parameters
            writer.WriteOptionalObject(AsyncApiConstants.Parameters, Parameters, (w, l) => l.SerializeAsV2(w));
            
            // bindings
            writer.WriteOptionalObject(AsyncApiConstants.Bindings, Bindings, (w, l) => l.SerializeAsV2(w));

            // specification extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }      
    }
}
