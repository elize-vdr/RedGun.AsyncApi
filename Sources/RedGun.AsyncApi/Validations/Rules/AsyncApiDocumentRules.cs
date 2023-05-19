// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
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
                    context.Enter(AsyncApiConstants.Info);
                    if (item.Info == null)
                    {
                        context.CreateError(nameof(AsyncApiDocumentFieldIsMissing),
                            string.Format(SRResource.Validation_FieldIsRequired, AsyncApiConstants.Info, AsyncApiConstants.Document));
                    }
                    context.Exit();

                    // channels
                    context.Enter(AsyncApiConstants.Channels);
                    if (item.Channels == null)
                    {
                        context.CreateError(nameof(AsyncApiDocumentFieldIsMissing),
                            string.Format(SRResource.Validation_FieldIsRequired, AsyncApiConstants.Channels, AsyncApiConstants.Document));
                    }
                    context.Exit();
                });
    }
}
