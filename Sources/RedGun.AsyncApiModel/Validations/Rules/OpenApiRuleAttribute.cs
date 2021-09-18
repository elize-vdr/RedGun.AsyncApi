// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;

namespace RedGun.AsyncApi.Validations.Rules
{
    /// <summary>
    /// The Validator attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class OpenApiRuleAttribute : Attribute
    {
    }
}
