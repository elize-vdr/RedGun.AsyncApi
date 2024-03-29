﻿// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using RedGun.AsyncApi.Attributes;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// Operation type.
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// A definition of a GET operation on this path.
        /// </summary>
        [Display("get")] Get,

        /// <summary>
        /// A definition of a PUT operation on this path.
        /// </summary>
        [Display("put")] Put,

        /// <summary>
        /// A definition of a POST operation on this path.
        /// </summary>
        [Display("post")] Post,

        /// <summary>
        /// A definition of a DELETE operation on this path.
        /// </summary>
        [Display("delete")] Delete,

        /// <summary>
        /// A definition of a OPTIONS operation on this path.
        /// </summary>
        [Display("options")] Options,

        /// <summary>
        /// A definition of a HEAD operation on this path.
        /// </summary>
        [Display("head")] Head,

        /// <summary>
        /// A definition of a PATCH operation on this path.
        /// </summary>
        [Display("patch")] Patch,

        /// <summary>
        /// A definition of a TRACE operation on this path.
        /// </summary>
        [Display("trace")] Trace
    }
}
