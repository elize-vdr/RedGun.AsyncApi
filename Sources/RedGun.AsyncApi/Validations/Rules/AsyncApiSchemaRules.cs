// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;
using System.Collections.Generic;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The validation rules for <see cref="AsyncApiSchema"/>.
    /// </summary>
    [AsyncApiRule]
    public static class AsyncApiSchemaRules
    {
        /// <summary>
        /// Validate the data matches with the given data type.
        /// </summary>
        public static ValidationRule<AsyncApiSchema> SchemaMismatchedDataType =>
            new ValidationRule<AsyncApiSchema>(
                (context, schema) =>
                {
                    // default
                    context.Enter(AsyncApiConstants.Default);

                    if (schema.Default != null)
                    {
                        RuleHelpers.ValidateDataTypeMismatch(context, nameof(SchemaMismatchedDataType), schema.Default, schema);
                    }

                    context.Exit();

                    // example
                    context.Enter(AsyncApiConstants.Example);

                    if (schema.Example != null)
                    {
                        RuleHelpers.ValidateDataTypeMismatch(context, nameof(SchemaMismatchedDataType), schema.Example, schema);
                    }

                    context.Exit();

                    // enum
                    context.Enter(AsyncApiConstants.Enum);

                    if (schema.Enum != null)
                    {
                        for (int i = 0; i < schema.Enum.Count; i++)
                        {
                            context.Enter(i.ToString());
                            RuleHelpers.ValidateDataTypeMismatch(context, nameof(SchemaMismatchedDataType), schema.Enum[i], schema);
                            context.Exit();
                        }
                    }

                    context.Exit();
                });

        /// <summary>
        /// Validates Schema Discriminator
        /// </summary>
        public static ValidationRule<AsyncApiSchema> ValidateSchemaDiscriminator =>
            new ValidationRule<AsyncApiSchema>(
                (context, schema) =>
                {
                    // discriminator
                    context.Enter(AsyncApiConstants.Discriminator);

                    if (schema.Reference != null && schema.Discriminator != null)
                    {
                        if (!schema.Required.Contains(schema.Discriminator?.PropertyName))
                        {
                            context.CreateError(nameof(ValidateSchemaDiscriminator),
                                                string.Format(SRResource.Validation_SchemaRequiredFieldListMustContainThePropertySpecifiedInTheDiscriminator,
                                                                                schema.Reference.Id, schema.Discriminator.PropertyName));
                        }
                    }

                    context.Exit();
                });
    }
}
