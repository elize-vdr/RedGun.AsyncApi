// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FluentAssertions;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Writers;
using Xunit;
using Xunit.Abstractions;

namespace RedGun.AsyncApi.Tests.Models
{
    [Collection("DefaultSettings")]
    public class AsyncApiParameterTests
    {
        public static AsyncApiParameter BasicParameter = new AsyncApiParameter
        {
            Name = "name1",
            In = ParameterLocation.Path
        };

        public static AsyncApiParameter ReferencedParameter = new AsyncApiParameter
        {
            Name = "name1",
            In = ParameterLocation.Path,
            Reference = new AsyncApiReference
            {
                Type = ReferenceType.Parameter,
                Id = "example1"
            }
        };

        public static AsyncApiParameter AdvancedPathParameterWithSchema = new AsyncApiParameter
        {
            Name = "name1",
            In = ParameterLocation.Path,
            Description = "description1",
            Required = true,
            Deprecated = false,

            Style = ParameterStyle.Simple,
            Explode = true,
            Schema = new AsyncApiSchema
            {
                Title = "title2",
                Description = "description2"
            },
            Examples = new Dictionary<string, AsyncApiExample>
            {
                ["test"] = new AsyncApiExample
                {
                    Summary = "summary3",
                    Description = "description3"
                }
            }
        };

        public static AsyncApiParameter ParameterWithFormStyleAndExplodeFalse = new AsyncApiParameter
        {
            Name = "name1",
            In = ParameterLocation.Query,
            Description = "description1",
            Style = ParameterStyle.Form,
            Explode = false,
            Schema = new AsyncApiSchema
            {
                Type = "array",
                Items = new AsyncApiSchema
                {
                    Enum = new List<IAsyncApiAny>
                    {
                        new AsyncApiString("value1"),
                        new AsyncApiString("value2")
                    }
                }
            }

        };

        public static AsyncApiParameter ParameterWithFormStyleAndExplodeTrue = new AsyncApiParameter
        {
            Name = "name1",
            In = ParameterLocation.Query,
            Description = "description1",
            Style = ParameterStyle.Form,
            Explode = true,
            Schema = new AsyncApiSchema
            {
                Type = "array",
                Items = new AsyncApiSchema
                {
                    Enum = new List<IAsyncApiAny>
                    {
                        new AsyncApiString("value1"),
                        new AsyncApiString("value2")
                    }
                }
            }

        };

        public static AsyncApiParameter AdvancedHeaderParameterWithSchemaReference = new AsyncApiParameter
        {
            Name = "name1",
            In = ParameterLocation.Header,
            Description = "description1",
            Required = true,
            Deprecated = false,

            Style = ParameterStyle.Simple,
            Explode = true,
            Schema = new AsyncApiSchema
            {
                Reference = new AsyncApiReference
                {
                    Type = ReferenceType.Schema,
                    Id = "schemaObject1"
                },
                UnresolvedReference = true
            },
            Examples = new Dictionary<string, AsyncApiExample>
            {
                ["test"] = new AsyncApiExample
                {
                    Summary = "summary3",
                    Description = "description3"
                }
            }
        };

        public static AsyncApiParameter AdvancedHeaderParameterWithSchemaTypeObject = new AsyncApiParameter
        {
            Name = "name1",
            In = ParameterLocation.Header,
            Description = "description1",
            Required = true,
            Deprecated = false,

            Style = ParameterStyle.Simple,
            Explode = true,
            Schema = new AsyncApiSchema
            {
                Type = "object"
            },
            Examples = new Dictionary<string, AsyncApiExample>
            {
                ["test"] = new AsyncApiExample
                {
                    Summary = "summary3",
                    Description = "description3"
                }
            }
        };

        private readonly ITestOutputHelper _output;

        public AsyncApiParameterTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData(ParameterStyle.Form, true)]
        [InlineData(ParameterStyle.SpaceDelimited, false)]
        [InlineData(null, false)]
        public void WhenStyleIsFormTheDefaultValueOfExplodeShouldBeTrueOtherwiseFalse(ParameterStyle? style, bool expectedExplode)
        {
            // Arrange
            var parameter = new AsyncApiParameter
            {
                Name = "name1",
                In = ParameterLocation.Query,
                Style = style
            };

            // Act & Assert
            parameter.Explode.Should().Be(expectedExplode);
        }

        [Fact]
        public void SerializeBasicParameterAsV3JsonWorks()
        {
            // Arrange
            var expected = @"{
  ""name"": ""name1"",
  ""in"": ""path""
}";

            // Act
            var actual = BasicParameter.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvancedParameterAsV3JsonWorks()
        {
            // Arrange
            var expected = @"{
  ""name"": ""name1"",
  ""in"": ""path"",
  ""description"": ""description1"",
  ""required"": true,
  ""style"": ""simple"",
  ""explode"": true,
  ""schema"": {
    ""title"": ""title2"",
    ""description"": ""description2""
  },
  ""examples"": {
    ""test"": {
      ""summary"": ""summary3"",
      ""description"": ""description3""
    }
  }
}";

            // Act
            var actual = AdvancedPathParameterWithSchema.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeReferencedParameterAsV3JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""$ref"": ""#/components/parameters/example1""
}";

            // Act
            ReferencedParameter.SerializeAsV3(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeReferencedParameterAsV3JsonWithoutReferenceWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""name"": ""name1"",
  ""in"": ""path""
}";

            // Act
            ReferencedParameter.SerializeAsV3WithoutReference(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeReferencedParameterAsV2JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""$ref"": ""#/parameters/example1""
}";

            // Act
            ReferencedParameter.SerializeAsV2(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeReferencedParameterAsV2JsonWithoutReferenceWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""in"": ""path"",
  ""name"": ""name1""
}";

            // Act
            ReferencedParameter.SerializeAsV2WithoutReference(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeParameterWithSchemaReferenceAsV2JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""in"": ""header"",
  ""name"": ""name1"",
  ""description"": ""description1"",
  ""required"": true,
  ""type"": ""string""
}";

            // Act
            AdvancedHeaderParameterWithSchemaReference.SerializeAsV2(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeParameterWithSchemaTypeObjectAsV2JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""in"": ""header"",
  ""name"": ""name1"",
  ""description"": ""description1"",
  ""required"": true,
  ""type"": ""string""
}";

            // Act
            AdvancedHeaderParameterWithSchemaTypeObject.SerializeAsV2(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeParameterWithFormStyleAndExplodeFalseWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""name"": ""name1"",
  ""in"": ""query"",
  ""description"": ""description1"",
  ""style"": ""form"",
  ""explode"": false,
  ""schema"": {
    ""type"": ""array"",
    ""items"": {
      ""enum"": [
        ""value1"",
        ""value2""
      ]
    }
  }
}";

            // Act
            ParameterWithFormStyleAndExplodeFalse.SerializeAsV3WithoutReference(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeParameterWithFormStyleAndExplodeTrueWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""name"": ""name1"",
  ""in"": ""query"",
  ""description"": ""description1"",
  ""style"": ""form"",
  ""schema"": {
    ""type"": ""array"",
    ""items"": {
      ""enum"": [
        ""value1"",
        ""value2""
      ]
    }
  }
}";

            // Act
            ParameterWithFormStyleAndExplodeTrue.SerializeAsV3WithoutReference(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }
    }
}
