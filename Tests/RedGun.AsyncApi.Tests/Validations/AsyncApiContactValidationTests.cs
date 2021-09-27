// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Linq;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;
using RedGun.AsyncApi.Validations;
using Xunit;

namespace RedGun.AsyncApi.Tests.Validations
{
    public class AsyncApiContactValidationTests
    {
        [Fact]
        public void ValidateEmailFieldIsEmailAddressInContact()
        {
            // Arrange
            const string testEmail = "support/example.com";

            AsyncApiContact contact = new AsyncApiContact()
            {
                Email = testEmail
            };

            // Act
            var errors = contact.Validate(ValidationRuleSet.GetDefaultRuleSet());
            bool result = !errors.Any();

            // Assert
            Assert.False(result);
            Assert.NotNull(errors);
            AsyncApiError error = Assert.Single(errors);
            Assert.Equal(String.Format(SRResource.Validation_StringMustBeEmailAddress, testEmail), error.Message);
        }
    }
}
