// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedGun.AsyncApi.Models;

namespace RedGun.AsyncApi.Validations
{
    /// <summary>
    /// Errors detected when validating an AsyncAPI Element
    /// </summary>
    public class AsyncApiValidatorError : AsyncApiError
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiError"/> class.
        /// </summary>
        public AsyncApiValidatorError(string ruleName, string pointer, string message) : base(pointer, message)
        {
            RuleName = ruleName;
        }

        /// <summary>
        /// Name of rule that detected the error.
        /// </summary>
        public string RuleName { get; set; }
    }
}
