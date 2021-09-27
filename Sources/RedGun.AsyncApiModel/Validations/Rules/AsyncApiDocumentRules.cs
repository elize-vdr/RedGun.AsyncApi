// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The validation rules for <see cref="AsyncApiDocument"/>.
    /// </summary>
    [AsyncApiRule]
    public static class AsyncApiDocumentRules
    {
        /// <summary>
        /// The Info field is required.
        /// </summary>
        public static ValidationRule<AsyncApiDocument> AsyncApiDocumentFieldIsMissing =>
            new ValidationRule<AsyncApiDocument>(
                (context, item) =>
                {
                    // info
                    context.Enter("info");
                    if (item.Info == null)
                    {
                        context.CreateError(nameof(AsyncApiDocumentFieldIsMissing),
                            String.Format(SRResource.Validation_FieldIsRequired, "info", "document"));
                    }
                    context.Exit();

                    // paths
                    context.Enter("paths");
                    if (item.Paths == null)
                    {
                        context.CreateError(nameof(AsyncApiDocumentFieldIsMissing),
                            String.Format(SRResource.Validation_FieldIsRequired, "paths", "document"));
                    }
                    context.Exit();
                });
    }
}
