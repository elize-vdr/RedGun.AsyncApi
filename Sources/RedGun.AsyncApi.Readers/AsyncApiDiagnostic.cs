// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.Interface;

namespace RedGun.AsyncApi.Readers
{
    /// <summary>
    /// Object containing all diagnostic information related to Async API parsing.
    /// </summary>
    public class AsyncApiDiagnostic : IDiagnostic
    {
        /// <summary>
        /// List of all errors.
        /// </summary>
        public IList<AsyncApiError> Errors { get; set; } = new List<AsyncApiError>();

        /// <summary>
        /// Async API specification version of the document parsed.
        /// </summary>
        public AsyncApiSpecVersion SpecificationVersion { get; set; }
    }
}
