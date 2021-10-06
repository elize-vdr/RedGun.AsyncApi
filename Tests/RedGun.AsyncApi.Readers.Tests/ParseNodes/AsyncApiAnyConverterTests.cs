// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using FluentAssertions;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.ParseNodes;
using SharpYaml.Serialization;
using Xunit;

namespace RedGun.AsyncApi.Readers.Tests.ParseNodes
{
    [Collection("DefaultSettings")]
    public class AsyncApiAnyConverterTests
    {
        [Fact]
        public void ParseObjectAsAnyShouldSucceed()
        {
            var input = @"
aString: fooBar
aInteger: 10
aDouble: 2.34
aDateTime: 2017-01-01
aDate: 2017-01-02
                ";
            var yamlStream = new YamlStream();
            yamlStream.Load(new StringReader(input));
            var yamlNode = yamlStream.Documents.First().RootNode;

            var diagnostic = new AsyncApiDiagnostic();
            var context = new ParsingContext(diagnostic);

            var node = new MapNode(context, (YamlMappingNode)yamlNode);

            var anyMap = node.CreateAny();

            var schema = new AsyncApiSchema()
            {
                Type = "object",
                Properties =
                {
                    ["aString"] = new AsyncApiSchema()
                    {
                        Type = "string"
                    },
                    ["aInteger"] = new AsyncApiSchema()
                    {
                        Type = "integer",
                        Format = "int32"
                    },
                    ["aDouble"] = new AsyncApiSchema()
                    {
                        Type = "number",
                        Format = "double"
                    },
                    ["aDateTime"] = new AsyncApiSchema()
                    {
                        Type = "string",
                        Format = "date-time"
                    },
                    ["aDate"] = new AsyncApiSchema()
                    {
                        Type = "string",
                        Format = "date"
                    }
                }
            };

            anyMap = AsyncApiAnyConverter.GetSpecificAsyncApiAny(anyMap, schema);

            diagnostic.Errors.Should().BeEmpty();

            anyMap.Should().BeEquivalentTo(
                new AsyncApiObject
                {
                    ["aString"] = new AsyncApiString("fooBar"),
                    ["aInteger"] = new AsyncApiInteger(10),
                    ["aDouble"] = new AsyncApiDouble(2.34),
                    ["aDateTime"] = new AsyncApiDateTime(DateTimeOffset.Parse("2017-01-01", CultureInfo.InvariantCulture)),
                    ["aDate"] = new AsyncApiDate(DateTimeOffset.Parse("2017-01-02", CultureInfo.InvariantCulture).Date),
                });
        }


        [Fact]
        public void ParseNestedObjectAsAnyShouldSucceed()
        {
            var input = @"
    aString: fooBar
    aInteger: 10
    aArray:
      - 1
      - 2
      - 3
    aNestedArray:
      - aFloat: 1
        aPassword: 1234
        aArray: [abc, def]
        aDictionary:
          arbitraryProperty: 1
          arbitraryProperty2: 2
      - aFloat: 1.6
        aArray: [123]
        aDictionary:
          arbitraryProperty: 1
          arbitraryProperty3: 20
    aObject:
      aDate: 2017-02-03
    aDouble: 2.34
    aDateTime: 2017-01-01
                    ";
            var yamlStream = new YamlStream();
            yamlStream.Load(new StringReader(input));
            var yamlNode = yamlStream.Documents.First().RootNode;

            var diagnostic = new AsyncApiDiagnostic();
            var context = new ParsingContext(diagnostic);

            var node = new MapNode(context, (YamlMappingNode)yamlNode);

            var anyMap = node.CreateAny();

            var schema = new AsyncApiSchema()
            {
                Type = "object",
                Properties =
                    {
                        ["aString"] = new AsyncApiSchema()
                        {
                            Type = "string"
                        },
                        ["aInteger"] = new AsyncApiSchema()
                        {
                            Type = "integer",
                            Format = "int32"
                        },
                        ["aArray"] = new AsyncApiSchema()
                        {
                            Type = "array",
                            Items = new AsyncApiSchema()
                            {
                                Type = "integer",
                                Format = "int64"
                            }
                        },
                        ["aNestedArray"] = new AsyncApiSchema()
                        {
                            Type = "array",
                            Items = new AsyncApiSchema()
                            {
                                Type = "object",
                                Properties =
                                {
                                    ["aFloat"] = new AsyncApiSchema()
                                    {
                                        Type = "number",
                                        Format = "float"
                                    },
                                    ["aPassword"] = new AsyncApiSchema()
                                    {
                                        Type = "string",
                                        Format = "password"
                                    },
                                    ["aArray"] = new AsyncApiSchema()
                                    {
                                        Type = "array",
                                        Items = new AsyncApiSchema()
                                        {
                                            Type = "string",
                                        }
                                    },
                                    ["aDictionary"] = new AsyncApiSchema()
                                    {
                                        Type = "object",
                                        AdditionalProperties = new AsyncApiSchema()
                                        {
                                            Type = "integer",
                                            Format = "int64"
                                        }
                                    }
                                }
                            }
                        },
                        ["aObject"] = new AsyncApiSchema()
                        {
                            Type = "array",
                            Properties =
                            {
                                ["aDate"] = new AsyncApiSchema()
                                {
                                    Type = "string",
                                    Format = "date"
                                }
                            }
                        },
                        ["aDouble"] = new AsyncApiSchema()
                        {
                            Type = "number",
                            Format = "double"
                        },
                        ["aDateTime"] = new AsyncApiSchema()
                        {
                            Type = "string",
                            Format = "date-time"
                        }
                    }
            };

            anyMap = AsyncApiAnyConverter.GetSpecificAsyncApiAny(anyMap, schema);

            diagnostic.Errors.Should().BeEmpty();

            anyMap.Should().BeEquivalentTo(
                new AsyncApiObject
                {
                    ["aString"] = new AsyncApiString("fooBar"),
                    ["aInteger"] = new AsyncApiInteger(10),
                    ["aArray"] = new AsyncApiArray()
                    {
                        new AsyncApiLong(1),
                        new AsyncApiLong(2),
                        new AsyncApiLong(3),
                    },
                    ["aNestedArray"] = new AsyncApiArray()
                    {
                        new AsyncApiObject()
                        {
                            ["aFloat"] = new AsyncApiFloat(1),
                            ["aPassword"] = new AsyncApiPassword("1234"),
                            ["aArray"] = new AsyncApiArray()
                            {
                                new AsyncApiString("abc"),
                                new AsyncApiString("def")
                            },
                            ["aDictionary"] = new AsyncApiObject()
                            {
                                ["arbitraryProperty"] = new AsyncApiLong(1),
                                ["arbitraryProperty2"] = new AsyncApiLong(2),
                            }
                        },
                        new AsyncApiObject()
                        {
                            ["aFloat"] = new AsyncApiFloat((float)1.6),
                            ["aArray"] = new AsyncApiArray()
                            {
                                new AsyncApiString("123"),
                            },
                            ["aDictionary"] = new AsyncApiObject()
                            {
                                ["arbitraryProperty"] = new AsyncApiLong(1),
                                ["arbitraryProperty3"] = new AsyncApiLong(20),
                            }
                        }
                    },
                    ["aObject"] = new AsyncApiObject()
                    {
                        ["aDate"] = new AsyncApiDate(DateTimeOffset.Parse("2017-02-03", CultureInfo.InvariantCulture).Date)
                    },
                    ["aDouble"] = new AsyncApiDouble(2.34),
                    ["aDateTime"] = new AsyncApiDateTime(DateTimeOffset.Parse("2017-01-01", CultureInfo.InvariantCulture))
                });
        }


        [Fact]
        public void ParseNestedObjectAsAnyWithPartialSchemaShouldSucceed()
        {
            var input = @"
        aString: fooBar
        aInteger: 10
        aArray:
          - 1
          - 2
          - 3
        aNestedArray:
          - aFloat: 1
            aPassword: 1234
            aArray: [abc, def]
            aDictionary:
              arbitraryProperty: 1
              arbitraryProperty2: 2
          - aFloat: 1.6
            aArray: [123]
            aDictionary:
              arbitraryProperty: 1
              arbitraryProperty3: 20
        aObject:
          aDate: 2017-02-03
        aDouble: 2.34
        aDateTime: 2017-01-01
                        ";
            var yamlStream = new YamlStream();
            yamlStream.Load(new StringReader(input));
            var yamlNode = yamlStream.Documents.First().RootNode;

            var diagnostic = new AsyncApiDiagnostic();
            var context = new ParsingContext(diagnostic);

            var node = new MapNode(context, (YamlMappingNode)yamlNode);

            var anyMap = node.CreateAny();

            var schema = new AsyncApiSchema()
            {
                Type = "object",
                Properties =
                        {
                            ["aString"] = new AsyncApiSchema()
                            {
                                Type = "string"
                            },
                            ["aArray"] = new AsyncApiSchema()
                            {
                                Type = "array",
                                Items = new AsyncApiSchema()
                                {
                                    Type = "integer"
                                }
                            },
                            ["aNestedArray"] = new AsyncApiSchema()
                            {
                                Type = "array",
                                Items = new AsyncApiSchema()
                                {
                                    Type = "object",
                                    Properties =
                                    {
                                        ["aFloat"] = new AsyncApiSchema()
                                        {
                                        },
                                        ["aPassword"] = new AsyncApiSchema()
                                        {
                                        },
                                        ["aArray"] = new AsyncApiSchema()
                                        {
                                            Type = "array",
                                            Items = new AsyncApiSchema()
                                            {
                                                Type = "string",
                                            }
                                        }
                                    }
                                }
                            },
                            ["aObject"] = new AsyncApiSchema()
                            {
                                Type = "array",
                                Properties =
                                {
                                    ["aDate"] = new AsyncApiSchema()
                                    {
                                        Type = "string"
                                    }
                                }
                            },
                            ["aDouble"] = new AsyncApiSchema()
                            {
                            },
                            ["aDateTime"] = new AsyncApiSchema()
                            {
                            }
                        }
            };

            anyMap = AsyncApiAnyConverter.GetSpecificAsyncApiAny(anyMap, schema);

            diagnostic.Errors.Should().BeEmpty();

            anyMap.Should().BeEquivalentTo(
                new AsyncApiObject
                {
                    ["aString"] = new AsyncApiString("fooBar"),
                    ["aInteger"] = new AsyncApiInteger(10),
                    ["aArray"] = new AsyncApiArray()
                    {
                            new AsyncApiInteger(1),
                            new AsyncApiInteger(2),
                            new AsyncApiInteger(3),
                    },
                    ["aNestedArray"] = new AsyncApiArray()
                    {
                            new AsyncApiObject()
                            {
                                ["aFloat"] = new AsyncApiInteger(1),
                                ["aPassword"] = new AsyncApiInteger(1234),
                                ["aArray"] = new AsyncApiArray()
                                {
                                    new AsyncApiString("abc"),
                                    new AsyncApiString("def")
                                },
                                ["aDictionary"] = new AsyncApiObject()
                                {
                                    ["arbitraryProperty"] = new AsyncApiInteger(1),
                                    ["arbitraryProperty2"] = new AsyncApiInteger(2),
                                }
                            },
                            new AsyncApiObject()
                            {
                                ["aFloat"] = new AsyncApiDouble(1.6),
                                ["aArray"] = new AsyncApiArray()
                                {
                                    new AsyncApiString("123"),
                                },
                                ["aDictionary"] = new AsyncApiObject()
                                {
                                    ["arbitraryProperty"] = new AsyncApiInteger(1),
                                    ["arbitraryProperty3"] = new AsyncApiInteger(20),
                                }
                            }
                    },
                    ["aObject"] = new AsyncApiObject()
                    {
                        ["aDate"] = new AsyncApiString("2017-02-03")
                    },
                    ["aDouble"] = new AsyncApiDouble(2.34),
                    ["aDateTime"] = new AsyncApiDateTime(DateTimeOffset.Parse("2017-01-01", CultureInfo.InvariantCulture))
                });
        }

        [Fact]
        public void ParseNestedObjectAsAnyWithoutUsingSchemaShouldSucceed()
        {
            var input = @"
        aString: fooBar
        aInteger: 10
        aArray:
          - 1
          - 2
          - 3
        aNestedArray:
          - aFloat: 1
            aPassword: 1234
            aArray: [abc, def]
            aDictionary:
              arbitraryProperty: 1
              arbitraryProperty2: 2
          - aFloat: 1.6
            aArray: [123]
            aDictionary:
              arbitraryProperty: 1
              arbitraryProperty3: 20
        aObject:
          aDate: 2017-02-03
        aDouble: 2.34
        aDateTime: 2017-01-01
                        ";
            var yamlStream = new YamlStream();
            yamlStream.Load(new StringReader(input));
            var yamlNode = yamlStream.Documents.First().RootNode;

            var diagnostic = new AsyncApiDiagnostic();
            var context = new ParsingContext(diagnostic);

            var node = new MapNode(context, (YamlMappingNode)yamlNode);

            var anyMap = node.CreateAny();

            anyMap = AsyncApiAnyConverter.GetSpecificAsyncApiAny(anyMap);

            diagnostic.Errors.Should().BeEmpty();

            anyMap.Should().BeEquivalentTo(
                new AsyncApiObject
                {
                    ["aString"] = new AsyncApiString("fooBar"),
                    ["aInteger"] = new AsyncApiInteger(10),
                    ["aArray"] = new AsyncApiArray()
                    {
                            new AsyncApiInteger(1),
                            new AsyncApiInteger(2),
                            new AsyncApiInteger(3),
                    },
                    ["aNestedArray"] = new AsyncApiArray()
                    {
                            new AsyncApiObject()
                            {
                                ["aFloat"] = new AsyncApiInteger(1),
                                ["aPassword"] = new AsyncApiInteger(1234),
                                ["aArray"] = new AsyncApiArray()
                                {
                                    new AsyncApiString("abc"),
                                    new AsyncApiString("def")
                                },
                                ["aDictionary"] = new AsyncApiObject()
                                {
                                    ["arbitraryProperty"] = new AsyncApiInteger(1),
                                    ["arbitraryProperty2"] = new AsyncApiInteger(2),
                                }
                            },
                            new AsyncApiObject()
                            {
                                ["aFloat"] = new AsyncApiDouble(1.6),
                                ["aArray"] = new AsyncApiArray()
                                {
                                    new AsyncApiInteger(123),
                                },
                                ["aDictionary"] = new AsyncApiObject()
                                {
                                    ["arbitraryProperty"] = new AsyncApiInteger(1),
                                    ["arbitraryProperty3"] = new AsyncApiInteger(20),
                                }
                            }
                    },
                    ["aObject"] = new AsyncApiObject()
                    {
                        ["aDate"] = new AsyncApiDateTime(DateTimeOffset.Parse("2017-02-03", CultureInfo.InvariantCulture))
                    },
                    ["aDouble"] = new AsyncApiDouble(2.34),
                    ["aDateTime"] = new AsyncApiDateTime(DateTimeOffset.Parse("2017-01-01", CultureInfo.InvariantCulture))
                });
        }
    }
}
