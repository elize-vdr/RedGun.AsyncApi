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
    public class AsyncApiResponseValidationTests
    {
        [Fact]
        public void ValidateDescriptionIsRequiredInResponse()
        {
            // Arrange
            IEnumerable<AsyncApiError> errors;
            AsyncApiResponse response = new AsyncApiResponse();

            // Act
            var validator = new AsyncApiValidator(ValidationRuleSet.GetDefaultRuleSet());
            var walker = new AsyncApiWalker(validator);
            walker.Walk(response);

            errors = validator.Errors;
            bool result = !errors.Any();

            // Assert
            Assert.False(result);
            Assert.NotNull(errors);
            AsyncApiValidatorError error = Assert.Single(errors) as AsyncApiValidatorError;
            Assert.Equal(String.Format(SRResource.Validation_FieldIsRequired, "description", "response"), error.Message);
            Assert.Equal("#/description", error.Pointer);
        }
    }
}
