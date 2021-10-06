// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.IO;
using System.Linq;
using FluentAssertions;
using RedGun.AsyncApi.Expressions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.ParseNodes;
using RedGun.AsyncApi.Readers.V2;
using SharpYaml.Serialization;
using Xunit;

namespace RedGun.AsyncApi.Readers.Tests.V2Tests
{
    [Collection("DefaultSettings")]
    public class AsyncApiCallbackTests
    {
        private const string SampleFolderPath = "V2Tests/Samples/AsyncApiCallback/";

        [Fact]
        public void ParseBasicCallbackShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "basicCallback.yaml")))
            {
                // Arrange
                var yamlStream = new YamlStream();
                yamlStream.Load(new StreamReader(stream));
                var yamlNode = yamlStream.Documents.First().RootNode;

                var diagnostic = new AsyncApiDiagnostic();
                var context = new ParsingContext(diagnostic);

                var node = new MapNode(context, (YamlMappingNode)yamlNode);

                // Act
                var callback = AsyncApiV2Deserializer.LoadCallback(node);

                // Assert
                diagnostic.Should().BeEquivalentTo(new AsyncApiDiagnostic());

                callback.Should().BeEquivalentTo(
                    new AsyncApiCallback
                    {
                        PathItems =
                        {
                            [RuntimeExpression.Build("$request.body#/url")]
                            = new AsyncApiPathItem
                            {
                                Operations =
                                {
                                    [OperationType.Post] =
                                    new AsyncApiOperation
                                    {
                                        RequestBody = new AsyncApiRequestBody
                                        {
                                            Content =
                                            {
                                                ["application/json"] = null
                                            }
                                        },
                                        Responses = new AsyncApiResponses
                                        {
                                            ["200"] = new AsyncApiResponse
                                            {
                                                Description = "Success"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    });
            }
        }

        [Fact]
        public void ParseCallbackWithReferenceShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "callbackWithReference.yaml")))
            {
                // Act
                var openApiDoc = new AsyncApiStreamReader().Read(stream, out var diagnostic);

                // Assert
                var path = openApiDoc.Paths.First().Value;
                var subscribeOperation = path.Operations[OperationType.Post];

                var callback = subscribeOperation.Callbacks["simpleHook"];

                diagnostic.Should().BeEquivalentTo(
                    new AsyncApiDiagnostic() { SpecificationVersion = AsyncApiSpecVersion.AsyncApi2_0 });

                callback.Should().BeEquivalentTo(
                    new AsyncApiCallback
                    {
                        PathItems =
                        {
                            [RuntimeExpression.Build("$request.body#/url")]= new AsyncApiPathItem {
                                Operations = {
                                    [OperationType.Post] = new AsyncApiOperation()
                                    {
                                        RequestBody = new AsyncApiRequestBody
                                        {
                                            Content =
                                            {
                                                ["application/json"] = new AsyncApiMediaType
                                                {
                                                    Schema = new AsyncApiSchema()
                                                    {
                                                        Type = "object"
                                                    }
                                                }
                                            }
                                        },
                                        Responses = {
                                            ["200"]= new AsyncApiResponse
                                            {
                                                Description = "Success"
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        Reference = new AsyncApiReference
                        {
                            Type = ReferenceType.Callback,
                            Id = "simpleHook",
                        }
                    });
            }
        }

        [Fact]
        public void ParseMultipleCallbacksWithReferenceShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "multipleCallbacksWithReference.yaml")))
            {
                // Act
                var openApiDoc = new AsyncApiStreamReader().Read(stream, out var diagnostic);

                // Assert
                var path = openApiDoc.Paths.First().Value;
                var subscribeOperation = path.Operations[OperationType.Post];

                diagnostic.Should().BeEquivalentTo(
                    new AsyncApiDiagnostic() { SpecificationVersion = AsyncApiSpecVersion.AsyncApi2_0 });

                var callback1 = subscribeOperation.Callbacks["simpleHook"];

                callback1.Should().BeEquivalentTo(
                    new AsyncApiCallback
                    {
                        PathItems =
                        {
                            [RuntimeExpression.Build("$request.body#/url")]= new AsyncApiPathItem {
                                Operations = {
                                    [OperationType.Post] = new AsyncApiOperation()
                                    {
                                        RequestBody = new AsyncApiRequestBody
                                        {
                                            Content =
                                            {
                                                ["application/json"] = new AsyncApiMediaType
                                                {
                                                    Schema = new AsyncApiSchema()
                                                    {
                                                        Type = "object"
                                                    }
                                                }
                                            }
                                        },
                                        Responses = {
                                            ["200"]= new AsyncApiResponse
                                            {
                                                Description = "Success"
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        Reference = new AsyncApiReference
                        {
                            Type = ReferenceType.Callback,
                            Id = "simpleHook",
                        }
                    });

                var callback2 = subscribeOperation.Callbacks["callback2"];
                callback2.Should().BeEquivalentTo(
                    new AsyncApiCallback
                    {
                        PathItems =
                        {
                            [RuntimeExpression.Build("/simplePath")]= new AsyncApiPathItem {
                                Operations = {
                                    [OperationType.Post] = new AsyncApiOperation()
                                    {
                                        RequestBody = new AsyncApiRequestBody
                                        {
                                            Description = "Callback 2",
                                            Content =
                                            {
                                                ["application/json"] = new AsyncApiMediaType
                                                {
                                                    Schema = new AsyncApiSchema()
                                                    {
                                                        Type = "string"
                                                    }
                                                }
                                            }
                                        },
                                        Responses = {
                                            ["400"]= new AsyncApiResponse
                                            {
                                                Description = "Callback Response"
                                            }
                                        }
                                    }
                                },
                            }
                        }
                    });

                var callback3 = subscribeOperation.Callbacks["callback3"];
                callback3.Should().BeEquivalentTo(
                    new AsyncApiCallback
                    {
                        PathItems =
                        {
                            [RuntimeExpression.Build(@"http://example.com?transactionId={$request.body#/id}&email={$request.body#/email}")] = new AsyncApiPathItem {
                                Operations = {
                                    [OperationType.Post] = new AsyncApiOperation()
                                    {
                                        RequestBody = new AsyncApiRequestBody
                                        {
                                            Content =
                                            {
                                                ["application/xml"] = new AsyncApiMediaType
                                                {
                                                    Schema = new AsyncApiSchema()
                                                    {
                                                        Type = "object"
                                                    }
                                                }
                                            }
                                        },
                                        Responses = {
                                            ["200"]= new AsyncApiResponse
                                            {
                                                Description = "Success"
                                            },
                                            ["401"]= new AsyncApiResponse
                                            {
                                                Description = "Unauthorized"
                                            },
                                            ["404"]= new AsyncApiResponse
                                            {
                                                Description = "Not Found"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    });
            }
        }
    }
}
