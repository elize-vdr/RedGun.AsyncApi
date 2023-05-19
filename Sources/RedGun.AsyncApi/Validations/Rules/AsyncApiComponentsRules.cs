// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using System.Text.RegularExpressions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The validation rules for <see cref="AsyncApiComponents"/>.
    /// </summary>
    //[AsyncApiRule]
    public static class AsyncApiComponentsRules
    {
        /// <summary>
        /// The key regex.
        /// </summary>
        public static Regex KeyRegex = new Regex(@"^[a-zA-Z0-9\.\-_]+$");

        /// <summary>
        /// All the fixed fields declared above are objects
        /// that MUST use keys that match the regular expression: ^[a-zA-Z0-9\.\-_]+$.
        /// </summary>
        public static ValidationRule<AsyncApiComponents> KeyMustBeRegularExpression =>
            new ValidationRule<AsyncApiComponents>(
                (context, components) =>
                {
                    ValidateKeys(context, components.Schemas?.Keys, AsyncApiConstants.Schemas);

                    ValidateKeys(context, components.Messages?.Keys, AsyncApiConstants.Messages);

                    ValidateKeys(context, components.SecuritySchemes?.Keys, AsyncApiConstants.SecuritySchemes);

                    ValidateKeys(context, components.Parameters?.Keys, AsyncApiConstants.Parameters);

                    ValidateKeys(context, components.CorrelationIds?.Keys, AsyncApiConstants.CorrelationIds);

                    ValidateKeys(context, components.OperationTraits?.Keys, AsyncApiConstants.OperationTraits);

                    ValidateKeys(context, components.MessageTraits?.Keys, AsyncApiConstants.MessageTraits);

                    ValidateKeys(context, components.ServerBindings?.Keys, AsyncApiConstants.ServerBindings);

                    ValidateKeys(context, components.ChannelBindings?.Keys, AsyncApiConstants.ChannelBindings);
                    
                    ValidateKeys(context, components.OperationBindings?.Keys, AsyncApiConstants.OperationBindings);
                    
                    ValidateKeys(context, components.MessageBindings?.Keys, AsyncApiConstants.MessageBindings);

                });

        private static void ValidateKeys(IValidationContext context, IEnumerable<string> keys, string component)
        {
            if (keys == null)
            {
                return;
            }

            foreach (var key in keys)
            {
                if (!KeyRegex.IsMatch(key))
                {
                    context.CreateError(nameof(KeyMustBeRegularExpression),
                        string.Format(SRResource.Validation_ComponentsKeyMustMatchRegularExpr, key, component, KeyRegex.ToString()));
                }
            }
        }
    }
}
