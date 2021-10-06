﻿// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Services;
using RedGun.AsyncApi.Validations;
using RedGun.AsyncApi.Validations.Rules;
using Xunit;

namespace RedGun.AsyncApi.Tests.Validations
{
    public class AsyncApiHeaderValidationTests
    {
        [Fact]
        public void ValidateExampleShouldNotHaveDataTypeMismatchForSimpleSchema()
        {
            // Arrange
            IEnumerable<AsyncApiError> errors;
            var header = new AsyncApiHeader()
            {
                Required = true,
                Example = new AsyncApiInteger(55),
                Schema = new AsyncApiSchema()
                {
                    Type = "string",
                }
            };

            // Act
            var validator = new AsyncApiValidator(ValidationRuleSet.GetDefaultRuleSet());
            var walker = new AsyncApiWalker(validator);
            walker.Walk(header);

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
                "#/example",
            });
        }

        [Fact]
        public void ValidateExamplesShouldNotHaveDataTypeMismatchForSimpleSchema()
        {
            // Arrange
            IEnumerable<AsyncApiError> errors;

            var header = new AsyncApiHeader()
            {
                Required = true,
                Schema = new AsyncApiSchema()
                {
                    Type = "object",
                    AdditionalProperties = new AsyncApiSchema()
                    {
                        Type = "integer",
                    }
                },
                Examples =
                    {
                        ["example0"] = new AsyncApiExample()
                        {
                            Value = new AsyncApiString("1"),
                        },
                        ["example1"] = new AsyncApiExample()
                        {
                           Value = new AsyncApiObject()
                            {
                                ["x"] = new AsyncApiInteger(2),
                                ["y"] = new AsyncApiString("20"),
                                ["z"] = new AsyncApiString("200")
                            }
                        },
                        ["example2"] = new AsyncApiExample()
                        {
                            Value =
                            new AsyncApiArray()
                            {
                                new AsyncApiInteger(3)
                            }
                        },
                        ["example3"] = new AsyncApiExample()
                        {
                            Value = new AsyncApiObject()
                            {
                                ["x"] = new AsyncApiInteger(4),
                                ["y"] = new AsyncApiInteger(40),
                            }
                        },
                    }
            };

            // Act
            var validator = new AsyncApiValidator(ValidationRuleSet.GetDefaultRuleSet());
            var walker = new AsyncApiWalker(validator);
            walker.Walk(header);

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
                "#/examples/example1/value/y",
                "#/examples/example1/value/z",
                "#/examples/example2/value"
            });
        }
    }
}
