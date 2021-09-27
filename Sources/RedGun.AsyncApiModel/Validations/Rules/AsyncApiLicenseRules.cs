// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The validation rules for <see cref="AsyncApiLicense"/>.
    /// </summary>
    [AsyncApiRule]
    public static class AsyncApiLicenseRules
    {
        /// <summary>
        /// REQUIRED.
        /// </summary>
        public static ValidationRule<AsyncApiLicense> LicenseRequiredFields =>
            new ValidationRule<AsyncApiLicense>(
                (context, license) =>
                {
                    context.Enter("name");
                    if (license.Name == null)
                    {
                        context.CreateError(nameof(LicenseRequiredFields),
                            String.Format(SRResource.Validation_FieldIsRequired, "name", "license"));
                    }
                    context.Exit();
                });

        // add more rules
    }
}
