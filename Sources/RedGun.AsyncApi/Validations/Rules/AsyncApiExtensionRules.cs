// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The validation rules for <see cref="IAsyncApiExtensible"/>.
    /// </summary>
    [AsyncApiRule]
    public static class AsyncApiExtensibleRules
    {
        /// <summary>
        /// Extension name MUST start with "x-".
        /// </summary>
        public static ValidationRule<IAsyncApiExtensible> ExtensionNameMustStartWithXDash =>
            new ValidationRule<IAsyncApiExtensible>(
                (context, item) =>
                {
                    context.Enter(AsyncApiConstants.Extensions);
                    foreach (var extensible in item.Extensions)
                    {
                        if (!extensible.Key.StartsWith("x-"))
                        {
                            context.CreateError(nameof(ExtensionNameMustStartWithXDash),
                                String.Format(SRResource.Validation_ExtensionNameMustBeginWithXDash, extensible.Key, context.PathString));
                        }
                    }
                    context.Exit();
                });
    }
}
