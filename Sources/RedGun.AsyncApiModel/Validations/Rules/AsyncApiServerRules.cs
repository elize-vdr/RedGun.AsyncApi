// Copyright (c) Microsoft Corporation. All rights reserved.
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
                    context.Enter("url");
                    if (server.Url == null)
                    {
                        context.CreateError(nameof(ServerRequiredFields),
                            String.Format(SRResource.Validation_FieldIsRequired, "url", "server"));
                    }
                    context.Exit();
                });

        // add more rules
    }
}
