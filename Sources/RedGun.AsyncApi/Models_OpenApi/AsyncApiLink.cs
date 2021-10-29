// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// Link Object.
    /// </summary>
    public class AsyncApiLink : IAsyncApiSerializable, IAsyncApiReferenceable, IAsyncApiExtensible
    {
        /// <summary>
        /// A relative or absolute reference to an OAS operation.
        /// This field is mutually exclusive of the operationId field, and MUST point to an Operation Object.
        /// </summary>
        public string OperationRef { get; set; }

        /// <summary>
        /// The name of an existing, resolvable OAS operation, as defined with a unique operationId.
        /// This field is mutually exclusive of the operationRef field.
        /// </summary>
        public string OperationId { get; set; }

        /// <summary>
        /// A map representing parameters to pass to an operation as specified with operationId or identified via operationRef.
        /// </summary>
        public Dictionary<string, RuntimeExpressionAnyWrapper> Parameters { get; set; } =
            new Dictionary<string, RuntimeExpressionAnyWrapper>();

        /// <summary>
        /// A literal value or {expression} to use as a request body when calling the target operation.
        /// </summary>
        public RuntimeExpressionAnyWrapper RequestBody { get; set; }

        /// <summary>
        /// A description of the link.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A server object to be used by the target operation.
        /// </summary>
        public AsyncApiServer Server { get; set; }

        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Indicates if object is populated with data or is just a reference to the data
        /// </summary>
        public bool UnresolvedReference { get; set; }

        /// <summary>
        /// Reference pointer.
        /// </summary>
        public AsyncApiReference Reference { get; set; }

        /// <summary>
        /// Serialize <see cref="AsyncApiLink"/> to Async API v2.0
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

            // operationRef
            writer.WriteProperty(AsyncApiConstants.OperationRef, OperationRef);

            // operationId
            writer.WriteProperty(AsyncApiConstants.OperationId, OperationId);

            // parameters
            writer.WriteOptionalMap(AsyncApiConstants.Parameters, Parameters, (w, p) => p.WriteValue(w));

            // requestBody
            writer.WriteOptionalObject(AsyncApiConstants.RequestBody, RequestBody, (w, r) => r.WriteValue(w));

            // description
            writer.WriteProperty(AsyncApiConstants.Description, Description);

            // server
            writer.WriteOptionalObject(AsyncApiConstants.Server, Server, (w, s) => s.SerializeAsV2(w));

            writer.WriteEndObject();
        }
        
    }
}
