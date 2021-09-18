// Copyright (c) Microsoft Corporation. All rights reserved.
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
    public class OpenApiReferenceError : OpenApiError
    {
        private AsyncApiReference _reference;
        /// <summary>
        /// Initializes the <see cref="OpenApiError"/> class using the message and pointer from the given exception.
        /// </summary>
        public OpenApiReferenceError(OpenApiException exception) : base(exception.Pointer, exception.Message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="message"></param>
        public OpenApiReferenceError(AsyncApiReference reference, string message) : base("", message)
        {
            _reference = reference;
        }
    }
}
