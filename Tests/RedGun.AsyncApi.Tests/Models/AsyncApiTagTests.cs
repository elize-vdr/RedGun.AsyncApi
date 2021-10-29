// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FluentAssertions;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Writers;
using Xunit;

namespace RedGun.AsyncApi.Tests.Models
{
    [Collection("DefaultSettings")]
    public class AsyncApiTagTests
    {
        public static AsyncApiTag BasicTag = new AsyncApiTag();

        public static AsyncApiTag AdvancedTag = new AsyncApiTag
        {
            Name = "pet",
            Description = "Pets operations",
            ExternalDocs = AsyncApiExternalDocsTests.AdvanceExDocs,
            Extensions = new Dictionary<string, IAsyncApiExtension>
            {
                {"x-tag-extension", new AsyncApiNull()}
            }
        };

        public static AsyncApiTag ReferencedTag = new AsyncApiTag
        {
            Name = "pet",
            Description = "Pets operations",
            ExternalDocs = AsyncApiExternalDocsTests.AdvanceExDocs,
            Extensions = new Dictionary<string, IAsyncApiExtension>
            {
                {"x-tag-extension", new AsyncApiNull()}
            },
            Reference = new AsyncApiReference
            {
                Type = ReferenceType.Tag,
                Id = "pet"
            }
        };

        [Fact]
        public void SerializeBasicTagAsV2JsonWithoutReferenceWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected = "{ }";

            // Act
            BasicTag.SerializeAsV2WithoutReference(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeBasicTagAsV2YamlWithoutReferenceWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiYamlWriter(outputStringWriter);
            var expected = "{ }";

            // Act
            BasicTag.SerializeAsV2WithoutReference(writer);
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvancedTagAsV2JsonWithoutReferenceWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""name"": ""pet"",
  ""description"": ""Pets operations"",
  ""externalDocs"": {
    ""description"": ""Find more info here"",
    ""url"": ""https://example.com""
  },
  ""x-tag-extension"": null
}";

            // Act
            AdvancedTag.SerializeAsV2WithoutReference(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }
        
        [Fact]
        public void SerializeAdvancedTagAsV2YamlWithoutReferenceWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiYamlWriter(outputStringWriter);
            var expected =
                @"name: pet
description: Pets operations
externalDocs:
  description: Find more info here
  url: https://example.com
x-tag-extension: ";

            // Act
            AdvancedTag.SerializeAsV2WithoutReference(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvancedTagAsV2JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);

            var expected = @"""pet""";

            // Act
            AdvancedTag.SerializeAsV2(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvancedTagAsV2YamlWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiYamlWriter(outputStringWriter);

            var expected = @" pet";

            // Act
            AdvancedTag.SerializeAsV2(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeReferencedTagAsV2JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);

            var expected = @"""pet""";

            // Act
            ReferencedTag.SerializeAsV2(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeReferencedTagAsV2YamlWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiYamlWriter(outputStringWriter);

            var expected = @" pet";

            // Act
            ReferencedTag.SerializeAsV2(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }
    }
}
