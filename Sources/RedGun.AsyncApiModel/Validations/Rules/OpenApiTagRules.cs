// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The validation rules for <see cref="AsyncApiTag"/>.
    /// </summary>
    [OpenApiRule]
    public static class OpenApiTagRules
    {
        /// <summary>
        /// Validate the field is required.
        /// </summary>
        public static ValidationRule<AsyncApiTag> TagRequiredFields =>
            new ValidationRule<AsyncApiTag>(
                (context, tag) =>
                {
                    context.Enter("name");
                    if (tag.Name == null)
                    {
                        context.CreateError(nameof(TagRequiredFields),
                            String.Format(SRResource.Validation_FieldIsRequired, "name", "tag"));
                    }
                    context.Exit();
                });

        // add more rules
    }
}
