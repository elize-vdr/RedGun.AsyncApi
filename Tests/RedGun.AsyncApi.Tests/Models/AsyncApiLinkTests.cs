// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Globalization;
using System.IO;
using FluentAssertions;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Expressions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Writers;
using Xunit;
using Xunit.Abstractions;

namespace RedGun.AsyncApi.Tests.Models
{
    [Collection("DefaultSettings")]
    public class AsyncApiLinkTests
    {
        public static AsyncApiLink AdvancedLink = new AsyncApiLink
        {
            OperationId = "operationId1",
            Parameters =
            {
                ["parameter1"] = new RuntimeExpressionAnyWrapper
                {
                    Expression = RuntimeExpression.Build("$request.path.id")
                }
            },
            RequestBody = new RuntimeExpressionAnyWrapper
            {
                Any = new AsyncApiObject
                {
                    ["property1"] = new AsyncApiBoolean(true)
                }
            },
            Description = "description1",
            Server = new AsyncApiServer
            {
                Description = "serverDescription1"
            }
        };

        public static AsyncApiLink ReferencedLink = new AsyncApiLink
        {
            Reference = new AsyncApiReference
            {
                Type = ReferenceType.Link,
                Id = "example1",
            },
            OperationId = "operationId1",
            Parameters =
            {
                ["parameter1"] = new RuntimeExpressionAnyWrapper
                {
                    Expression = RuntimeExpression.Build("$request.path.id")
                }
            },
            RequestBody = new RuntimeExpressionAnyWrapper
            {
                Any = new AsyncApiObject
                {
                    ["property1"] = new AsyncApiBoolean(true)
                }
            },
            Description = "description1",
            Server = new AsyncApiServer
            {
                Description = "serverDescription1"
            }
        };

        private readonly ITestOutputHelper _output;

        public AsyncApiLinkTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void SerializeAdvancedLinkAsV2JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""operationId"": ""operationId1"",
  ""parameters"": {
    ""parameter1"": ""$request.path.id""
  },
  ""requestBody"": {
    ""property1"": true
  },
  ""description"": ""description1"",
  ""server"": {
    ""description"": ""serverDescription1""
  }
}";

            // Act
            AdvancedLink.SerializeAsV2(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeReferencedLinkAsV2JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""$ref"": ""#/components/links/example1""
}";

            // Act
            ReferencedLink.SerializeAsV2(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeReferencedLinkAsV2JsonWithoutReferenceWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""operationId"": ""operationId1"",
  ""parameters"": {
    ""parameter1"": ""$request.path.id""
  },
  ""requestBody"": {
    ""property1"": true
  },
  ""description"": ""description1"",
  ""server"": {
    ""description"": ""serverDescription1""
  }
}";

            // Act
            ReferencedLink.SerializeAsV2WithoutReference(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }
    }
}
