// Copyright (c) Microsoft Corporation. All rights reserved.
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
                    context.Enter("title");
                    if (item.Title == null)
                    {
                        context.CreateError(nameof(InfoRequiredFields),
                            String.Format(SRResource.Validation_FieldIsRequired, "title", "info"));
                    }
                    context.Exit();

                    // version
                    context.Enter("version");
                    if (item.Version == null)
                    {
                        context.CreateError(nameof(InfoRequiredFields),
                            String.Format(SRResource.Validation_FieldIsRequired, "version", "info"));
                    }
                    context.Exit();

                });

        // add more rule.
    }
}
