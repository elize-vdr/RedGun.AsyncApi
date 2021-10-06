// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Any;

namespace RedGun.AsyncApi.Interfaces
{
    /// <summary>
    /// Represents an Extensible Async API element.
    /// </summary>
    public interface IAsyncApiExtensible : IAsyncApiElement
    {
        /// <summary>
        /// Specification extensions.
        /// </summary>
        IDictionary<string, IAsyncApiExtension> Extensions { get; set; }
    }
}
