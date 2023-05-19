// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// A simple object to allow referencing other components in the specification, internally and externally.
    /// </summary>
    public class AsyncApiReference : IAsyncApiSerializable
    {
        /// <summary>
        /// External resource in the reference.
        /// It maybe:
        /// 1. a absolute/relative file path, for example:  ../commons/pet.json
        /// 2. a Url, for example: http://localhost/pet.json
        /// </summary>
        public string ExternalResource { get; set; }

        /// <summary>
        /// The element type referenced.
        /// </summary>
        /// <remarks>This must be present if <see cref="ExternalResource"/> is not present.</remarks>
        public ReferenceType? Type { get; set; }

        /// <summary>
        /// The identifier of the reusable component of one particular ReferenceType.
        /// If ExternalResource is present, this is the path to the component after the '#/'.
        /// For example, if the reference is 'example.json#/path/to/component', the Id is 'path/to/component'.
        /// If ExternalResource is not present, this is the name of the component without the reference type name.
        /// For example, if the reference is '#/components/schemas/componentName', the Id is 'componentName'.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets a flag indicating whether this reference is an external reference.
        /// </summary>
        public bool IsExternal => ExternalResource != null;

        /// <summary>
        /// Gets a flag indicating whether this reference is a local reference.
        /// </summary>
        public bool IsLocal => ExternalResource == null;

        /// <summary>
        /// Gets the full reference string for v2.0.
        /// </summary>
        public string ReferenceV2
        {
            get
            {
                if (IsExternal)
                {
                    return GetExternalReference();
                }

                if (!Type.HasValue)
                {
                    throw Error.ArgumentNull(nameof(Type));
                }

                if (Type == ReferenceType.Tag)
                {
                    return Id;
                }

                if (Type == ReferenceType.SecurityScheme)
                {
                    return Id;
                }

                return "#/components/" + Type.GetDisplayName() + "/" + Id;
            }
        }

        /// <summary>
        /// Serialize <see cref="AsyncApiReference"/> to Async API v2.0.
        /// </summary>
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            if (Type == ReferenceType.Tag)
            {
                // Write the string value only
                writer.WriteValue(ReferenceV2);
                return;
            }

            if (Type == ReferenceType.SecurityScheme)
            {
                // Write the string as property name
                writer.WritePropertyName(ReferenceV2);
                return;
            }

            writer.WriteStartObject();

            // $ref
            writer.WriteProperty(AsyncApiConstants.DollarRef, ReferenceV2);

            writer.WriteEndObject();
        }

        private string GetExternalReference()
        {
            if (Id != null)
            {
                return ExternalResource + "#/" + Id;
            }

            return ExternalResource;
        }

        private string GetReferenceTypeNameAsV2(ReferenceType type)
        {
            switch (type)
            {
                case ReferenceType.Schema:
                    return AsyncApiConstants.Definitions;

                case ReferenceType.Message:
                    return AsyncApiConstants.Messages;

                case ReferenceType.SecurityScheme:
                    return AsyncApiConstants.SecuritySchemes;

                case ReferenceType.Parameter:
                    return AsyncApiConstants.Parameters;

                case ReferenceType.CorrelationId:
                    return AsyncApiConstants.CorrelationIds;

                case ReferenceType.OperationTrait:
                    return AsyncApiConstants.OperationTraits;

                case ReferenceType.MessageTrait:
                    return AsyncApiConstants.MessageTraits;

                case ReferenceType.ServerBindings:
                    return AsyncApiConstants.ServerBindings;

                case ReferenceType.ChannelBindings:
                    return AsyncApiConstants.ChannelBindings;

                case ReferenceType.OperationBindings:
                    return AsyncApiConstants.OperationBindings;

                case ReferenceType.MessageBindings:
                    return AsyncApiConstants.MessageBindings;

                case ReferenceType.Tag:
                    return AsyncApiConstants.Tags;

                default:
                    // If the reference type is not supported in V2, simply return null
                    // to indicate that the reference is not pointing to any object.
                    return null;
            }
        }
    }
}
