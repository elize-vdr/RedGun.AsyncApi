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
    public class AsyncApiXmlTests
    {
        public static AsyncApiXml AdvancedXml = new AsyncApiXml
        {
            Name = "animal",
            Namespace = new Uri("http://swagger.io/schema/sample"),
            Prefix = "sample",
            Wrapped = true,
            Attribute = true,
            Extensions = new Dictionary<string, IAsyncApiExtension>
            {
                {"x-xml-extension", new AsyncApiInteger(7)}
            }
        };

        public static AsyncApiXml BasicXml = new AsyncApiXml();

        [Theory]
        //[InlineData(AsyncApiSpecVersion.AsyncApi3_0, AsyncApiFormat.Json)]
        [InlineData(AsyncApiSpecVersion.AsyncApi2_0, AsyncApiFormat.Json)]
        //[InlineData(AsyncApiSpecVersion.AsyncApi3_0, AsyncApiFormat.Yaml)]
        [InlineData(AsyncApiSpecVersion.AsyncApi2_0, AsyncApiFormat.Yaml)]
        public void SerializeBasicXmlWorks(
            AsyncApiSpecVersion version,
            AsyncApiFormat format)
        {
            // Act
            var actual = BasicXml.Serialize(version, format);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be("{ }");
        }

        [Theory]
        //[InlineData(AsyncApiSpecVersion.AsyncApi3_0)]
        [InlineData(AsyncApiSpecVersion.AsyncApi2_0)]
        public void SerializeAdvancedXmlAsJsonWorks(AsyncApiSpecVersion version)
        {
            // Arrange
            var expected =
                @"{
  ""name"": ""animal"",
  ""namespace"": ""http://swagger.io/schema/sample"",
  ""prefix"": ""sample"",
  ""attribute"": true,
  ""wrapped"": true,
  ""x-xml-extension"": 7
}";

            // Act
            var actual = AdvancedXml.SerializeAsJson(version);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Theory]
        //[InlineData(AsyncApiSpecVersion.AsyncApi3_0)]
        [InlineData(AsyncApiSpecVersion.AsyncApi2_0)]
        public void SerializeAdvancedXmlAsYamlWorks(AsyncApiSpecVersion version)
        {
            // Arrange
            var expected =
                @"name: animal
namespace: http://swagger.io/schema/sample
prefix: sample
attribute: true
wrapped: true
x-xml-extension: 7";

            // Act
            var actual = AdvancedXml.SerializeAsYaml(version);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }
    }
}
