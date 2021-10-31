// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// Operation Bindings object.
    /// </summary>
    public class AsyncApiOperationBindings : IAsyncApiSerializable, IAsyncApiReferenceable, IAsyncApiExtensible
    {
        /// <summary>
        /// Protocol-specific information for an HTTP operation.
        /// </summary>
        public AsyncApiBindingHttpOperation BindingHttp { get; set; }
        
        /* TODO: Add rest of channel binding fixed fields, see: https://www.asyncapi.com/docs/specifications/v2.2.0#operationBindingsObject
            kafka	Kafka Operation Binding	Protocol-specific information for a Kafka operation.
            anypointmq	Anypoint MQ Operation Binding	Protocol-specific information for an Anypoint MQ operation.
            amqp	AMQP Operation Binding	Protocol-specific information for an AMQP 0-9-1 operation.
            amqp1	AMQP 1.0 Operation Binding	Protocol-specific information for an AMQP 1.0 operation.
            mqtt	MQTT Operation Binding	Protocol-specific information for an MQTT operation.
            mqtt5	MQTT 5 Operation Binding	Protocol-specific information for an MQTT 5 operation.
            nats	NATS Operation Binding	Protocol-specific information for a NATS operation.
            jms	JMS Operation Binding	Protocol-specific information for a JMS operation.
            sns	SNS Operation Binding	Protocol-specific information for an SNS operation.
            sqs	SQS Operation Binding	Protocol-specific information for an SQS operation.
            stomp	STOMP Operation Binding	Protocol-specific information for a STOMP operation.
            redis	Redis Operation Binding	Protocol-specific information for a Redis operation.
            mercure	Mercure Operation Binding	Protocol-specific information for a Mercure operation.
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
        /// Serialize <see cref="AsyncApiParameter"/> to Async API v2.0
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

            // http
            writer.WriteOptionalObject(AsyncApiConstants.BindingHttp, BindingHttp, (w, s) => s.SerializeAsV2(w));
            
            // TODO: Add rest of bindings

            // extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }

    }
}
