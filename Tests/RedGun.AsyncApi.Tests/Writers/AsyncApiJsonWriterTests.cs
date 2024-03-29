﻿// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FluentAssertions;
using RedGun.AsyncApi.Writers;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace RedGun.AsyncApi.Tests.Writers
{
    [Collection("DefaultSettings")]
    public class AsyncApiJsonWriterTests
    {
        private readonly ITestOutputHelper _output;

        public AsyncApiJsonWriterTests(ITestOutputHelper output)
        {
            _output = output;
        }

        public static IEnumerable<object[]> WriteStringListAsJsonShouldMatchExpectedTestCases()
        {
            yield return new object[]
            {
                new[]
                {
                    "string1",
                    "string2",
                    "string3",
                    "string4",
                    "string5",
                    "string6",
                    "string7",
                    "string8"
                }
            };

            yield return new object[]
            {
                new[] {"string1", "string1", "string1", "string1"}
            };
        }

        [Theory]
        [MemberData(nameof(WriteStringListAsJsonShouldMatchExpectedTestCases))]
        public void WriteStringListAsJsonShouldMatchExpected(string[] stringValues)
        {
            // Arrange
            var outputString = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputString);

            // Act
            writer.WriteStartArray();
            foreach (var stringValue in stringValues)
            {
                writer.WriteValue(stringValue);
            }

            writer.WriteEndArray();
            writer.Flush();

            var parsedObject = JsonConvert.DeserializeObject(outputString.GetStringBuilder().ToString());
            var expectedObject =
                JsonConvert.DeserializeObject(JsonConvert.SerializeObject(new List<string>(stringValues)));

            // Assert
            parsedObject.Should().BeEquivalentTo(expectedObject);
        }

        public static IEnumerable<object[]> WriteMapAsJsonShouldMatchExpectedTestCasesSimple()
        {
            // Simple map
            yield return new object[]
            {
                new Dictionary<string, object>
                {
                    ["property1"] = "value1",
                    ["property2"] = "value2",
                    ["property3"] = "value3",
                    ["property4"] = "value4"
                }
            };

            // Simple map with duplicate values
            yield return new object[]
            {
                new Dictionary<string, object>
                {
                    ["property1"] = "value1",
                    ["property2"] = "value1",
                    ["property3"] = "value1",
                    ["property4"] = "value1"
                }
            };
        }

        public static IEnumerable<object[]> WriteMapAsJsonShouldMatchExpectedTestCasesComplex()
        {
            // Empty map and empty list
            yield return new object[]
            {
                new Dictionary<string, object>
                {
                    ["property1"] = new Dictionary<string, object>(),
                    ["property2"] = new List<string>(),
                    ["property3"] = new List<object>
                    {
                        new Dictionary<string, object>(),
                    },
                    ["property4"] = "value4"
                }
            };

            // Number, boolean, and null handling
            yield return new object[]
            {
                new Dictionary<string, object>
                {
                    ["property1"] = "10.0",
                    ["property2"] = "10",
                    ["property3"] = "-5",
                    ["property4"] = 10.0M,
                    ["property5"] = 10,
                    ["property6"] = -5,
                    ["property7"] = true,
                    ["property8"] = "true",
                    ["property9"] = null,
                    ["property10"] = "null",
                    ["property11"] = "",
                }
            };

            // DateTime
            yield return new object[]
            {
                new Dictionary<string, object>
                {
                    ["property1"] = new DateTime(1970, 01, 01),
                    ["property2"] = new DateTimeOffset(new DateTime(1970, 01, 01)),
                    ["property3"] = new DateTime(2018, 04, 03),
                }
            };

            // Nested map
            yield return new object[]
            {
                new Dictionary<string, object>
                {
                    ["property1"] = new Dictionary<string, object>
                    {
                        ["innerProperty1"] = "innerValue1"
                    },
                    ["property2"] = "value2",
                    ["property3"] = new Dictionary<string, object>
                    {
                        ["innerProperty3"] = "innerValue3"
                    },
                    ["property4"] = "value4"
                }
            };

            // Nested map and list
            yield return new object[]
            {
                new Dictionary<string, object>
                {
                    ["property1"] = new Dictionary<string, object>(),
                    ["property2"] = new List<string>(),
                    ["property3"] = new List<object>
                    {
                        new Dictionary<string, object>(),
                        "string1",
                        new Dictionary<string, object>
                        {
                            ["innerProperty1"] = new List<object>(),
                            ["innerProperty2"] = "string2",
                            ["innerProperty3"] = new List<object>
                            {
                                new List<string>
                                {
                                    "string3"
                                }
                            }
                        }
                    },
                    ["property4"] = "value4"
                }
            };
        }

        private void WriteValueRecursive(AsyncApiJsonWriter writer, object value)
        {
            if (value == null
                || value.GetType().IsPrimitive
                || value is decimal
                || value is string
                || value is DateTimeOffset
                || value is DateTime)
            {
                writer.WriteValue(value);
            }
            else if (value.GetType().IsGenericType &&
                (typeof(IDictionary<,>).IsAssignableFrom(value.GetType().GetGenericTypeDefinition()) ||
                    typeof(Dictionary<,>).IsAssignableFrom(value.GetType().GetGenericTypeDefinition())))
            {
                writer.WriteStartObject();
                foreach (var elementValue in (dynamic)(value))
                {
                    writer.WritePropertyName(elementValue.Key);
                    WriteValueRecursive(writer, elementValue.Value);
                }

                writer.WriteEndObject();
            }
            else if (typeof(IEnumerable).IsAssignableFrom(value.GetType()))
            {
                writer.WriteStartArray();
                foreach (var elementValue in (IEnumerable)value)
                {
                    WriteValueRecursive(writer, elementValue);
                }

                writer.WriteEndArray();
            }
        }

        [Theory]
        [MemberData(nameof(WriteMapAsJsonShouldMatchExpectedTestCasesSimple))]
        [MemberData(nameof(WriteMapAsJsonShouldMatchExpectedTestCasesComplex))]
        public void WriteMapAsJsonShouldMatchExpected(IDictionary<string, object> inputMap)
        {
            // Arrange
            var outputString = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputString);

            // Act
            WriteValueRecursive(writer, inputMap);

            var parsedObject = JsonConvert.DeserializeObject(outputString.GetStringBuilder().ToString());
            var expectedObject = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(inputMap));

            // Assert
            parsedObject.Should().BeEquivalentTo(expectedObject);
        }

        public static IEnumerable<object[]> WriteDateTimeAsJsonTestCases()
        {
            yield return new object[]
            {
                new DateTimeOffset(2018, 1, 1, 10, 20, 30, TimeSpan.Zero),
            };

            yield return new object[]
            {
                new DateTimeOffset(2018, 1, 1, 10, 20, 30, 100, TimeSpan.FromHours(14)),
            };

            yield return new object[]
            {
                DateTimeOffset.UtcNow + TimeSpan.FromDays(4)
            };

            yield return new object[]
            {
                DateTime.UtcNow + TimeSpan.FromDays(4)
            };
        }

        [Theory]
        [MemberData(nameof(WriteDateTimeAsJsonTestCases))]
        public void WriteDateTimeAsJsonShouldMatchExpected(DateTimeOffset dateTimeOffset)
        {
            // Arrange
            var outputString = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputString);

            // Act
            writer.WriteValue(dateTimeOffset);

            var writtenString = outputString.GetStringBuilder().ToString();
            var expectedString = JsonConvert.SerializeObject(dateTimeOffset, new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffffffK",
            });

            // Assert
            writtenString.Should().Be(expectedString);
        }
    }
}
