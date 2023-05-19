// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The validation rules for <see cref="AsyncApiOAuthFlow"/>.
    /// </summary>
    //[AsyncApiRule]
    public static class AsyncApiOAuthFlowRules
    {
        /// <summary>
        /// Validate the field is required.
        /// </summary>
        public static ValidationRule<AsyncApiOAuthFlow> OAuthFlowRequiredFields =>
            new ValidationRule<AsyncApiOAuthFlow>(
                (context, flow) =>
                {
                    // authorizationUrl
                    context.Enter(AsyncApiConstants.AuthorizationUrl);
                    if (flow.AuthorizationUrl == null)
                    {
                        context.CreateError(nameof(OAuthFlowRequiredFields),
                            String.Format(SRResource.Validation_FieldIsRequired, AsyncApiConstants.AuthorizationUrl, "OAuth Flow"));
                    }
                    context.Exit();

                    // tokenUrl
                    context.Enter(AsyncApiConstants.TokenUrl);
                    if (flow.TokenUrl == null)
                    {
                        context.CreateError(nameof(OAuthFlowRequiredFields),
                            String.Format(SRResource.Validation_FieldIsRequired, AsyncApiConstants.TokenUrl, "OAuth Flow"));
                    }
                    context.Exit();

                    // scopes
                    context.Enter(AsyncApiConstants.Scopes);
                    if (flow.Scopes == null)
                    {
                        context.CreateError(nameof(OAuthFlowRequiredFields),
                            String.Format(SRResource.Validation_FieldIsRequired, AsyncApiConstants.Scopes, "OAuth Flow"));
                    }
                    context.Exit();
                });

        // add more rule.
    }
}
