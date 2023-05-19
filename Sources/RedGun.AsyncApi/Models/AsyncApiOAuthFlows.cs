// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// OAuth Flows Object.
    /// </summary>
    public class AsyncApiOAuthFlows : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// Configuration for the OAuth Implicit flow
        /// </summary>
        public AsyncApiOAuthFlow Implicit { get; set; }

        /// <summary>
        /// Configuration for the OAuth Resource Owner Password flow.
        /// </summary>
        public AsyncApiOAuthFlow Password { get; set; }

        /// <summary>
        /// Configuration for the OAuth Client Credentials flow.
        /// </summary>
        public AsyncApiOAuthFlow ClientCredentials { get; set; }

        /// <summary>
        /// Configuration for the OAuth Authorization Code flow.
        /// </summary>
        public AsyncApiOAuthFlow AuthorizationCode { get; set; }

        /// <summary>
        /// Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiOAuthFlows"/> to Async API v2.0
        /// </summary>
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            writer.WriteStartObject();

            // implicit
            writer.WriteOptionalObject(AsyncApiConstants.Implicit, Implicit, (w, o) => o.SerializeAsV2(w));

            // password
            writer.WriteOptionalObject(AsyncApiConstants.Password, Password, (w, o) => o.SerializeAsV2(w));

            // clientCredentials
            writer.WriteOptionalObject(
                AsyncApiConstants.ClientCredentials,
                ClientCredentials,
                (w, o) => o.SerializeAsV2(w));

            // authorizationCode
            writer.WriteOptionalObject(
                AsyncApiConstants.AuthorizationCode,
                AuthorizationCode,
                (w, o) => o.SerializeAsV2(w));

            // extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }
    }
}
