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
    /// Helper methods to simplify creating validation rules
    /// </summary>
    public static class ValidationContextExtensions
    {
        /// <summary>
        /// Helper method to simplify validation rules
        /// </summary>
        public static void CreateError(this IValidationContext context, string ruleName, string message)
        {
            AsyncApiValidatorError error = new AsyncApiValidatorError(ruleName, context.PathString, message);
            context.AddError(error);
        }
    }
}
