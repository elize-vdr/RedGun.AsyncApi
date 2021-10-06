// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using FluentAssertions;

namespace RedGun.AsyncApi.Readers.Tests
{
    /// <summary>
    /// Fixture containing default settings for external libraries.
    /// </summary>
    public class DefaultSettingsFixture
    {
        /// <summary>
        /// Initializes an intance of <see cref="DefaultSettingsFixture"/>.
        /// </summary>
        public DefaultSettingsFixture()
        {
            // We need RespectingRuntimeTypes() to ensure equivalence test works property,
            // given that there are multiple types that can be used for the declared type OpenApiAny.
            // Without this option, properties specific to those types would not be compared.
            AssertionOptions.AssertEquivalencyUsing(
                o => o
                    .AllowingInfiniteRecursion()
                    .RespectingRuntimeTypes()
                    .WithStrictOrdering());
        }
    }
}
