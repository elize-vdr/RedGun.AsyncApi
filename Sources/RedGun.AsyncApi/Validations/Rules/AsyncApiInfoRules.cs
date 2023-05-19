// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The validation rules for <see cref="AsyncApiInfo"/>.
    /// </summary>
    [AsyncApiRule]
    public static class AsyncApiInfoRules
    {
        /// <summary>
        /// Validate the field is required.
        /// </summary>
        public static ValidationRule<AsyncApiInfo> InfoRequiredFields =>
            new ValidationRule<AsyncApiInfo>(
                (context, item) =>
                {

                    // title
                    context.Enter(AsyncApiConstants.Title);
                    if (item.Title == null)
                    {
                        context.CreateError(nameof(InfoRequiredFields),
                            String.Format(SRResource.Validation_FieldIsRequired, AsyncApiConstants.Title, AsyncApiConstants.Info));
                    }
                    context.Exit();

                    // version
                    context.Enter(AsyncApiConstants.Version);
                    if (item.Version == null)
                    {
                        context.CreateError(nameof(InfoRequiredFields),
                            String.Format(SRResource.Validation_FieldIsRequired, AsyncApiConstants.Version, AsyncApiConstants.Info));
                    }
                    context.Exit();

                });

        // add more rule.
    }
}
