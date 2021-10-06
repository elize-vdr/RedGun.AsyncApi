// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Linq;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;
using RedGun.AsyncApi.Validations;
using RedGun.AsyncApi.Validations.Rules;
using Xunit;

namespace RedGun.AsyncApi.Tests.Validations
{
    public class AsyncApiComponentsValidationTests
    {
        [Fact]
        public void ValidateKeyMustMatchRegularExpressionInComponents()
        {
            // Arrange
            const string key = "%@abc";

            AsyncApiComponents components = new AsyncApiComponents()
            {
                Responses = new Dictionary<string, AsyncApiResponse>
                {
                    { key, new AsyncApiResponse { Description = "any" } }
                }
            };

            var errors = components.Validate(ValidationRuleSet.GetDefaultRuleSet());

            // Act
            bool result = !errors.Any();


            // Assert
            Assert.False(result);
            Assert.NotNull(errors);
            AsyncApiError error = Assert.Single(errors);
            Assert.Equal(String.Format(SRResource.Validation_ComponentsKeyMustMatchRegularExpr, key, "responses", AsyncApiComponentsRules.KeyRegex.ToString()),
                error.Message);
        }
    }
}
