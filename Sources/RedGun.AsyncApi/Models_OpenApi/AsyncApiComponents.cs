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
        /// An object to hold reusable <see cref="AsyncApiResponse"/> Objects.
        /// </summary>
        public IDictionary<string, AsyncApiResponse> Responses { get; set; } = new Dictionary<string, AsyncApiResponse>();

        /// <summary>
        /// An object to hold reusable <see cref="AsyncApiParameter"/> Objects.
        /// </summary>
        public IDictionary<string, AsyncApiParameter> Parameters { get; set; } =
            new Dictionary<string, AsyncApiParameter>();

        /// <summary>
        /// An object to hold reusable <see cref="AsyncApiExample"/> Objects.
        /// </summary>
        public IDictionary<string, AsyncApiExample> Examples { get; set; } = new Dictionary<string, AsyncApiExample>();

        /// <summary>
        /// An object to hold reusable <see cref="AsyncApiRequestBody"/> Objects.
        /// </summary>
        public IDictionary<string, AsyncApiRequestBody> RequestBodies { get; set; } =
            new Dictionary<string, AsyncApiRequestBody>();

        /// <summary>
        /// An object to hold reusable <see cref="AsyncApiHeader"/> Objects.
        /// </summary>
        public IDictionary<string, AsyncApiHeader> Headers { get; set; } = new Dictionary<string, AsyncApiHeader>();

        /// <summary>
        /// An object to hold reusable <see cref="AsyncApiSecurityScheme"/> Objects.
        /// </summary>
        public IDictionary<string, AsyncApiSecurityScheme> SecuritySchemes { get; set; } =
            new Dictionary<string, AsyncApiSecurityScheme>();

        /// <summary>
        /// An object to hold reusable <see cref="AsyncApiLink"/> Objects.
        /// </summary>
        public IDictionary<string, AsyncApiLink> Links { get; set; } = new Dictionary<string, AsyncApiLink>();

        /// <summary>
        /// An object to hold reusable <see cref="AsyncApiCallback"/> Objects.
        /// </summary>
        public IDictionary<string, AsyncApiCallback> Callbacks { get; set; } = new Dictionary<string, AsyncApiCallback>();

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

            // If references have been inlined we don't need the to render the components section
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

            // Serialize each referenceable object as full object without reference if the reference in the object points to itself.
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

            // responses
            writer.WriteOptionalMap(
                AsyncApiConstants.Responses,
                Responses,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.Response &&
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

            // examples
            writer.WriteOptionalMap(
                AsyncApiConstants.Examples,
                Examples,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.Example &&
                        component.Reference.Id == key)
                    {
                        component.SerializeAsV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeAsV2(w);
                    }
                });

            // requestBodies
            writer.WriteOptionalMap(
                AsyncApiConstants.RequestBodies,
                RequestBodies,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.RequestBody &&
                        component.Reference.Id == key)
                    {
                        component.SerializeAsV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeAsV2(w);
                    }
                });

            // headers
            writer.WriteOptionalMap(
                AsyncApiConstants.Headers,
                Headers,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.Header &&
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

            // links
            writer.WriteOptionalMap(
                AsyncApiConstants.Links,
                Links,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.Link &&
                        component.Reference.Id == key)
                    {
                        component.SerializeAsV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeAsV2(w);
                    }
                });

            // callbacks
            writer.WriteOptionalMap(
                AsyncApiConstants.Callbacks,
                Callbacks,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.Callback &&
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
