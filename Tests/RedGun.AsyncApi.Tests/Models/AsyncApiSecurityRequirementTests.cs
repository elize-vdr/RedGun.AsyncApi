// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using FluentAssertions;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using Xunit;

namespace RedGun.AsyncApi.Tests.Models
{
    [Collection("DefaultSettings")]
    public class AsyncApiSecurityRequirementTests
    {
        public static AsyncApiSecurityRequirement BasicSecurityRequirement = new AsyncApiSecurityRequirement();

        public static AsyncApiSecurityRequirement SecurityRequirementWithReferencedSecurityScheme =
            new AsyncApiSecurityRequirement
            {
                [
                    new AsyncApiSecurityScheme
                    {
                        Reference = new AsyncApiReference { Type = ReferenceType.SecurityScheme, Id = "scheme1" }
                    }
                ] = new List<string>
                {
                    "scope1",
                    "scope2",
                    "scope3",
                },
                [
                    new AsyncApiSecurityScheme
                    {
                        Reference = new AsyncApiReference { Type = ReferenceType.SecurityScheme, Id = "scheme2" }
                    }
                ] = new List<string>
                {
                    "scope4",
                    "scope5",
                },
                [
                    new AsyncApiSecurityScheme
                    {
                        Reference = new AsyncApiReference { Type = ReferenceType.SecurityScheme, Id = "scheme3" }
                    }
                ] = new List<string>()
            };

        public static AsyncApiSecurityRequirement SecurityRequirementWithUnreferencedSecurityScheme =
            new AsyncApiSecurityRequirement
            {
                [
                    new AsyncApiSecurityScheme
                    {
                        Reference = new AsyncApiReference { Type = ReferenceType.SecurityScheme, Id = "scheme1" }
                    }
                ] = new List<string>
                {
                    "scope1",
                    "scope2",
                    "scope3",
                },
                [
                    new AsyncApiSecurityScheme
                    {
                        // This security scheme is unreferenced, so this key value pair cannot be serialized.
                        Name = "brokenUnreferencedScheme"
                    }
                ] = new List<string>
                {
                    "scope4",
                    "scope5",
                },
                [
                    new AsyncApiSecurityScheme
                    {
                        Reference = new AsyncApiReference { Type = ReferenceType.SecurityScheme, Id = "scheme3" }
                    }
                ] = new List<string>()
            };

        [Fact]
        public void SerializeBasicSecurityRequirementAsV3JsonWorks()
        {
            // Arrange
            var expected = @"{ }";

            // Act
            var actual = BasicSecurityRequirement.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeSecurityRequirementWithReferencedSecuritySchemeAsV3JsonWorks()
        {
            // Arrange
            var expected =
                @"{
  ""scheme1"": [
    ""scope1"",
    ""scope2"",
    ""scope3""
  ],
  ""scheme2"": [
    ""scope4"",
    ""scope5""
  ],
  ""scheme3"": [ ]
}";

            // Act
            var actual =
                SecurityRequirementWithReferencedSecurityScheme.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeSecurityRequirementWithReferencedSecuritySchemeAsV2JsonWorks()
        {
            // Arrange
            var expected =
                @"{
  ""scheme1"": [
    ""scope1"",
    ""scope2"",
    ""scope3""
  ],
  ""scheme2"": [
    ""scope4"",
    ""scope5""
  ],
  ""scheme3"": [ ]
}";

            // Act
            var actual = SecurityRequirementWithReferencedSecurityScheme.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void
            SerializeSecurityRequirementWithUnreferencedSecuritySchemeAsV3JsonShouldSkipUnserializableKeyValuePair()
        {
            // Arrange
            var expected =
                @"{
  ""scheme1"": [
    ""scope1"",
    ""scope2"",
    ""scope3""
  ],
  ""scheme3"": [ ]
}";

            // Act
            var actual =
                SecurityRequirementWithUnreferencedSecurityScheme.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void
            SerializeSecurityRequirementWithUnreferencedSecuritySchemeAsV2JsonShouldSkipUnserializableKeyValuePair()
        {
            // Arrange
            var expected =
                @"{
  ""scheme1"": [
    ""scope1"",
    ""scope2"",
    ""scope3""
  ],
  ""scheme3"": [ ]
}";

            // Act
            var actual =
                SecurityRequirementWithUnreferencedSecurityScheme.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SchemesShouldConsiderOnlyReferenceIdForEquality()
        {
            // Arrange
            var securityRequirement = new AsyncApiSecurityRequirement();

            var securityScheme1 = new AsyncApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                Name = "apiKeyName1",
                In = ParameterLocation.Header,
                Reference = new AsyncApiReference
                {
                    Id = "securityScheme1",
                    Type = ReferenceType.SecurityScheme
                }
            };

            var securityScheme2 = new AsyncApiSecurityScheme
            {
                Type = SecuritySchemeType.OpenIdConnect,
                OpenIdConnectUrl = new Uri("http://example.com"),
                Reference = new AsyncApiReference
                {
                    Id = "securityScheme2",
                    Type = ReferenceType.SecurityScheme
                }
            };

            var securityScheme1Duplicate = new AsyncApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                Name = "apiKeyName1",
                In = ParameterLocation.Header,
                Reference = new AsyncApiReference
                {
                    Id = "securityScheme1",
                    Type = ReferenceType.SecurityScheme
                }
            };

            var securityScheme1WithDifferentProperties = new AsyncApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                Name = "apiKeyName2",
                In = ParameterLocation.Query,
                Reference = new AsyncApiReference
                {
                    Id = "securityScheme1",
                    Type = ReferenceType.SecurityScheme
                }
            };

            // Act
            securityRequirement.Add(securityScheme1, new List<string>());
            securityRequirement.Add(securityScheme2, new List<string> { "scope1", "scope2" });

            Action addSecurityScheme1Duplicate = () =>
                securityRequirement.Add(securityScheme1Duplicate, new List<string>());
            Action addSecurityScheme1WithDifferentProperties = () =>
                securityRequirement.Add(securityScheme1WithDifferentProperties, new List<string>());

            // Assert
            // Only the first two should be added successfully since the latter two are duplicates of securityScheme1.
            // Duplicate determination only considers Reference.Id.
            addSecurityScheme1Duplicate.Should().Throw<ArgumentException>();
            addSecurityScheme1WithDifferentProperties.Should().Throw<ArgumentException>();

            securityRequirement.Should().HaveCount(2);

            securityRequirement.Should().BeEquivalentTo(
                new AsyncApiSecurityRequirement
                {
                    // This should work with any security scheme object
                    // as long as Reference.Id os securityScheme1
                    [securityScheme1WithDifferentProperties] = new List<string>(),
                    [securityScheme2] = new List<string> { "scope1", "scope2" },
                });
        }
    }
}
