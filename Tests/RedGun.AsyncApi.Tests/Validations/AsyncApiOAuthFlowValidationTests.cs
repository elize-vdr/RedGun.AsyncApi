// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Linq;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;
using RedGun.AsyncApi.Services;
using RedGun.AsyncApi.Validations;
using Xunit;

namespace RedGun.AsyncApi.Tests.Validations
{
    public class AsyncApiOAuthFlowValidationTests
    {
        [Fact]
        public void ValidateFixedFieldsIsRequiredInResponse()
        {
            // Arrange
            string authorizationUrlError = String.Format(SRResource.Validation_FieldIsRequired, "authorizationUrl", "OAuth Flow");
            string tokenUrlError = String.Format(SRResource.Validation_FieldIsRequired, "tokenUrl", "OAuth Flow");
            IEnumerable<AsyncApiError> errors;
            AsyncApiOAuthFlow oAuthFlow = new AsyncApiOAuthFlow();

            // Act
            var validator = new AsyncApiValidator(ValidationRuleSet.GetDefaultRuleSet());
            var walker = new AsyncApiWalker(validator);
            walker.Walk(oAuthFlow);

            errors = validator.Errors;
            bool result = !errors.Any();

            // Assert
            Assert.False(result);
            Assert.NotNull(errors);
            Assert.Equal(2, errors.Count());
            Assert.Equal(new[] { authorizationUrlError, tokenUrlError }, errors.Select(e => e.Message));
        }
    }
}
