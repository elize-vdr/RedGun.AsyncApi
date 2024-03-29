﻿// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The validation rules for <see cref="AsyncApiExternalDocs"/>.
    /// </summary>
    [AsyncApiRule]
    public static class AsyncApiExternalDocsRules
    {
        /// <summary>
        /// Validate the field is required.
        /// </summary>
        public static ValidationRule<AsyncApiExternalDocs> UrlIsRequired =>
            new ValidationRule<AsyncApiExternalDocs>(
                (context, item) =>
                {
                    // url
                    context.Enter(AsyncApiConstants.Url);
                    if (item.Url == null)
                    {
                        context.CreateError(nameof(UrlIsRequired),
                            String.Format(SRResource.Validation_FieldIsRequired, AsyncApiConstants.Url, "External Documentation"));
                    }
                    context.Exit();
                });

        // add more rule.
    }
}
