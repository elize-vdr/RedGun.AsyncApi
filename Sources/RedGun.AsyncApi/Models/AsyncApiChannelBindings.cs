// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// Channel Bindings object.
    /// </summary>
    public class AsyncApiChannelBindings : IAsyncApiSerializable, IAsyncApiReferenceable, IAsyncApiExtensible
    {
        
        /// <summary>
        /// Protocol-specific information for a WebSockets channel.
        /// When using WebSockets, the channel represents the connection.
        /// Unlike other protocols that support multiple virtual channels (topics, routing keys, etc.) per connection,
        /// WebSockets doesn't support virtual channels or, put it another way, there's only one channel and its characteristics
        /// are strongly related to the protocol used for the handshake, i.e., HTTP.
        /// </summary>
        public AsyncApiBindingWebSocketsChannel BindingWebSockets { get; set; }
        
        /* TODO: Add rest of channel binding fixed fields, see: https://www.asyncapi.com/docs/specifications/v2.2.0#channelBindingsObject
            kafka	Kafka Channel Binding	Protocol-specific information for a Kafka channel.
            anypointmq	Anypoint MQ Channel Binding	Protocol-specific information for an Anypoint MQ channel.
            amqp	AMQP Channel Binding	Protocol-specific information for an AMQP 0-9-1 channel.
            amqp1	AMQP 1.0 Channel Binding	Protocol-specific information for an AMQP 1.0 channel.
            mqtt	MQTT Channel Binding	Protocol-specific information for an MQTT channel.
            mqtt5	MQTT 5 Channel Binding	Protocol-specific information for an MQTT 5 channel.
            nats	NATS Channel Binding	Protocol-specific information for a NATS channel.
            jms	JMS Channel Binding	Protocol-specific information for a JMS channel.
            sns	SNS Channel Binding	Protocol-specific information for an SNS channel.
            sqs	SQS Channel Binding	Protocol-specific information for an SQS channel.
            stomp	STOMP Channel Binding	Protocol-specific information for a STOMP channel.
            redis	Redis Channel Binding	Protocol-specific information for a Redis channel.
            mercure	Mercure Channel Binding	Protocol-specific information for a Mercure channel.
            ibmmq	IBM MQ Channel Binding	Protocol-specific information for an IBM MQ channel.
         */

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
        /// Serialize <see cref="AsyncApiChannelBindings"/> to Async API v2.0
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

            // ws
            writer.WriteOptionalObject(AsyncApiConstants.BindingWebSockets, BindingWebSockets, (w, s) => s.SerializeAsV2(w));
            
            // TODO: Add rest of bindings

            // extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }

    }
}
