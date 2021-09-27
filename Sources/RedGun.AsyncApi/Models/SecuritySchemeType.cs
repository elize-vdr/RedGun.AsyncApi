// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using RedGun.AsyncApi.Attributes;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// The type of the security scheme
    /// </summary>
    public enum SecuritySchemeType
    {
        /// <summary>
        /// Use API key
        /// </summary>
        [Display("apiKey")] ApiKey,

        /// <summary>
        /// Use basic or bearer token authorization header.
        /// </summary>
        [Display("http")] Http,

        /// <summary>
        /// Use OAuth2
        /// </summary>
        [Display("oauth2")] OAuth2,

        /// <summary>
        /// Use OAuth2 with OpenId Connect URL to discover OAuth2 configuration value.
        /// </summary>
        [Display("openIdConnect")] OpenIdConnect
    }
}
