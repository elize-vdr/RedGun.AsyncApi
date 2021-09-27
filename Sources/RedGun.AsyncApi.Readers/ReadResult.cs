// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedGun.AsyncApi.Models;

namespace RedGun.AsyncApi.Readers
{
    /// <summary>
    /// Container object used for returning the result of reading an AsyncAPI description.
    /// </summary>
    public class ReadResult
    {
        /// <summary>
        /// The parsed AsyncApiDocument.  Null will be returned if the document could not be parsed.
        /// </summary>
        public AsyncApiDocument AsyncApiDocument { set; get; }
        /// <summary>
        /// AsyncApiDiagnostic contains the Errors reported while parsing
        /// </summary>
        public AsyncApiDiagnostic AsyncApiDiagnostic { set; get; }
    }
}
