// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;
using RedGun.AsyncApi.Services;
using RedGun.AsyncApi.Validations;
using RedGun.AsyncApi.Validations.Rules;
using Xunit;

namespace RedGun.AsyncApi.Tests.Validations
{
    [Collection("DefaultSettings")]
    public class AsyncApiSchemaValidationTests
    {
        [Fact]
        public void ValidateDefaultShouldNotHaveDataTypeMismatchForSimpleSchema()
        {
            // Arrange
            IEnumerable<AsyncApiError> errors;
            var schema = new AsyncApiSchema()
            {
                Default = new AsyncApiInteger(55),
                Type = "string",
            };

            // Act
            var validator = new AsyncApiValidator(ValidationRuleSet.GetDefaultRuleSet());
            var walker = new AsyncApiWalker(validator);
            walker.Walk(schema);

            errors = validator.Errors;
            bool result = !errors.Any();

            // Assert
            result.Should().BeFalse();
            errors.Select(e => e.Message).Should().BeEquivalentTo(new[]
            {
                RuleHelpers.DataTypeMismatchedErrorMessage
            });
            errors.Select(e => e.Pointer).Should().BeEquivalentTo(new[]
            {
                "#/default",
            });
        }

        [Fact]
        public void ValidateExampleAndDefaultShouldNotHaveDataTypeMismatchForSimpleSchema()
        {
            // Arrange
            IEnumerable<AsyncApiError> errors;
            var schema = new AsyncApiSchema()
            {
                Example = new AsyncApiLong(55),
                Default = new AsyncApiPassword("1234"),
                Type = "string",
            };

            // Act
            var validator = new AsyncApiValidator(ValidationRuleSet.GetDefaultRuleSet());
            var walker = new AsyncApiWalker(validator);
            walker.Walk(schema);

            errors = validator.Errors;
            bool result = !errors.Any();

            // Assert
            result.Should().BeFalse();
            errors.Select(e => e.Message).Should().BeEquivalentTo(new[]
            {
                RuleHelpers.DataTypeMismatchedErrorMessage,
                RuleHelpers.DataTypeMismatchedErrorMessage
            });
            errors.Select(e => e.Pointer).Should().BeEquivalentTo(new[]
            {
                "#/default",
                "#/example",
            });
        }

        [Fact]
        public void ValidateEnumShouldNotHaveDataTypeMismatchForSimpleSchema()
        {
            // Arrange
            IEnumerable<AsyncApiError> errors;
            var schema = new AsyncApiSchema()
            {
                Enum =
                {
                    new AsyncApiString("1"),
                    new AsyncApiObject()
                    {
                        ["x"] = new AsyncApiInteger(2),
                        ["y"] = new AsyncApiString("20"),
                        ["z"] = new AsyncApiString("200")
                    },
                    new AsyncApiArray()
                    {
                        new AsyncApiInteger(3)
                    },
                    new AsyncApiObject()
                    {
                        ["x"] = new AsyncApiInteger(4),
                        ["y"] = new AsyncApiInteger(40),
                    },
                },
                Type = "object",
                AdditionalProperties = new AsyncApiSchema()
                {
                    Type = "integer",
                }
            };

            // Act
            var validator = new AsyncApiValidator(ValidationRuleSet.GetDefaultRuleSet());
            var walker = new AsyncApiWalker(validator);
            walker.Walk(schema);

            errors = validator.Errors;
            bool result = !errors.Any();

            // Assert
            result.Should().BeFalse();
            errors.Select(e => e.Message).Should().BeEquivalentTo(new[]
            {
                RuleHelpers.DataTypeMismatchedErrorMessage,
                RuleHelpers.DataTypeMismatchedErrorMessage,
                RuleHelpers.DataTypeMismatchedErrorMessage,
            });
            errors.Select(e => e.Pointer).Should().BeEquivalentTo(new[]
            {
                // #enum/0 is not an error since the spec allows
                // representing an object using a string.
                "#/enum/1/y",
                "#/enum/1/z",
                "#/enum/2"
            });
        }

        [Fact]
        public void ValidateDefaultShouldNotHaveDataTypeMismatchForComplexSchema()
        {
            // Arrange
            IEnumerable<AsyncApiError> errors;
            var schema = new AsyncApiSchema()
            {
                Type = "object",
                Properties =
                {
                    ["property1"] = new AsyncApiSchema()
                    {
                        Type = "array",
                        Items = new AsyncApiSchema()
                        {
                            Type = "integer",
                            Format = "int64"
                        }
                    },
                    ["property2"] = new AsyncApiSchema()
                    {
                        Type = "array",
                        Items = new AsyncApiSchema()
                        {
                            Type = "object",
                            AdditionalProperties = new AsyncApiSchema()
                            {
                                Type = "boolean"
                            }
                        }
                    },
                    ["property3"] = new AsyncApiSchema()
                    {
                        Type = "string",
                        Format = "password"
                    },
                    ["property4"] = new AsyncApiSchema()
                    {
                        Type = "string"
                    }
                },
                Default = new AsyncApiObject()
                {
                    ["property1"] = new AsyncApiArray()
                    {
                        new AsyncApiInteger(12),
                        new AsyncApiLong(13),
                        new AsyncApiString("1"),
                    },
                    ["property2"] = new AsyncApiArray()
                    {
                        new AsyncApiInteger(2),
                        new AsyncApiObject()
                        {
                            ["x"] = new AsyncApiBoolean(true),
                            ["y"] = new AsyncApiBoolean(false),
                            ["z"] = new AsyncApiString("1234"),
                        }
                    },
                    ["property3"] = new AsyncApiPassword("123"),
                    ["property4"] = new AsyncApiDateTime(DateTime.UtcNow)
                }
            };

            // Act
            var validator = new AsyncApiValidator(ValidationRuleSet.GetDefaultRuleSet());
            var walker = new AsyncApiWalker(validator);
            walker.Walk(schema);

            errors = validator.Errors;
            bool result = !errors.Any();

            // Assert
            result.Should().BeFalse();
            errors.Select(e => e.Message).Should().BeEquivalentTo(new[]
            {
                RuleHelpers.DataTypeMismatchedErrorMessage,
                RuleHelpers.DataTypeMismatchedErrorMessage,
                RuleHelpers.DataTypeMismatchedErrorMessage,
                RuleHelpers.DataTypeMismatchedErrorMessage,
                RuleHelpers.DataTypeMismatchedErrorMessage,
            });
            errors.Select(e => e.Pointer).Should().BeEquivalentTo(new[]
            {
                "#/default/property1/0",
                "#/default/property1/2",
                "#/default/property2/0",
                "#/default/property2/1/z",
                "#/default/property4",
            });
        }

        [Fact]
        public void ValidateSchemaRequiredFieldListMustContainThePropertySpecifiedInTheDiscriminator()
        {
            IEnumerable<AsyncApiError> errors;
            var components = new AsyncApiComponents
            {
                Schemas = {
                    {
                        "schema1",
                        new AsyncApiSchema
                        {
                            Type = "object",
                            Discriminator = new AsyncApiDiscriminator { PropertyName = "property1" },
                            Reference = new AsyncApiReference { Id = "schema1" }
                        }
                    }
                }
            };
            // Act
            var validator = new AsyncApiValidator(ValidationRuleSet.GetDefaultRuleSet());
            var walker = new AsyncApiWalker(validator);
            walker.Walk(components);

            errors = validator.Errors;
            bool result = !errors.Any();

            // Assert
            result.Should().BeFalse();
            errors.Should().BeEquivalentTo(new List<AsyncApiValidatorError>
            {
                    new AsyncApiValidatorError(nameof(AsyncApiSchemaRules.ValidateSchemaDiscriminator),"#/schemas/schema1/discriminator",
                        string.Format(SRResource.Validation_SchemaRequiredFieldListMustContainThePropertySpecifiedInTheDiscriminator,
                                    "schema1", "property1"))
            });
        }
    }
}
