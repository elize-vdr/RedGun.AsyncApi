// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The validation rules for <see cref="AsyncApiTag"/>.
    /// </summary>
    [AsyncApiRule]
    public static class AsyncApiTagRules
    {
        /// <summary>
        /// Validate the field is required.
        /// </summary>
        public static ValidationRule<AsyncApiTag> TagRequiredFields =>
            new ValidationRule<AsyncApiTag>(
                (context, tag) =>
                {
                    context.Enter(AsyncApiConstants.Name);
                    if (tag.Name == null)
                    {
                        context.CreateError(nameof(TagRequiredFields),
                            String.Format(SRResource.Validation_FieldIsRequired, AsyncApiConstants.Name, AsyncApiConstants.Tag));
                    }
                    context.Exit();
                });

        // add more rules
    }
}
