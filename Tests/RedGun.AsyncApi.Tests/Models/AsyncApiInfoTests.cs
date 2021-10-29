// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using FluentAssertions;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using Xunit;

namespace RedGun.AsyncApi.Tests.Models
{
    [Collection("DefaultSettings")]
    public class AsyncApiInfoTests
    {
        public static AsyncApiInfo AdvanceInfo = new AsyncApiInfo
        {
            Title = "Sample Pet Store App",
            Description = "This is a sample server for a pet store.",
            TermsOfService = new Uri("http://example.com/terms/"),
            Contact = AsyncApiContactTests.AdvanceContact,
            License = AsyncApiLicenseTests.AdvanceLicense,
            Version = "1.1.1",
            Extensions = new Dictionary<string, IAsyncApiExtension>
            {
                {"x-updated", new AsyncApiString("metadata")}
            }
        };

        public static AsyncApiInfo BasicInfo = new AsyncApiInfo
        {
            Title = "Sample Pet Store App",
            Version = "1.0"
        };

        public static IEnumerable<object[]> BasicInfoJsonExpected()
        {
            var specVersions = new[] { AsyncApiSpecVersion.AsyncApi2_0 /*, AsyncApiSpecVersion.AsyncApi3_0*/ };
            foreach (var specVersion in specVersions)
            {
                yield return new object[]
                {
                    specVersion,
                    @"{
  ""title"": ""Sample Pet Store App"",
  ""version"": ""1.0""
}"
                };
            }
        }

        [Theory]
        [MemberData(nameof(BasicInfoJsonExpected))]
        public void SerializeBasicInfoAsJsonWorks(AsyncApiSpecVersion version, string expected)
        {
            // Arrange & Act
            var actual = BasicInfo.SerializeAsJson(version);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        public static IEnumerable<object[]> BasicInfoYamlExpected()
        {
            var specVersions = new[] { AsyncApiSpecVersion.AsyncApi2_0 /*, AsyncApiSpecVersion.AsyncApi3_0*/ };
            foreach (var specVersion in specVersions)
            {
                yield return new object[]
                {
                    specVersion,
                    @"title: Sample Pet Store App
version: '1.0'"
                };
            }
        }

        [Theory]
        [MemberData(nameof(BasicInfoYamlExpected))]
        public void SerializeBasicInfoAsYamlWorks(AsyncApiSpecVersion version, string expected)
        {
            // Arrange & Act
            var actual = BasicInfo.SerializeAsYaml(version);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        public static IEnumerable<object[]> AdvanceInfoJsonExpect()
        {
            var specVersions = new[] { AsyncApiSpecVersion.AsyncApi2_0 /*, AsyncApiSpecVersion.AsyncApi3_0*/ };
            foreach (var specVersion in specVersions)
            {
                yield return new object[]
                {
                    specVersion,
                    @"{
  ""title"": ""Sample Pet Store App"",
  ""description"": ""This is a sample server for a pet store."",
  ""termsOfService"": ""http://example.com/terms/"",
  ""contact"": {
    ""name"": ""API Support"",
    ""url"": ""http://www.example.com/support"",
    ""email"": ""support@example.com"",
    ""x-internal-id"": 42
  },
  ""license"": {
    ""name"": ""Apache 2.0"",
    ""url"": ""http://www.apache.org/licenses/LICENSE-2.0.html"",
    ""x-copyright"": ""Abc""
  },
  ""version"": ""1.1.1"",
  ""x-updated"": ""metadata""
}"
                };
            }
        }

        [Theory]
        [MemberData(nameof(AdvanceInfoJsonExpect))]
        public void SerializeAdvanceInfoAsJsonWorks(AsyncApiSpecVersion version, string expected)
        {
            // Arrange & Act
            var actual = AdvanceInfo.SerializeAsJson(version);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        public static IEnumerable<object[]> AdvanceInfoYamlExpect()
        {
            var specVersions = new[] { AsyncApiSpecVersion.AsyncApi2_0 /*, AsyncApiSpecVersion.AsyncApi3_0*/ };
            foreach (var specVersion in specVersions)
            {
                yield return new object[]
                {
                    specVersion,
                    @"title: Sample Pet Store App
description: This is a sample server for a pet store.
termsOfService: http://example.com/terms/
contact:
  name: API Support
  url: http://www.example.com/support
  email: support@example.com
  x-internal-id: 42
license:
  name: Apache 2.0
  url: http://www.apache.org/licenses/LICENSE-2.0.html
  x-copyright: Abc
version: '1.1.1'
x-updated: metadata"
                };
            }
        }

        [Theory]
        [MemberData(nameof(AdvanceInfoYamlExpect))]
        public void SerializeAdvanceInfoAsYamlWorks(AsyncApiSpecVersion version, string expected)
        {
            // Arrange & Act
            var actual = AdvanceInfo.SerializeAsYaml(version);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void InfoVersionShouldAcceptDateStyledAsVersions()
        {
            // Arrange
            var info = new AsyncApiInfo
            {
                Title = "Sample Pet Store App",
                Version = "2017-03-01"
            };

            var expected =
                @"title: Sample Pet Store App
version: '2017-03-01'";

            // Act
            var actual = info.Serialize(AsyncApiSpecVersion.AsyncApi2_0, AsyncApiFormat.Yaml);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }
    }
}
