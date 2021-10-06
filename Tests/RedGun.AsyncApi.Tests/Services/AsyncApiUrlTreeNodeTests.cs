// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Services;
using Xunit;

namespace RedGun.AsyncApi.Tests.Services
{
    public class AsyncApiUrlTreeNodeTests
    {
        private AsyncApiDocument AsyncApiDocumentSample_1 => new AsyncApiDocument()
        {
            Paths = new AsyncApiPaths()
            {
                ["/"] = new AsyncApiPathItem(),
                ["/houses"] = new AsyncApiPathItem(),
                ["/cars"] = new AsyncApiPathItem()
            }
        };

        private AsyncApiDocument AsyncApiDocumentSample_2 => new AsyncApiDocument()
        {
            Paths = new AsyncApiPaths()
            {
                ["/"] = new AsyncApiPathItem(),
                ["/hotels"] = new AsyncApiPathItem(),
                ["/offices"] = new AsyncApiPathItem()
            }
        };

        [Fact]
        public void CreateUrlSpaceWithoutAsyncApiDocument()
        {
            var rootNode = AsyncApiUrlTreeNode.Create();

            Assert.NotNull(rootNode);
        }

        [Fact]
        public void CreateSingleRootWorks()
        {
            var doc = new AsyncApiDocument()
            {
                Paths = new AsyncApiPaths()
                {
                    ["/"] = new AsyncApiPathItem()
                }
            };

            var label = "v1.0";
            var rootNode = AsyncApiUrlTreeNode.Create(doc, label);

            Assert.NotNull(rootNode);
            Assert.NotNull(rootNode.PathItems);
            Assert.False(rootNode.HasOperations(label));
            Assert.Equal(0, rootNode.Children.Count);
        }

        [Fact]
        public void CreatePathWithoutRootWorks()
        {
            var doc = new AsyncApiDocument()
            {
                Paths = new AsyncApiPaths()
                {
                    ["/houses"] = new AsyncApiPathItem()
                }
            };

            var label = "cabin";
            var rootNode = AsyncApiUrlTreeNode.Create(doc, label);

            Assert.NotNull(rootNode);
            Assert.NotNull(rootNode.PathItems);
            Assert.Equal(1, rootNode.Children.Count);
            Assert.Equal("houses", rootNode.Children["houses"].Segment);
            Assert.NotNull(rootNode.Children["houses"].PathItems);
            Assert.False(rootNode.Children["houses"].HasOperations("cabin"));
        }

        [Fact]
        public void CreateMultiplePathsWorks()
        {
            var doc = AsyncApiDocumentSample_1;

            string label = "assets";
            var rootNode = AsyncApiUrlTreeNode.Create(doc, label);

            Assert.NotNull(rootNode);
            Assert.Equal(2, rootNode.Children.Count);
            Assert.Equal("houses", rootNode.Children["houses"].Segment);
            Assert.Equal("cars", rootNode.Children["cars"].Segment);
            Assert.True(rootNode.PathItems.ContainsKey(label));
            Assert.True(rootNode.Children["houses"].PathItems.ContainsKey(label));
            Assert.True(rootNode.Children["cars"].PathItems.ContainsKey(label));
        }

        [Fact]
        public void AttachDocumentWorks()
        {
            var doc1 = AsyncApiDocumentSample_1;

            var doc2 = AsyncApiDocumentSample_2;

            var label1 = "personal";
            var label2 = "business";
            var rootNode = AsyncApiUrlTreeNode.Create(doc1, label1);
            rootNode.Attach(doc2, label2);

            Assert.NotNull(rootNode);
            Assert.Equal(4, rootNode.Children.Count);
            Assert.True(rootNode.Children["houses"].PathItems.ContainsKey(label1));
            Assert.True(rootNode.Children["offices"].PathItems.ContainsKey(label2));
        }

        [Fact]
        public void AttachPathWorks()
        {
            var doc = AsyncApiDocumentSample_1;

            var label1 = "personal";
            var rootNode = AsyncApiUrlTreeNode.Create(doc, label1);

            var pathItem1 = new AsyncApiPathItem
            {
                Operations = new Dictionary<OperationType, AsyncApiOperation>
                {
                    {
                        OperationType.Get, new AsyncApiOperation
                        {
                            OperationId = "motorcycles.ListMotorcycle",
                            Responses = new AsyncApiResponses()
                            {
                                {
                                    "200", new AsyncApiResponse()
                                    {
                                        Description = "Retrieved entities"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var path1 = "/motorcycles";
            rootNode.Attach(path1, pathItem1, label1);

            var pathItem2 = new AsyncApiPathItem
            {
                Operations = new Dictionary<OperationType, AsyncApiOperation>
                {
                    {
                        OperationType.Get, new AsyncApiOperation
                        {
                            OperationId = "computers.ListComputer",
                            Responses = new AsyncApiResponses()
                            {
                                {
                                    "200", new AsyncApiResponse()
                                    {
                                        Description = "Retrieved entities"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var path2 = "/computers";
            var label2 = "business";
            rootNode.Attach(path2, pathItem2, label2);

            Assert.Equal(4, rootNode.Children.Count);
            Assert.True(rootNode.Children.ContainsKey(path1.Substring(1)));
            Assert.True(rootNode.Children.ContainsKey(path2.Substring(1)));
            Assert.True(rootNode.Children[path1.Substring(1)].PathItems.ContainsKey(label1));
            Assert.True(rootNode.Children[path2.Substring(1)].PathItems.ContainsKey(label2));
        }

        [Fact]
        public void CreatePathsWithMultipleSegmentsWorks()
        {
            var doc = new AsyncApiDocument()
            {
                Paths = new AsyncApiPaths()
                {
                    ["/"] = new AsyncApiPathItem(),
                    ["/houses/apartments/{apartment-id}"] = new AsyncApiPathItem(),
                    ["/cars/coupes"] = new AsyncApiPathItem()
                }
            };

            var label = "assets";
            var rootNode = AsyncApiUrlTreeNode.Create(doc, label);

            Assert.NotNull(rootNode);
            Assert.Equal(2, rootNode.Children.Count);
            Assert.NotNull(rootNode.Children["houses"].Children["apartments"].Children["{apartment-id}"].PathItems);
            Assert.True(rootNode.Children["houses"].Children["apartments"].Children["{apartment-id}"].PathItems.ContainsKey(label));
            Assert.True(rootNode.Children["cars"].Children["coupes"].PathItems.ContainsKey(label));
            Assert.True(rootNode.PathItems.ContainsKey(label));
            Assert.Equal("coupes", rootNode.Children["cars"].Children["coupes"].Segment);
        }

        [Fact]
        public void HasOperationsWorks()
        {
            var doc1 = new AsyncApiDocument()
            {
                Paths = new AsyncApiPaths()
                {
                    ["/"] = new AsyncApiPathItem(),
                    ["/houses"] = new AsyncApiPathItem(),
                    ["/cars/{car-id}"] = new AsyncApiPathItem()
                    {
                        Operations = new Dictionary<OperationType, AsyncApiOperation>
                        {
                            {
                                OperationType.Get, new AsyncApiOperation
                                {
                                    OperationId = "cars.GetCar",
                                    Responses = new AsyncApiResponses()
                                    {
                                        {
                                            "200", new AsyncApiResponse()
                                            {
                                                Description = "Retrieved entity"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var doc2 = new AsyncApiDocument()
            {
                Paths = new AsyncApiPaths()
                {
                    ["/cars/{car-id}"] = new AsyncApiPathItem()
                    {
                        Operations = new Dictionary<OperationType, AsyncApiOperation>
                        {
                            {
                                OperationType.Get, new AsyncApiOperation
                                {
                                    OperationId = "cars.GetCar",
                                    Responses = new AsyncApiResponses()
                                    {
                                        {
                                            "200", new AsyncApiResponse()
                                            {
                                                Description = "Retrieved entity"
                                            }
                                        }
                                    }
                                }
                            },
                            {
                                OperationType.Put, new AsyncApiOperation
                                {
                                    OperationId = "cars.UpdateCar",
                                    Responses = new AsyncApiResponses()
                                    {
                                        {
                                            "204", new AsyncApiResponse()
                                            {
                                                Description = "Success."
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var label1 = "personal";
            var label2 = "business";
            var rootNode = AsyncApiUrlTreeNode.Create(doc1, label1);
            rootNode.Attach(doc2, label2);

            Assert.NotNull(rootNode);

            Assert.Equal(2, rootNode.Children["cars"].Children["{car-id}"].PathItems.Count);
            Assert.True(rootNode.Children["cars"].Children["{car-id}"].PathItems.ContainsKey(label1));
            Assert.True(rootNode.Children["cars"].Children["{car-id}"].PathItems.ContainsKey(label2));

            Assert.False(rootNode.Children["houses"].HasOperations(label1));
            Assert.True(rootNode.Children["cars"].Children["{car-id}"].HasOperations(label1));
            Assert.True(rootNode.Children["cars"].Children["{car-id}"].HasOperations(label2));
            Assert.Single(rootNode.Children["cars"].Children["{car-id}"].PathItems[label1].Operations);
            Assert.Equal(2, rootNode.Children["cars"].Children["{car-id}"].PathItems[label2].Operations.Count);
        }

        [Fact]
        public void SegmentIsParameterWorks()
        {
            var doc = new AsyncApiDocument()
            {
                Paths = new AsyncApiPaths()
                {
                    ["/"] = new AsyncApiPathItem(),
                    ["/houses/apartments/{apartment-id}"] = new AsyncApiPathItem()
                }
            };

            var label = "properties";
            var rootNode = AsyncApiUrlTreeNode.Create(doc, label);

            Assert.NotNull(rootNode);
            Assert.Equal(1, rootNode.Children.Count);
            Assert.NotNull(rootNode.Children["houses"].Children["apartments"].Children["{apartment-id}"].PathItems);
            Assert.True(rootNode.Children["houses"].Children["apartments"].Children["{apartment-id}"].IsParameter);
            Assert.Equal("{apartment-id}", rootNode.Children["houses"].Children["apartments"].Children["{apartment-id}"].Segment);
        }

        [Fact]
        public void AdditionalDataWorks()
        {
            var doc = AsyncApiDocumentSample_1;

            var label = "personal";
            var rootNode = AsyncApiUrlTreeNode.Create(doc, label);

            var additionalData1 = new Dictionary<string, List<string>>
            {
                {"DatePurchased", new List<string> { "21st April 2021" } },
                {"Location", new List<string> { "Seattle, WA" } },
                {"TotalEstimateValue", new List<string> { "USD 2 Million" } },
                {"Owner", new List<string> { "John Doe, Mr" } }
            };
            rootNode.AddAdditionalData(additionalData1);

            var additionalData2 = new Dictionary<string, List<string>>
            {
                {"Bedrooms", new List<string> { "Five" } }
            };
            rootNode.Children["houses"].AddAdditionalData(additionalData2);

            var additionalData3 = new Dictionary<string, List<string>>
            {
                {"Categories", new List<string> { "Coupe", "Sedan", "Convertible" } }
            };
            rootNode.Children["cars"].AddAdditionalData(additionalData3);

            Assert.Equal(4, rootNode.AdditionalData.Count);
            Assert.True(rootNode.AdditionalData.ContainsKey("DatePurchased"));
            Assert.Collection(rootNode.AdditionalData["Location"],
                item =>
                {
                    Assert.Equal("Seattle, WA", item);
                });
            Assert.Collection(rootNode.Children["houses"].AdditionalData["Bedrooms"],
                item =>
                {
                    Assert.Equal("Five", item);
                });
            Assert.Collection(rootNode.Children["cars"].AdditionalData["Categories"],
                item =>
                {
                    Assert.Equal("Coupe", item);
                },
                item =>
                {
                    Assert.Equal("Sedan", item);
                },
                item =>
                {
                    Assert.Equal("Convertible", item);
                });
        }

        [Fact]
        public void ThrowsArgumentExceptionForDuplicateLabels()
        {
            var doc1 = AsyncApiDocumentSample_1;

            var doc2 = AsyncApiDocumentSample_2;

            var label1 = "personal";
            var rootNode = AsyncApiUrlTreeNode.Create(doc1, label1);

            Assert.Throws<ArgumentException>(() => rootNode.Attach(doc2, label1));
        }

        [Fact]
        public void ThrowsArgumentNullExceptionForNullOrEmptyArgumentsInCreateMethod()
        {
            var doc = AsyncApiDocumentSample_1;

            Assert.Throws<ArgumentNullException>(() => AsyncApiUrlTreeNode.Create(doc, ""));
            Assert.Throws<ArgumentNullException>(() => AsyncApiUrlTreeNode.Create(doc, null));
            Assert.Throws<ArgumentNullException>(() => AsyncApiUrlTreeNode.Create(null, "beta"));
            Assert.Throws<ArgumentNullException>(() => AsyncApiUrlTreeNode.Create(null, null));
            Assert.Throws<ArgumentNullException>(() => AsyncApiUrlTreeNode.Create(null, ""));
        }

        [Fact]
        public void ThrowsArgumentNullExceptionForNullOrEmptyArgumentsInAttachMethod()
        {
            var doc1 = AsyncApiDocumentSample_1;

            var doc2 = AsyncApiDocumentSample_2;

            var label1 = "personal";
            var rootNode = AsyncApiUrlTreeNode.Create(doc1, label1);

            Assert.Throws<ArgumentNullException>(() => rootNode.Attach(doc2, ""));
            Assert.Throws<ArgumentNullException>(() => rootNode.Attach(doc2, null));
            Assert.Throws<ArgumentNullException>(() => rootNode.Attach(null, "beta"));
            Assert.Throws<ArgumentNullException>(() => rootNode.Attach(null, null));
            Assert.Throws<ArgumentNullException>(() => rootNode.Attach(null, ""));
        }

        [Fact]
        public void ThrowsArgumentNullExceptionForNullOrEmptyArgumentInHasOperationsMethod()
        {
            var doc = AsyncApiDocumentSample_1;

            var label = "personal";
            var rootNode = AsyncApiUrlTreeNode.Create(doc, label);

            Assert.Throws<ArgumentNullException>(() => rootNode.HasOperations(null));
            Assert.Throws<ArgumentNullException>(() => rootNode.HasOperations(""));
        }

        [Fact]
        public void ThrowsArgumentNullExceptionForNullArgumentInAddAdditionalDataMethod()
        {
            var doc = AsyncApiDocumentSample_1;

            var label = "personal";
            var rootNode = AsyncApiUrlTreeNode.Create(doc, label);

            Assert.Throws<ArgumentNullException>(() => rootNode.AddAdditionalData(null));
        }
    }
}
