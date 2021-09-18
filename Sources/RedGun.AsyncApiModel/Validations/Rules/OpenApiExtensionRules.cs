﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The validation rules for <see cref="IAsyncApiExtensible"/>.
    /// </summary>
    [OpenApiRule]
    public static class OpenApiExtensibleRules
    {
        /// <summary>
        /// Extension name MUST start with "x-".
        /// </summary>
        public static ValidationRule<IAsyncApiExtensible> ExtensionNameMustStartWithXDash =>
            new ValidationRule<IAsyncApiExtensible>(
                (context, item) =>
                {
                    context.Enter("extensions");
                    foreach (var extensible in item.Extensions)
                    {
                        if (!extensible.Key.StartsWith("x-"))
                        {
                            context.CreateError(nameof(ExtensionNameMustStartWithXDash),
                                String.Format(SRResource.Validation_ExtensionNameMustBeginWithXDash, extensible.Key, context.PathString));
                        }
                    }
                    context.Exit();
                });
    }
}
