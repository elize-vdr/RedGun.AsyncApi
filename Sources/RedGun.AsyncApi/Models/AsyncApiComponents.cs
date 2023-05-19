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
    /// Components Object.
    /// </summary>
    public class AsyncApiComponents : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// An object to hold reusable <see cref="AsyncApiSchema"/> Objects.
        /// </summary>
        public IDictionary<string, AsyncApiSchema> Schemas { get; set; } = new Dictionary<string, AsyncApiSchema>();

        /// <summary>
        /// An object to hold reusable <see cref="AsyncApiMessage"/> Objects.
        /// </summary>
        public IDictionary<string, AsyncApiMessage> Messages { get; set; } = new Dictionary<string, AsyncApiMessage>();
        
        /// <summary>
        /// An object to hold reusable <see cref="AsyncApiSecurityScheme"/> Objects.
        /// </summary>
        public IDictionary<string, AsyncApiSecurityScheme> SecuritySchemes { get; set; } = new Dictionary<string, AsyncApiSecurityScheme>();
        
        /// <summary>
        /// An object to hold reusable <see cref="AsyncApiParameter"/> Objects.
        /// </summary>
        public IDictionary<string, AsyncApiParameter> Parameters { get; set; } = new Dictionary<string, AsyncApiParameter>();
        
        /// <summary>
        /// An object to hold reusable <see cref="AsyncApiCorrelationId"/> Objects.
        /// </summary>
        public IDictionary<string, AsyncApiCorrelationId> CorrelationIds { get; set; } = new Dictionary<string, AsyncApiCorrelationId>();
        
        /// <summary>
        /// An object to hold reusable <see cref="AsyncApiOperationTrait"/> Objects.
        /// </summary>
        public IDictionary<string, AsyncApiOperationTrait> OperationTraits { get; set; } = new Dictionary<string, AsyncApiOperationTrait>();

        /// <summary>
        /// An object to hold reusable <see cref="AsyncApiMessageTrait"/> Objects.
        /// </summary>
        public IDictionary<string, AsyncApiMessageTrait> MessageTraits { get; set; } = new Dictionary<string, AsyncApiMessageTrait>();

        /// <summary>
        /// An object to hold reusable <see cref="AsyncApiServerBindings"/> Objects.
        /// </summary>
        public IDictionary<string, AsyncApiServerBindings> ServerBindings { get; set; } = new Dictionary<string, AsyncApiServerBindings>();

        /// <summary>
        /// An object to hold reusable <see cref="AsyncApiChannelBindings"/> Objects.
        /// </summary>
        public IDictionary<string, AsyncApiChannelBindings> ChannelBindings { get; set; } = new Dictionary<string, AsyncApiChannelBindings>();

        /// <summary>
        /// An object to hold reusable <see cref="AsyncApiOperationBindings"/> Objects.
        /// </summary>
        public IDictionary<string, AsyncApiOperationBindings> OperationBindings { get; set; } = new Dictionary<string, AsyncApiOperationBindings>();

        /// <summary>
        /// An object to hold reusable <see cref="AsyncApiMessageBindings"/> Objects.
        /// </summary>
        public IDictionary<string, AsyncApiMessageBindings> MessageBindings { get; set; } = new Dictionary<string, AsyncApiMessageBindings>();

        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiComponents"/> to Async API v2.0.
        /// </summary>
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            // If references have been inlined we don't need to render the components section
            // however if they have cycles, then we will need a component rendered
            if (writer.GetSettings().ReferenceInline != ReferenceInlineSetting.DoNotInlineReferences)
            {
                var loops = writer.GetSettings().LoopDetector.Loops;
                writer.WriteStartObject();
                if (loops.TryGetValue(typeof(AsyncApiSchema), out List<object> schemas))
                {
                    var asyncApiSchemas = schemas.Cast<AsyncApiSchema>().Distinct().ToList()
                        .ToDictionary<AsyncApiSchema, string>(k => k.Reference.Id);

                    writer.WriteOptionalMap(
                       AsyncApiConstants.Schemas,
                       Schemas,
                       (w, key, component) => {
                           component.SerializeAsV2WithoutReference(w);
                           });
                }
                writer.WriteEndObject();
                return;  
            }

            writer.WriteStartObject();

            // Serialize each referencable object as full object without reference if the reference in the object points to itself.
            // If the reference exists but points to other objects, the object is serialized to just that reference.

            // schemas
            writer.WriteOptionalMap(
                AsyncApiConstants.Schemas,
                Schemas,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.Schema &&
                        component.Reference.Id == key)
                    {
                        component.SerializeAsV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeAsV2(w);
                    }
                });

            // messages
            writer.WriteOptionalMap(
                AsyncApiConstants.Messages,
                Messages,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.MessageBindings &&
                        component.Reference.Id == key)
                    {
                        component.SerializeAsV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeAsV2(w);
                    }
                });

            // securitySchemes
            writer.WriteOptionalMap(
                AsyncApiConstants.SecuritySchemes,
                SecuritySchemes,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.SecurityScheme &&
                        component.Reference.Id == key)
                    {
                        component.SerializeAsV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeAsV2(w);
                    }
                });
            
            
            // parameters
            writer.WriteOptionalMap(
                AsyncApiConstants.Parameters,
                Parameters,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.Parameter &&
                        component.Reference.Id == key)
                    {
                        component.SerializeAsV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeAsV2(w);
                    }
                });
            
            // correlationIds
            writer.WriteOptionalMap(
                AsyncApiConstants.CorrelationIds,
                CorrelationIds,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.CorrelationId &&
                        component.Reference.Id == key)
                    {
                        component.SerializeAsV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeAsV2(w);
                    }
                });
            
            // operationTraits
            writer.WriteOptionalMap(
                AsyncApiConstants.OperationTraits,
                OperationTraits,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.OperationTrait &&
                        component.Reference.Id == key)
                    {
                        component.SerializeAsV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeAsV2(w);
                    }
                });

            // messageTraits
            writer.WriteOptionalMap(
                AsyncApiConstants.MessageTraits,
                MessageTraits,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.MessageTrait &&
                        component.Reference.Id == key)
                    {
                        component.SerializeAsV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeAsV2(w);
                    }
                });

            // serverBindings
            writer.WriteOptionalMap(
                AsyncApiConstants.ServerBindings,
                ServerBindings,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.ServerBindings &&
                        component.Reference.Id == key)
                    {
                        component.SerializeAsV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeAsV2(w);
                    }
                });

            // channelBindings
            writer.WriteOptionalMap(
                AsyncApiConstants.ChannelBindings,
                ChannelBindings,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.ChannelBindings &&
                        component.Reference.Id == key)
                    {
                        component.SerializeAsV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeAsV2(w);
                    }
                });

            // operationBindings
            writer.WriteOptionalMap(
                AsyncApiConstants.OperationBindings,
                OperationBindings,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.OperationBindings &&
                        component.Reference.Id == key)
                    {
                        component.SerializeAsV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeAsV2(w);
                    }
                });

            // messageBindings
            writer.WriteOptionalMap(
                AsyncApiConstants.MessageBindings,
                MessageBindings,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.MessageBindings &&
                        component.Reference.Id == key)
                    {
                        component.SerializeAsV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeAsV2(w);
                    }
                });

            // extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }

    }
}
