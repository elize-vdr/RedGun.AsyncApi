// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The validation rules for <see cref="AsyncApiHeader"/>.
    /// </summary>
    //[AsyncApiRule]
    public static class AsyncApiHeaderRules
    {
        /// <summary>
        /// Validate the data matches with the given data type.
        /// </summary>
        public static ValidationRule<AsyncApiHeader> HeaderMismatchedDataType =>
            new ValidationRule<AsyncApiHeader>(
                (context, header) =>
                {
                    // example
                    context.Enter("example");

                    if (header.Example != null)
                    {
                        RuleHelpers.ValidateDataTypeMismatch(context, nameof(HeaderMismatchedDataType), header.Example, header.Schema);
                    }

                    context.Exit();

                    // examples
                    context.Enter("examples");

                    if (header.Examples != null)
                    {
                        foreach (var key in header.Examples.Keys)
                        {
                            if (header.Examples[key] != null)
                            {
                                context.Enter(key);
                                context.Enter("value");
                                RuleHelpers.ValidateDataTypeMismatch(context, nameof(HeaderMismatchedDataType), header.Examples[key]?.Value, header.Schema);
                                context.Exit();
                                context.Exit();
                            }
                        }
                    }

                    context.Exit();
                });

        // add more rule.
    }
}
