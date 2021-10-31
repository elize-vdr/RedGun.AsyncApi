// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The validation rules for <see cref="AsyncApiBindingHttpOperation"/>.
    /// </summary>
    [AsyncApiRule]
    public static class AsyncApiBindingHttpOperationRules
    {
        /// <summary>
        /// Method: When type is request, this is the HTTP method, otherwise it MUST be ignored.
        /// Its value MUST be one of GET, POST, PUT, PATCH, DELETE, HEAD, OPTIONS, CONNECT, and TRACE.
        /// </summary>
        public static ValidationRule<AsyncApiBindingHttpOperation> TypeIsRequiredAndMustBeRequestOrResponse =>
            new ValidationRule<AsyncApiBindingHttpOperation>(
                (context, item) =>
                {
                    context.Enter("type");
                    if (String.IsNullOrWhiteSpace(item.Type))
                    {
                        context.CreateError(nameof(TypeIsRequiredAndMustBeRequestOrResponse),
                            String.Format(SRResource.Validation_FieldIsRequired, "type", "http binding"));
                    }
                    else
                    {
                        List<string> typeList = new List<string> {"request", "response"};
                        var found = typeList.Contains(item.Type);
                        if (!found)
                        {
                            context.CreateError(nameof(TypeIsRequiredAndMustBeRequestOrResponse),
                                "The binding type MUST be 'request' or 'response'");
                        }
                    }
                    context.Exit();
                });
        
        /// <summary>
        /// Method: When type is request, this is the HTTP method, otherwise it MUST be ignored.
        /// Its value MUST be one of GET, POST, PUT, PATCH, DELETE, HEAD, OPTIONS, CONNECT, and TRACE.
        /// </summary>
        public static ValidationRule<AsyncApiBindingHttpOperation> MethodMustBeHttpMethodIfTypeIsRequest =>
            new ValidationRule<AsyncApiBindingHttpOperation>(
                (context, item) =>
                {
                    context.Enter("method");
                    if (!String.IsNullOrWhiteSpace(item.Method))
                    {
                        List<string> methodList = new List<string> {"GET", "POST", "PUT", "PATCH", "DELETE", "HEAD", "OPTIONS", "CONNECT", "TRACE"};
                        var found = methodList.Contains(item.Method);
                        if (!found)
                        {
                            context.CreateError(nameof(MethodMustBeHttpMethodIfTypeIsRequest),
                                "If the Type is 'request' the Method must be one of " +
                                "GET, POST, PUT, PATCH, DELETE, HEAD, OPTIONS, CONNECT, or TRACE.");
                        }
                    }
                    context.Exit();
                });

    }
    
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
