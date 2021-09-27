// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Linq;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;
using RedGun.AsyncApi.Validations;
using Xunit;

namespace RedGun.AsyncApi.Tests.Validations
{
    public class AsyncApiInfoValidationTests
    {
        [Fact]
        public void ValidateFieldIsRequiredInInfo()
        {
            // Arrange
            string titleError = String.Format(SRResource.Validation_FieldIsRequired, "title", "info");
            string versionError = String.Format(SRResource.Validation_FieldIsRequired, "version", "info");
            var info = new AsyncApiInfo();

            // Act
            var errors = info.Validate(ValidationRuleSet.GetDefaultRuleSet());

            // Assert
            bool result = !errors.Any();

            // Assert
            Assert.False(result);
            Assert.NotNull(errors);

            Assert.Equal(new[] { titleError, versionError }, errors.Select(e => e.Message));
        }
    }
}
