// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The validation rules for <see cref="AsyncApiParameter"/>.
    /// </summary>
    [AsyncApiRule]
    public static class AsyncApiParameterRules
    {
        /// <summary>
        /// Validate the field is required.
        /// </summary>
        public static ValidationRule<AsyncApiParameter> ParameterRequiredFields =>
            new ValidationRule<AsyncApiParameter>(
                (context, item) =>
                {
                    // name
                    context.Enter("name");
                    if (item.Name == null)
                    {
                        context.CreateError(nameof(ParameterRequiredFields),
                            String.Format(SRResource.Validation_FieldIsRequired, "name", "parameter"));
                    }
                    context.Exit();

                    // in
                    context.Enter("in");
                    if (item.In == null)
                    {
                        context.CreateError(nameof(ParameterRequiredFields),
                            String.Format(SRResource.Validation_FieldIsRequired, "in", "parameter"));
                    }
                    context.Exit();
                });

        /// <summary>
        /// Validate the "required" field is true when "in" is path.
        /// </summary>
        public static ValidationRule<AsyncApiParameter> RequiredMustBeTrueWhenInIsPath =>
            new ValidationRule<AsyncApiParameter>(
                (context, item) =>
                {
                    // required
                    context.Enter("required");
                    if (item.In == ParameterLocation.Path && !item.Required)
                    {
                        context.CreateError(
                            nameof(RequiredMustBeTrueWhenInIsPath),
                            "\"required\" must be true when parameter location is \"path\"");
                    }

                    context.Exit();
                });

        /// <summary>
        /// Validate the data matches with the given data type.
        /// </summary>
        public static ValidationRule<AsyncApiParameter> ParameterMismatchedDataType =>
            new ValidationRule<AsyncApiParameter>(
                (context, parameter) =>
                {
                    // example
                    context.Enter("example");

                    if (parameter.Example != null)
                    {
                        RuleHelpers.ValidateDataTypeMismatch(context, nameof(ParameterMismatchedDataType), parameter.Example, parameter.Schema);
                    }

                    context.Exit();

                    // examples
                    context.Enter("examples");

                    if (parameter.Examples != null)
                    {
                        foreach (var key in parameter.Examples.Keys)
                        {
                            if (parameter.Examples[key] != null)
                            {
                                context.Enter(key);
                                context.Enter("value");
                                RuleHelpers.ValidateDataTypeMismatch(context, nameof(ParameterMismatchedDataType), parameter.Examples[key]?.Value, parameter.Schema);
                                context.Exit();
                                context.Exit();
                            }
                        }
                    }

                    context.Exit();
                });

        /// <summary>
        /// Validate that a path parameter should always appear in the path 
        /// </summary>
        public static ValidationRule<AsyncApiParameter> PathParameterShouldBeInThePath =>
            new ValidationRule<AsyncApiParameter>(
                (context, parameter) =>
                {
                    if (parameter.In == ParameterLocation.Path && !context.PathString.Contains("{" + parameter.Name + "}"))
                    {
                        context.Enter("in");
                        context.CreateError(
                            nameof(PathParameterShouldBeInThePath),
                            $"Declared path parameter \"{parameter.Name}\" needs to be defined as a path parameter at either the path or operation level");
                        context.Exit();
                    }
                });
        // add more rule.
    }
}
