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
    /// Operation Object.
    /// </summary>
    public class AsyncApiOperation : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// Default value for <see cref="Deprecated"/>.
        /// </summary>
        public const bool DeprecatedDefault = false;

        /// <summary>
        /// A list of tags for API documentation control.
        /// Tags can be used for logical grouping of operations by resources or any other qualifier.
        /// </summary>
        public IList<AsyncApiTag> Tags { get; set; } = new List<AsyncApiTag>();

        /// <summary>
        /// A short summary of what the operation does.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// A verbose explanation of the operation behavior.
        /// CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Additional external documentation for this operation.
        /// </summary>
        public AsyncApiExternalDocs ExternalDocs { get; set; }

        /// <summary>
        /// Unique string used to identify the operation. The id MUST be unique among all operations described in the API.
        /// Tools and libraries MAY use the operationId to uniquely identify an operation, therefore,
        /// it is RECOMMENDED to follow common programming naming conventions.
        /// </summary>
        public string OperationId { get; set; }

        /// <summary>
        /// A list of parameters that are applicable for this operation.
        /// If a parameter is already defined at the Path Item, the new definition will override it but can never remove it.
        /// The list MUST NOT include duplicated parameters. A unique parameter is defined by a combination of a name and location.
        /// The list can use the Reference Object to link to parameters that are defined at the AsyncAPI Object's components/parameters.
        /// </summary>
        public IList<AsyncApiParameter> Parameters { get; set; } = new List<AsyncApiParameter>();

        /// <summary>
        /// The request body applicable for this operation.
        /// The requestBody is only supported in HTTP methods where the HTTP 1.1 specification RFC7231
        /// has explicitly defined semantics for request bodies.
        /// In other cases where the HTTP spec is vague, requestBody SHALL be ignored by consumers.
        /// </summary>
        public AsyncApiRequestBody RequestBody { get; set; }

        /// <summary>
        /// REQUIRED. The list of possible responses as they are returned from executing this operation.
        /// </summary>
        public AsyncApiResponses Responses { get; set; } = new AsyncApiResponses();

        /// <summary>
        /// A map of possible out-of band callbacks related to the parent operation.
        /// The key is a unique identifier for the Callback Object.
        /// Each value in the map is a Callback Object that describes a request
        /// that may be initiated by the API provider and the expected responses.
        /// The key value used to identify the callback object is an expression, evaluated at runtime,
        /// that identifies a URL to use for the callback operation.
        /// </summary>
        public IDictionary<string, AsyncApiCallback> Callbacks { get; set; } = new Dictionary<string, AsyncApiCallback>();

        /// <summary>
        /// Declares this operation to be deprecated. Consumers SHOULD refrain from usage of the declared operation.
        /// </summary>
        public bool Deprecated { get; set; } = DeprecatedDefault;

        /// <summary>
        /// A declaration of which security mechanisms can be used for this operation.
        /// The list of values includes alternative security requirement objects that can be used.
        /// Only one of the security requirement objects need to be satisfied to authorize a request.
        /// This definition overrides any declared top-level security.
        /// To remove a top-level security declaration, an empty array can be used.
        /// </summary>
        public IList<AsyncApiSecurityRequirement> Security { get; set; } = new List<AsyncApiSecurityRequirement>();

        /// <summary>
        /// An alternative server array to service this operation.
        /// If an alternative server object is specified at the Path Item Object or Root level,
        /// it will be overridden by this value.
        /// </summary>
        public IList<AsyncApiServer> Servers { get; set; } = new List<AsyncApiServer>();

        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiOperation"/> to Async API v2.0.
        /// </summary>
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            writer.WriteStartObject();

            // tags
            writer.WriteOptionalCollection(
                AsyncApiConstants.Tags,
                Tags,
                (w, t) =>
                {
                    t.SerializeAsV2(w);
                });

            // summary
            writer.WriteProperty(AsyncApiConstants.Summary, Summary);

            // description
            writer.WriteProperty(AsyncApiConstants.Description, Description);

            // externalDocs
            writer.WriteOptionalObject(AsyncApiConstants.ExternalDocs, ExternalDocs, (w, e) => e.SerializeAsV2(w));

            // operationId
            writer.WriteProperty(AsyncApiConstants.OperationId, OperationId);

            // parameters
            writer.WriteOptionalCollection(AsyncApiConstants.Parameters, Parameters, (w, p) => p.SerializeAsV2(w));

            // requestBody
            writer.WriteOptionalObject(AsyncApiConstants.RequestBody, RequestBody, (w, r) => r.SerializeAsV2(w));

            // responses
            writer.WriteRequiredObject(AsyncApiConstants.Responses, Responses, (w, r) => r.SerializeAsV2(w));

            // callbacks
            writer.WriteOptionalMap(AsyncApiConstants.Callbacks, Callbacks, (w, c) => c.SerializeAsV2(w));

            // deprecated
            writer.WriteProperty(AsyncApiConstants.Deprecated, Deprecated, false);

            // security
            writer.WriteOptionalCollection(AsyncApiConstants.Security, Security, (w, s) => s.SerializeAsV2(w));

            // servers
            writer.WriteOptionalCollection(AsyncApiConstants.Servers, Servers, (w, s) => s.SerializeAsV2(w));

            // specification extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }
    }
}
