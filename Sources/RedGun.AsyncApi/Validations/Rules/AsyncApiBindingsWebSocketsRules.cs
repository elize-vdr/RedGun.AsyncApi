// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The validation rules for <see cref="AsyncApiBindingWebSocketsChannel"/>.
    /// </summary>
    [AsyncApiRule]
    public static class AsyncApiBindingWebSocketsChannelRules
    {
        /// <summary>
        /// Method: When type is request, this is the HTTP method, otherwise it MUST be ignored.
        /// Its value MUST be one of GET, POST, PUT, PATCH, DELETE, HEAD, OPTIONS, CONNECT, and TRACE.
        /// </summary>
        public static ValidationRule<AsyncApiBindingWebSocketsChannel> MethodIsRequiredAndMustBeGetOrPost =>
            new ValidationRule<AsyncApiBindingWebSocketsChannel>(
                (context, item) =>
                {
                    context.Enter("method");
                    if (String.IsNullOrWhiteSpace(item.Method))
                    {
                        context.CreateError(nameof(MethodIsRequiredAndMustBeGetOrPost),
                            String.Format(SRResource.Validation_FieldIsRequired, "method", "ws binding"));
                    }
                    else
                    {
                        List<string> methodList = new List<string> {"GET", "POST"};
                        var found = methodList.Contains(item.Method);
                        if (!found)
                        {
                            context.CreateError(nameof(MethodIsRequiredAndMustBeGetOrPost),
                                "The WebSockets binding method MUST be 'GET' or 'POST'");
                        }
                    }
                    context.Exit();
                });
    }
}
