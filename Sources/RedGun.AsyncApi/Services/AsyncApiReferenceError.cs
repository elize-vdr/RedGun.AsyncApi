// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedGun.AsyncApi.Exceptions;
using RedGun.AsyncApi.Models;

namespace RedGun.AsyncApi.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class AsyncApiReferenceError : AsyncApiError
    {
        private AsyncApiReference _reference;
        /// <summary>
        /// Initializes the <see cref="AsyncApiError"/> class using the message and pointer from the given exception.
        /// </summary>
        public AsyncApiReferenceError(AsyncApiException exception) : base(exception.Pointer, exception.Message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="message"></param>
        public AsyncApiReferenceError(AsyncApiReference reference, string message) : base("", message)
        {
            _reference = reference;
        }
    }
}
