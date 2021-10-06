// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using RedGun.AsyncApi.Attributes;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// The style of the parameter.
    /// </summary>
    public enum ParameterStyle
    {
        /// <summary>
        /// Path-style parameters.
        /// </summary>
        [Display("matrix")] Matrix,

        /// <summary>
        /// Label style parameters.
        /// </summary>
        [Display("label")] Label,

        /// <summary>
        /// Form style parameters.
        /// </summary>
        [Display("form")] Form,

        /// <summary>
        /// Simple style parameters.
        /// </summary>
        [Display("simple")] Simple,

        /// <summary>
        /// Space separated array values.
        /// </summary>
        [Display("spaceDelimited")] SpaceDelimited,

        /// <summary>
        /// Pipe separated array values.
        /// </summary>
        [Display("pipeDelimited")] PipeDelimited,

        /// <summary>
        /// Provides a simple way of rendering nested objects using form parameters.
        /// </summary>
        [Display("deepObject")] DeepObject
    }
}
