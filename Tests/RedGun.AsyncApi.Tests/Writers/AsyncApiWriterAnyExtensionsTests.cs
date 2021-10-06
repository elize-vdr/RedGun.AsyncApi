// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Globalization;
using System.IO;
using FluentAssertions;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Writers;
using Xunit;

namespace RedGun.AsyncApi.Tests.Writers
{
    [Collection("DefaultSettings")]
    public class AsyncApiWriterAnyExtensionsTests
    {
        [Fact]
        public void WriteAsyncApiNullAsJsonWorks()
        {
            // Arrange
            var nullValue = new AsyncApiNull();

            var json = WriteAsJson(nullValue);

            // Assert
            json.Should().Be("null");
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(42)]
        [InlineData(int.MaxValue)]
        public void WriteAsyncApiIntegerAsJsonWorks(int input)
        {
            // Arrange
            var intValue = new AsyncApiInteger(input);

            var json = WriteAsJson(intValue);

            // Assert
            json.Should().Be(input.ToString());
        }

        [Theory]
        [InlineData(long.MinValue)]
        [InlineData(42)]
        [InlineData(long.MaxValue)]
        public void WriteAsyncApiLongAsJsonWorks(long input)
        {
            // Arrange
            var longValue = new AsyncApiLong(input);

            var json = WriteAsJson(longValue);

            // Assert
            json.Should().Be(input.ToString());
        }

        [Theory]
        [InlineData(float.MinValue)]
        [InlineData(42.42)]
        [InlineData(float.MaxValue)]
        public void WriteAsyncApiFloatAsJsonWorks(float input)
        {
            // Arrange
            var floatValue = new AsyncApiFloat(input);

            var json = WriteAsJson(floatValue);

            // Assert
            json.Should().Be(input.ToString());
        }

        [Theory]
        [InlineData(double.MinValue)]
        [InlineData(42.42)]
        [InlineData(double.MaxValue)]
        public void WriteAsyncApiDoubleAsJsonWorks(double input)
        {
            // Arrange
            var doubleValue = new AsyncApiDouble(input);

            var json = WriteAsJson(doubleValue);

            // Assert
            json.Should().Be(input.ToString());
        }

        [Theory]
        [InlineData("2017-1-2")]
        [InlineData("1999-01-02T12:10:22")]
        [InlineData("1999-01-03")]
        [InlineData("10:30:12")]
        public void WriteAsyncApiDateTimeAsJsonWorks(string inputString)
        {
            // Arrange
            var input = DateTimeOffset.Parse(inputString, CultureInfo.InvariantCulture);
            var dateTimeValue = new AsyncApiDateTime(input);

            var json = WriteAsJson(dateTimeValue);
            var expectedJson = "\"" + input.ToString("o") + "\"";

            // Assert
            json.Should().Be(expectedJson);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void WriteAsyncApiBooleanAsJsonWorks(bool input)
        {
            // Arrange
            var boolValue = new AsyncApiBoolean(input);

            var json = WriteAsJson(boolValue);

            // Assert
            json.Should().Be(input.ToString().ToLower());
        }

        [Fact]
        public void WriteAsyncApiObjectAsJsonWorks()
        {
            // Arrange
            var AsyncApiObject = new AsyncApiObject
            {
                {"stringProp", new AsyncApiString("stringValue1")},
                {"objProp", new AsyncApiObject()},
                {
                    "arrayProp",
                    new AsyncApiArray
                    {
                        new AsyncApiBoolean(false)
                    }
                }
            };

            var actualJson = WriteAsJson(AsyncApiObject);

            // Assert

            var expectedJson = @"{
  ""stringProp"": ""stringValue1"",
  ""objProp"": { },
  ""arrayProp"": [
    false
  ]
}";
            expectedJson = expectedJson.MakeLineBreaksEnvironmentNeutral();

            actualJson.Should().Be(expectedJson);
        }

        [Fact]
        public void WriteAsyncApiArrayAsJsonWorks()
        {
            // Arrange
            var AsyncApiObject = new AsyncApiObject
            {
                {"stringProp", new AsyncApiString("stringValue1")},
                {"objProp", new AsyncApiObject()},
                {
                    "arrayProp",
                    new AsyncApiArray
                    {
                        new AsyncApiBoolean(false)
                    }
                }
            };

            var array = new AsyncApiArray
            {
                new AsyncApiBoolean(false),
                AsyncApiObject,
                new AsyncApiString("stringValue2")
            };

            var actualJson = WriteAsJson(array);

            // Assert

            var expectedJson = @"[
  false,
  {
    ""stringProp"": ""stringValue1"",
    ""objProp"": { },
    ""arrayProp"": [
      false
    ]
  },
  ""stringValue2""
]";

            expectedJson = expectedJson.MakeLineBreaksEnvironmentNeutral();

            actualJson.Should().Be(expectedJson);
        }

        private static string WriteAsJson(IAsyncApiAny any)
        {
            // Arrange (continued)
            var stream = new MemoryStream();
            IAsyncApiWriter writer = new AsyncApiJsonWriter(new StreamWriter(stream));

            writer.WriteAny(any);
            writer.Flush();
            stream.Position = 0;

            // Act
            var value = new StreamReader(stream).ReadToEnd();

            if (any.AnyType == AnyType.Primitive || any.AnyType == AnyType.Null)
            {
                return value;
            }

            return value.MakeLineBreaksEnvironmentNeutral();
        }
    }
}
