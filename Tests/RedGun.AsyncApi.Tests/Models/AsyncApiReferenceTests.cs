﻿// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using FluentAssertions;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using Xunit;

namespace RedGun.AsyncApi.Tests.Models
{
    [Collection("DefaultSettings")]
    public class AsyncApiReferenceTests
    {
        [Theory]
        [InlineData("#/components/schemas/Pet", ReferenceType.Schema, "Pet")]
        [InlineData("#/components/parameters/name", ReferenceType.Parameter, "name")]
        [InlineData("#/components/responses/200", ReferenceType.Response, "200")]
        public void SettingInternalReferenceForComponentsStyleReferenceShouldSucceed(
            string input,
            ReferenceType type,
            string id)
        {
            // Arrange & Act
            var reference = new AsyncApiReference
            {
                Type = type,
                Id = id
            };

            // Assert
            reference.ExternalResource.Should().BeNull();
            reference.Type.Should().Be(type);
            reference.Id.Should().Be(id);

            //reference.ReferenceV2.Should().Be(input);
            reference.ReferenceV2.Should().Be(input);
        }

        [Theory]
        [InlineData("Pet.json", "Pet.json", null)]
        [InlineData("Pet.yaml", "Pet.yaml", null)]
        [InlineData("abc", "abc", null)]
        [InlineData("Pet.json#/Pet", "Pet.json", "Pet")]
        [InlineData("Pet.yaml#/Pet", "Pet.yaml", "Pet")]
        [InlineData("abc#/Pet", "abc", "Pet")]
        public void SettingExternalReferenceShouldSucceed(string expected, string externalResource, string id)
        {
            // Arrange & Act
            var reference = new AsyncApiReference
            {
                ExternalResource = externalResource,
                Id = id
            };

            // Assert
            reference.ExternalResource.Should().Be(externalResource);
            reference.Type.Should().BeNull();
            reference.Id.Should().Be(id);

            //reference.ReferenceV2.Should().Be(expected);
            reference.ReferenceV2.Should().Be(expected);
        }

        [Fact]
        public void SerializeSchemaReferenceAsJsonV2Works()
        {
            // Arrange
            var reference = new AsyncApiReference { Type = ReferenceType.Schema, Id = "Pet" };
            var expected = @"{
  ""$ref"": ""#/components/schemas/Pet""
}";

            // Act
            var actual = reference.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual = actual.MakeLineBreaksEnvironmentNeutral();

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeSchemaReferenceAsYamlV2Works()
        {
            // Arrange
            var reference = new AsyncApiReference
            {
                Type = ReferenceType.Schema,
                Id = "Pet"
            };

            var expected = @"$ref: '#/components/schemas/Pet'";

            // Act
            var actual = reference.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeExternalReferenceAsJsonV2Works()
        {
            // Arrange
            var reference = new AsyncApiReference { ExternalResource = "main.json", Id = "Pets" };

            var expected = @"{
  ""$ref"": ""main.json#/Pets""
}";

            // Act
            var actual = reference.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual = actual.MakeLineBreaksEnvironmentNeutral();

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeExternalReferenceAsYamlV2Works()
        {
            // Arrange
            var reference = new AsyncApiReference { ExternalResource = "main.json", Id = "Pets" };
            var expected = @"$ref: main.json#/Pets";

            // Act
            var actual = reference.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual.Should().Be(expected);
        }
    }
}
