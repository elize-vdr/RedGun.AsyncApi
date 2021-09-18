﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The validation rules for <see cref="AsyncApiPaths"/>.
    /// </summary>
    [OpenApiRule]
    public static class OpenApiPathsRules
    {

        /// <summary>
        /// A relative path to an individual endpoint. The field name MUST begin with a slash.
        /// </summary>
        public static ValidationRule<AsyncApiPaths> PathNameMustBeginWithSlash =>
            new ValidationRule<AsyncApiPaths>(
                (context, item) =>
                {
                    foreach (var pathName in item.Keys)
                    {
                        context.Enter(pathName);

                        if (pathName == null || !pathName.StartsWith("/"))
                        {
                            context.CreateError(nameof(PathNameMustBeginWithSlash),
                                string.Format(SRResource.Validation_PathItemMustBeginWithSlash, pathName));
                        }

                        context.Exit();
                    }
                });

        // add more rules
    }
}
