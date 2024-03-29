﻿// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license.

using System;

namespace RedGun.AsyncApi
{
    /// <summary>
    /// Utilities methods
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// Check whether the input argument value is null or not.
        /// </summary>
        /// <typeparam name="T">The input value type.</typeparam>
        /// <param name="value">The input value.</param>
        /// <param name="parameterName">The input parameter name.</param>
        /// <returns>The input value.</returns>
        internal static T CheckArgumentNull<T>(T value, string parameterName) where T : class
        {
            return value ?? throw new ArgumentNullException(parameterName, $"Value cannot be null: {parameterName}");
        }

        /// <summary>
        /// Check whether the input string value is null or empty.
        /// </summary>
        /// <param name="value">The input string value.</param>
        /// <param name="parameterName">The input parameter name.</param>
        /// <returns>The input value.</returns>
        internal static string CheckArgumentNullOrEmpty(string value, string parameterName)
        {
            return string.IsNullOrEmpty(value) ? throw new ArgumentNullException(parameterName, $"Value cannot be null or empty: {parameterName}") : value;
        }
    }
}
