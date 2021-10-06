// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Linq;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;
using RedGun.AsyncApi.Validations;
using Xunit;

namespace RedGun.AsyncApi.Tests.Validations
{
    public class AsyncApiTagValidationTests
    {
        [Fact]
        public void ValidateNameIsRequiredInTag()
        {
            // Arrange
            IEnumerable<AsyncApiError> errors;
            AsyncApiTag tag = new AsyncApiTag();

            // Act
            var validator = new AsyncApiValidator(ValidationRuleSet.GetDefaultRuleSet());
            validator.Visit(tag);
            errors = validator.Errors;
            bool result = !errors.Any();

            // Assert
            Assert.False(result);
            Assert.NotNull(errors);
            AsyncApiError error = Assert.Single(errors);
            Assert.Equal(String.Format(SRResource.Validation_FieldIsRequired, "name", "tag"), error.Message);
        }

        [Fact]
        public void ValidateExtensionNameStartsWithXDashInTag()
        {
            // Arrange
            IEnumerable<AsyncApiError> errors;
            AsyncApiTag tag = new AsyncApiTag
            {
                Name = "tag"
            };
            tag.Extensions.Add("tagExt", new AsyncApiString("value"));

            // Act
            var validator = new AsyncApiValidator(ValidationRuleSet.GetDefaultRuleSet());
            validator.Visit(tag as IAsyncApiExtensible);
            errors = validator.Errors;
            bool result = !errors.Any();

            // Assert
            Assert.False(result);
            Assert.NotNull(errors);
            AsyncApiError error = Assert.Single(errors);
            Assert.Equal(String.Format(SRResource.Validation_ExtensionNameMustBeginWithXDash, "tagExt", "#/extensions"), error.Message);
        }
    }
}
