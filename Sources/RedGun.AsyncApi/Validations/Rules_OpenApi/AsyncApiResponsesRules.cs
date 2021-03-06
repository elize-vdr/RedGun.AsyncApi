// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Linq;
using System.Text.RegularExpressions;
using RedGun.AsyncApi.Models;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The validation rules for <see cref="AsyncApiResponses"/>.
    /// </summary>
    //[AsyncApiRule]
    public static class AsyncApiResponsesRules
    {
        /// <summary>
        /// An AsyncAPI operation must contain at least one response
        /// </summary>
        public static ValidationRule<AsyncApiResponses> ResponsesMustContainAtLeastOneResponse =>
            new ValidationRule<AsyncApiResponses>(
                (context, responses) =>
                {
                    if (!responses.Keys.Any())
                    {
                        context.CreateError(nameof(ResponsesMustContainAtLeastOneResponse),
                                "Responses must contain at least one response");
                    }
                });

        /// <summary>
        /// The response key must either be "default" or an HTTP status code (1xx, 2xx, 3xx, 4xx, 5xx).
        /// </summary>
        public static ValidationRule<AsyncApiResponses> ResponsesMustBeIdentifiedByDefaultOrStatusCode =>
            new ValidationRule<AsyncApiResponses>(
                (context, responses) =>
                {
                    foreach (var key in responses.Keys)
                    {
                        context.Enter(key);

                        if (key != "default" && !Regex.IsMatch(key, "^[1-5](?>[0-9]{2}|XX)$"))
                        {
                            context.CreateError(nameof(ResponsesMustBeIdentifiedByDefaultOrStatusCode),
                                    "Responses key must be 'default', an HTTP status code, " +
                                    "or one of the following strings representing a range of HTTP status codes: " +
                                    "'1XX', '2XX', '3XX', '4XX', '5XX'");
                        }

                        context.Exit();
                    }
                });
    }
}
