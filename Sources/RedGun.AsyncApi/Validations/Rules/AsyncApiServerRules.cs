﻿// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The validation rules for <see cref="AsyncApiServer"/>.
    /// </summary>
    [AsyncApiRule]
    public static class AsyncApiServerRules
    {
        /// <summary>
        /// Validate the field is required.
        /// </summary>
        public static ValidationRule<AsyncApiServer> ServerRequiredFields =>
            new ValidationRule<AsyncApiServer>(
                (context, server) =>
                {
                    context.Enter(AsyncApiConstants.Url);
                    if (server.Url == null)
                    {
                        context.CreateError(nameof(ServerRequiredFields),
                            String.Format(SRResource.Validation_FieldIsRequired, AsyncApiConstants.Url, AsyncApiConstants.Server));
                    }
                    context.Exit();
                });

        // add more rules
    }
}
