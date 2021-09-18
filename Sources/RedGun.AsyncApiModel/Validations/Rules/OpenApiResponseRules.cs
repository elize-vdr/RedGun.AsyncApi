﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The validation rules for <see cref="AsyncApiResponse"/>.
    /// </summary>
    [OpenApiRule]
    public static class OpenApiResponseRules
    {
        /// <summary>
        /// Validate the field is required.
        /// </summary>
        public static ValidationRule<AsyncApiResponse> ResponseRequiredFields =>
            new ValidationRule<AsyncApiResponse>(
                (context, response) =>
                {
                    // description
                    context.Enter("description");
                    if (response.Description == null)
                    {
                        context.CreateError(nameof(ResponseRequiredFields),
                            String.Format(SRResource.Validation_FieldIsRequired, "description", "response"));
                    }
                    context.Exit();
                });

        // add more rule.
    }
}
