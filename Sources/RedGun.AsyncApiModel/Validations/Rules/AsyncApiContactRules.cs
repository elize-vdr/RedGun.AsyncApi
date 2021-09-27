// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The validation rules for <see cref="AsyncApiContact"/>.
    /// </summary>
    [AsyncApiRule]
    public static class AsyncApiContactRules
    {
        /// <summary>
        /// Email field MUST be email address.
        /// </summary>
        public static ValidationRule<AsyncApiContact> EmailMustBeEmailFormat =>
            new ValidationRule<AsyncApiContact>(
                (context, item) =>
                {

                    context.Enter("email");
                    if (item != null && item.Email != null)
                    {
                        if (!item.Email.IsEmailAddress())
                        {
                            context.CreateError(nameof(EmailMustBeEmailFormat),
                                String.Format(SRResource.Validation_StringMustBeEmailAddress, item.Email));
                        }
                    }
                    context.Exit();
                });

    }
}
