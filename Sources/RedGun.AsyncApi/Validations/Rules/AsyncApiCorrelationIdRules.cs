// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The validation rules for <see cref="AsyncApiCorrelationId"/>.
    /// </summary>
    [AsyncApiRule]
    public static class AsyncApiCorrelationIdRules
    {
        /// <summary>
        /// Validate the field is required.
        /// </summary>
        public static ValidationRule<AsyncApiCorrelationId> CorrelationIdRequiredFields =>
            new ValidationRule<AsyncApiCorrelationId>(
                (context, item) =>
                {
                    // location
                    context.Enter(AsyncApiConstants.Location);
                    if (item.Location == null)
                    {
                        context.CreateError(nameof(CorrelationIdRequiredFields),
                            string.Format(SRResource.Validation_FieldIsRequired, AsyncApiConstants.Location, AsyncApiConstants.CorrelationId));
                    }
                    context.Exit();
                });

    }
}
