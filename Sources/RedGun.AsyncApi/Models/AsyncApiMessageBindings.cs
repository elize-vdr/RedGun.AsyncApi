// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// Message Bindings object.
    /// </summary>
    public class AsyncApiMessageBindings : IAsyncApiSerializable, IAsyncApiReferenceable, IAsyncApiExtensible
    {
      
        /// <summary>
        /// Protocol-specific information for a Kafka message.
        /// </summary>
        public AsyncApiBindingKafkaMessage BindingKafka { get; set; }
        
        /* TODO: Add rest of message binding fixed fields, see: https://www.asyncapi.com/docs/specifications/v2.2.0#messageBindingsObject
            kafka	Kafka Message Binding	Protocol-specific information for a Kafka message.
            anypointmq	Anypoint MQ Message Binding	Protocol-specific information for an Anypoint MQ message.
            amqp	AMQP Message Binding	Protocol-specific information for an AMQP 0-9-1 message.
            amqp1	AMQP 1.0 Message Binding	Protocol-specific information for an AMQP 1.0 message.
            mqtt	MQTT Message Binding	Protocol-specific information for an MQTT message.
            mqtt5	MQTT 5 Message Binding	Protocol-specific information for an MQTT 5 message.
            nats	NATS Message Binding	Protocol-specific information for a NATS message.
            jms	JMS Message Binding	Protocol-specific information for a JMS message.
            sns	SNS Message Binding	Protocol-specific information for an SNS message.
            sqs	SQS Message Binding	Protocol-specific information for an SQS message.
            stomp	STOMP Message Binding	Protocol-specific information for a STOMP message.
            redis	Redis Message Binding	Protocol-specific information for a Redis message.
            mercure	Mercure Message Binding	Protocol-specific information for a Mercure message.
            ibmmq	IBM MQ Message Binding	Protocol-specific information for an IBM MQ message.
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
        /// Serialize <see cref="AsyncApiMessageBindings"/> to Async API v2.0
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

            // kafka
            writer.WriteOptionalObject(AsyncApiConstants.BindingKafka, BindingKafka, (w, s) => s.SerializeAsV2(w));
            
            // TODO: Add rest of bindings

            // extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }

    }
}
