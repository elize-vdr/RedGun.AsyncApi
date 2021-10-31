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
                    context.Enter("authorizationUrl");
                    if (flow.AuthorizationUrl == null)
                    {
                        context.CreateError(nameof(OAuthFlowRequiredFields),
                            String.Format(SRResource.Validation_FieldIsRequired, "authorizationUrl", "OAuth Flow"));
                    }
                    context.Exit();

                    // tokenUrl
                    context.Enter("tokenUrl");
                    if (flow.TokenUrl == null)
                    {
                        context.CreateError(nameof(OAuthFlowRequiredFields),
                            String.Format(SRResource.Validation_FieldIsRequired, "tokenUrl", "OAuth Flow"));
                    }
                    context.Exit();

                    // scopes
                    context.Enter("scopes");
                    if (flow.Scopes == null)
                    {
                        context.CreateError(nameof(OAuthFlowRequiredFields),
                            String.Format(SRResource.Validation_FieldIsRequired, "scopes", "OAuth Flow"));
                    }
                    context.Exit();
                });

        // add more rule.
    }
}
