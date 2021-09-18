﻿// Copyright (c) Microsoft Corporation. All rights reserved.
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
        public IList<OpenApiError> Errors { get; set; } = new List<OpenApiError>();

        /// <summary>
        /// Async API specification version of the document parsed.
        /// </summary>
        public OpenApiSpecVersion SpecificationVersion { get; set; }
    }
}