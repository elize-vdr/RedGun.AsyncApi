// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using RedGun.AsyncApi.Attributes;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// The location of the parameter.
    /// </summary>
    public enum ParameterLocation
    {
        /// <summary>
        /// Parameters that are appended to the URL.
        /// </summary>
        [Display("query")] Query,

        /// <summary>
        /// Custom headers that are expected as part of the request.
        /// </summary>
        [Display("header")] Header,

        /// <summary>
        /// Used together with Path Templating,
        /// where the parameter value is actually part of the operation's URL
        /// </summary>
        [Display("path")] Path,

        /// <summary>
        /// Used to pass a specific cookie value to the API.
        /// </summary>
        [Display("cookie")] Cookie
    }
}
