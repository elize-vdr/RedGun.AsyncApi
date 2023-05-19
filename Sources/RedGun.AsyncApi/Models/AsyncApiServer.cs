// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// Server Object: an object representing a Server.
    /// </summary>
    public class AsyncApiServer : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// An optional string describing the host designated by the URL. CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// REQUIRED. A URL to the target host. This URL supports Server Variables and MAY be relative,
        /// to indicate that the host location is relative to the location where the AsyncAPI document is being served.
        /// Variable substitutions will be made when a variable is named in {brackets}.
        /// </summary>
        public string Url { get; set; }
        
        /// <summary>
        /// REQUIRED. The protocol this URL supports for connection. Supported protocol include, but are not limited
        /// to: amqp, amqps, http, https, ibmmq, jms, kafka, kafka-secure, anypointmq, mqtt, secure-mqtt, stomp,
        /// stomps, ws, wss, mercure.
        /// </summary>
        public string Protocol { get; set; }
        
        /// <summary>
        /// The version of the protocol used for connection. For instance: AMQP 0.9.1, HTTP 2.0, Kafka 1.0.0, etc.
        /// </summary>
        public string ProtocolVersion { get; set; }

        /// <summary>
        /// A map between a variable name and its value. The value is used for substitution in the server's URL template.
        /// </summary>
        public IDictionary<string, AsyncApiServerVariable> Variables { get; set; } = new Dictionary<string, AsyncApiServerVariable>();
        
        /// <summary>
        /// A declaration of which security mechanisms can be used with this server. The list of values includes alternative
        /// security requirement objects that can be used. Only one of the security requirement objects need to be satisfied
        /// to authorize a connection or operation.
        /// </summary>
        public IList<AsyncApiSecurityRequirement> SecurityRequirements { get; set; } = new List<AsyncApiSecurityRequirement>();
        
        /// <summary>
        /// A map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the server.
        /// </summary>
        public AsyncApiServerBindings Bindings { get; set; }

        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiServer"/> to Async API v2.0
        /// </summary>
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            writer.WriteStartObject();

            // url
            writer.WriteRequiredProperty(AsyncApiConstants.Url, Url);
            
            // protocol
            writer.WriteRequiredProperty(AsyncApiConstants.Protocol, Protocol);
            
            // protocolVersion
            writer.WriteProperty(AsyncApiConstants.ProtocolVersion, ProtocolVersion);

            // description
            writer.WriteProperty(AsyncApiConstants.Description, Description);

            // variables
            writer.WriteOptionalMap(AsyncApiConstants.Variables, Variables, (w, v) => v.SerializeAsV2(w));
            
            // security
            writer.WriteOptionalCollection(AsyncApiConstants.Security, SecurityRequirements, (w, s) => s.SerializeAsV2(w));
            
            // bindings
            writer.WriteOptionalObject(AsyncApiConstants.Bindings, Bindings, (w, l) => l.SerializeAsV2(w));

            // specification extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }
    }
}
