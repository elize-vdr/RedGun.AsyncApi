// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.ParseNodes;

namespace RedGun.AsyncApi.Readers.V2
{
    /// <summary>
    /// Class containing logic to deserialize Async API V3 document into
    /// runtime Async API object model.
    /// </summary>
    internal static partial class AsyncApiV2Deserializer
    {
        public static AsyncApiSecurityRequirement LoadSecurityRequirement(ParseNode node)
        {
            var mapNode = node.CheckMapNode("security");

            var securityRequirement = new AsyncApiSecurityRequirement();

            foreach (var property in mapNode)
            {
                var scheme = LoadSecuritySchemeByReference(
                    mapNode.Context,
                    property.Name);

                var scopes = property.Value.CreateSimpleList(value => value.GetScalarValue());

                if (scheme != null)
                {
                    securityRequirement.Add(scheme, scopes);
                }
                else
                {
                    mapNode.Context.Diagnostic.Errors.Add(
                        new AsyncApiError(node.Context.GetLocation(), $"Scheme {property.Name} is not found"));
                }
            }

            return securityRequirement;
        }

        private static AsyncApiSecurityScheme LoadSecuritySchemeByReference(
            ParsingContext context,
            string schemeName)
        {
            var securitySchemeObject = new AsyncApiSecurityScheme()
            {
                UnresolvedReference = true,
                Reference = new AsyncApiReference()
                {
                    Id = schemeName,
                    Type = ReferenceType.SecurityScheme
                }
            };

            return securitySchemeObject;
        }
    }
}
