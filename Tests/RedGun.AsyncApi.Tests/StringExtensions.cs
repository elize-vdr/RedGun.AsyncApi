// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;

namespace RedGun.AsyncApi.Tests
{
    /// <summary>
    /// Extensions for string class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Ensures line breaks are changed to the current environment line breaks
        /// </summary>
        /// <remarks>This makes sure comparison of two strings do not break simply due to line break differences.</remarks>
        public static string MakeLineBreaksEnvironmentNeutral(this string input)
        {
            return input.Replace("\r\n", "\n")
                .Replace("\r", "\n")
                .Replace("\n", Environment.NewLine);
        }
    }
}
