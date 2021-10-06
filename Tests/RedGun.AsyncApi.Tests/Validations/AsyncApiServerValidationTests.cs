// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Linq;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;
using RedGun.AsyncApi.Validations;
using Xunit;

namespace RedGun.AsyncApi.Tests.Validations
{
    public class AsyncApiServerValidationTests
    {
        [Fact]
        public void ValidateFieldIsRequiredInServer()
        {
            // Arrange
            IEnumerable<AsyncApiError> errors;
            AsyncApiServer server = new AsyncApiServer();

            // Act
            var validator = new AsyncApiValidator(ValidationRuleSet.GetDefaultRuleSet());
            validator.Visit(server);
            errors = validator.Errors;
            bool result = !errors.Any();

            // Assert
            Assert.False(result);
            Assert.NotNull(errors);
            AsyncApiError error = Assert.Single(errors);
            Assert.Equal(String.Format(SRResource.Validation_FieldIsRequired, "url", "server"), error.Message);
        }
    }
}
