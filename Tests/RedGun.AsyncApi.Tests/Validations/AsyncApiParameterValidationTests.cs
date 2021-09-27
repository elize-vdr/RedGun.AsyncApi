// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;
using RedGun.AsyncApi.Services;
using RedGun.AsyncApi.Validations;
using RedGun.AsyncApi.Validations.Rules;
using Xunit;

namespace RedGun.AsyncApi.Tests.Validations
{
    public class AsyncApiParameterValidationTests
    {
        [Fact]
        public void ValidateFieldIsRequiredInParameter()
        {
            // Arrange
            string nameError = String.Format(SRResource.Validation_FieldIsRequired, "name", "parameter");
            string inError = String.Format(SRResource.Validation_FieldIsRequired, "in", "parameter");
            var parameter = new AsyncApiParameter();

            // Act
            var errors = parameter.Validate(ValidationRuleSet.GetDefaultRuleSet());

            // Assert
            errors.Should().NotBeEmpty();
            errors.Select(e => e.Message).Should().BeEquivalentTo(new[]
            {
                nameError,
                inError
            });
        }

        [Fact]
        public void ValidateRequiredIsTrueWhenInIsPathInParameter()
        {
            // Arrange
            var parameter = new AsyncApiParameter()
            {
                Name = "name",
                In = ParameterLocation.Path
            };

            // Act
            var validator = new AsyncApiValidator(ValidationRuleSet.GetDefaultRuleSet());
            validator.Enter("{name}");
            var walker = new AsyncApiWalker(validator);
            walker.Walk(parameter);
            var errors = validator.Errors;
            // Assert
            errors.Should().NotBeEmpty();
            errors.Select(e => e.Message).Should().BeEquivalentTo(new[]
            {
                "\"required\" must be true when parameter location is \"path\""
            });
        }

        [Fact]
        public void ValidateExampleShouldNotHaveDataTypeMismatchForSimpleSchema()
        {
            // Arrange
            IEnumerable<AsyncApiError> errors;
            var parameter = new AsyncApiParameter()
            {
                Name = "parameter1",
                In = ParameterLocation.Path,
                Required = true,
                Example = new AsyncApiInteger(55),
                Schema = new AsyncApiSchema()
                {
                    Type = "string",
                }
            };

            // Act
            var validator = new AsyncApiValidator(ValidationRuleSet.GetDefaultRuleSet());
            validator.Enter("{parameter1}");
            var walker = new AsyncApiWalker(validator);
            walker.Walk(parameter);

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
                "#/{parameter1}/example",
            });
        }

        [Fact]
        public void ValidateExamplesShouldNotHaveDataTypeMismatchForSimpleSchema()
        {
            // Arrange
            IEnumerable<AsyncApiError> errors;

            var parameter = new AsyncApiParameter()
            {
                Name = "parameter1",
                In = ParameterLocation.Path,
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
            validator.Enter("{parameter1}");
            var walker = new AsyncApiWalker(validator);
            walker.Walk(parameter);

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
                "#/{parameter1}/examples/example1/value/y",
                "#/{parameter1}/examples/example1/value/z",
                "#/{parameter1}/examples/example2/value"
            });
        }

        [Fact]
        public void PathParameterNotInThePathShouldReturnAnError()
        {
            // Arrange
            IEnumerable<AsyncApiError> errors;

            var parameter = new AsyncApiParameter()
            {
                Name = "parameter1",
                In = ParameterLocation.Path,
                Required = true,
                Schema = new AsyncApiSchema()
                {
                    Type = "string",
                }
            };

            // Act
            var validator = new AsyncApiValidator(ValidationRuleSet.GetDefaultRuleSet());

            var walker = new AsyncApiWalker(validator);
            walker.Walk(parameter);

            errors = validator.Errors;
            bool result = errors.Any();

            // Assert
            result.Should().BeTrue();
            errors.OfType<AsyncApiValidatorError>().Select(e => e.RuleName).Should().BeEquivalentTo(new[]
            {
                "PathParameterShouldBeInThePath"
            });
            errors.Select(e => e.Pointer).Should().BeEquivalentTo(new[]
            {
                "#/in"
            });
        }

        [Fact]
        public void PathParameterInThePastShouldBeOk()
        {
            // Arrange
            IEnumerable<AsyncApiError> errors;

            var parameter = new AsyncApiParameter()
            {
                Name = "parameter1",
                In = ParameterLocation.Path,
                Required = true,
                Schema = new AsyncApiSchema()
                {
                    Type = "string",
                }
            };

            // Act
            var validator = new AsyncApiValidator(ValidationRuleSet.GetDefaultRuleSet());
            validator.Enter("paths");
            validator.Enter("/{parameter1}");
            validator.Enter("get");
            validator.Enter("parameters");
            validator.Enter("1");

            var walker = new AsyncApiWalker(validator);
            walker.Walk(parameter);

            errors = validator.Errors;
            bool result = errors.Any();

            // Assert
            result.Should().BeFalse();
        }
    }
}
