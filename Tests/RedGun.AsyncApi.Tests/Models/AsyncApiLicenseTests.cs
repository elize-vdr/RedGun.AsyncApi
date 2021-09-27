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
    public class AsyncApiLicenseTests
    {
        public static AsyncApiLicense BasicLicense = new AsyncApiLicense
        {
            Name = "Apache 2.0"
        };

        public static AsyncApiLicense AdvanceLicense = new AsyncApiLicense
        {
            Name = "Apache 2.0",
            Url = new Uri("http://www.apache.org/licenses/LICENSE-2.0.html"),
            Extensions = new Dictionary<string, IAsyncApiExtension>
            {
                {"x-copyright", new AsyncApiString("Abc")}
            }
        };

        [Theory]
        //[InlineData(AsyncApiSpecVersion.AsyncApi3_0)]
        [InlineData(AsyncApiSpecVersion.AsyncApi2_0)]
        public void SerializeBasicLicenseAsJsonWorks(AsyncApiSpecVersion version)
        {
            // Arrange
            var expected =
                @"{
  ""name"": ""Apache 2.0""
}";

            // Act
            var actual = BasicLicense.SerializeAsJson(version);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Theory]
        //[InlineData(AsyncApiSpecVersion.AsyncApi3_0)]
        [InlineData(AsyncApiSpecVersion.AsyncApi2_0)]
        public void SerializeBasicLicenseAsYamlWorks(AsyncApiSpecVersion version)
        {
            // Arrange
            var expected = "name: Apache 2.0";

            // Act
            var actual = BasicLicense.SerializeAsYaml(version);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Theory]
        //[InlineData(AsyncApiSpecVersion.AsyncApi3_0)]
        [InlineData(AsyncApiSpecVersion.AsyncApi2_0)]
        public void SerializeAdvanceLicenseAsJsonWorks(AsyncApiSpecVersion version)
        {
            // Arrange
            var expected =
                @"{
  ""name"": ""Apache 2.0"",
  ""url"": ""http://www.apache.org/licenses/LICENSE-2.0.html"",
  ""x-copyright"": ""Abc""
}";

            // Act
            var actual = AdvanceLicense.SerializeAsJson(version);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Theory]
        //[InlineData(AsyncApiSpecVersion.AsyncApi3_0)]
        [InlineData(AsyncApiSpecVersion.AsyncApi2_0)]
        public void SerializeAdvanceLicenseAsYamlWorks(AsyncApiSpecVersion version)
        {
            // Arrange
            var expected =
                @"name: Apache 2.0
url: http://www.apache.org/licenses/LICENSE-2.0.html
x-copyright: Abc";

            // Act
            var actual = AdvanceLicense.SerializeAsYaml(version);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }
    }
}
