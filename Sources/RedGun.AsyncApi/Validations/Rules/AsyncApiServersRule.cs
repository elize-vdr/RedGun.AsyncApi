// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;
using System.Text.RegularExpressions;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The validation rules for <see cref="AsyncApiServers"/>.
    /// </summary>
    [AsyncApiRule]
    public static class AsyncApiServersRules
    {

        /// <summary>
        /// A relative path to an individual endpoint. The field name MUST begin with a slash.
        /// </summary>
        public static ValidationRule<AsyncApiServers> ServerNameMustMatchPattern =>
            new ValidationRule<AsyncApiServers>(
                (context, item) =>
                {
                    const string pattern = @"^[A-Za-z0-9_\-]+$";
                    foreach (var serverName in item.Keys)
                    {
                        context.Enter(serverName);

                        if (serverName == null || !Regex.IsMatch(serverName, pattern))
                        {
                            context.CreateError(nameof(ServerNameMustMatchPattern),
                                string.Format(SRResource.Validation_ServerNameMustMatchPattern, serverName, pattern));
                        }
                        
                        context.Exit();
                    }
                });

        // add more rules
    }
}
