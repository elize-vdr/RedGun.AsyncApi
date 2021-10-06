// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Services;
using Xunit;

namespace RedGun.AsyncApi.Tests.Walkers
{
    public class WalkerLocationTests
    {

        [Fact]
        public void LocateTopLevelObjects()
        {
            var doc = new AsyncApiDocument();

            var locator = new LocatorVisitor();
            var walker = new AsyncApiWalker(locator);
            walker.Walk(doc);

            locator.Locations.Should().BeEquivalentTo(new List<string> {
                "#/servers",
                "#/tags"
            });
        }

        [Fact]
        public void LocateTopLevelArrayItems()
        {
            var doc = new AsyncApiDocument()
            {
                Servers = new List<AsyncApiServer>() {
                    new AsyncApiServer(),
                    new AsyncApiServer()
                },
                Paths = new AsyncApiPaths(),
                Tags = new List<AsyncApiTag>()
                {
                    new AsyncApiTag()
                }
            };

            var locator = new LocatorVisitor();
            var walker = new AsyncApiWalker(locator);
            walker.Walk(doc);

            locator.Locations.Should().BeEquivalentTo(new List<string> {
                "#/servers",
                "#/servers/0",
                "#/servers/1",
                "#/paths",
                "#/tags",
                "#/tags/0"
            });
        }

        [Fact]
        public void LocatePathOperationContentSchema()
        {
            var doc = new AsyncApiDocument
            {
                Paths = new AsyncApiPaths()
            };
            doc.Paths.Add("/test", new AsyncApiPathItem()
            {
                Operations = new Dictionary<OperationType, AsyncApiOperation>()
                {
                    [OperationType.Get] = new AsyncApiOperation()
                    {
                        Responses = new AsyncApiResponses()
                        {
                            ["200"] = new AsyncApiResponse()
                            {
                                Content = new Dictionary<string, AsyncApiMediaType>
                                {
                                    ["application/json"] = new AsyncApiMediaType
                                    {
                                        Schema = new AsyncApiSchema
                                        {
                                            Type = "string"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            });

            var locator = new LocatorVisitor();
            var walker = new AsyncApiWalker(locator);
            walker.Walk(doc);

            locator.Locations.Should().BeEquivalentTo(new List<string> {
                "#/servers",
                "#/paths",
                "#/paths/~1test",
                "#/paths/~1test/get",
                "#/paths/~1test/get/responses",
                "#/paths/~1test/get/responses/200",
                "#/paths/~1test/get/responses/200/content",
                "#/paths/~1test/get/responses/200/content/application~1json",
                "#/paths/~1test/get/responses/200/content/application~1json/schema",
                "#/paths/~1test/get/tags",
                "#/tags",

            });

            locator.Keys.Should().BeEquivalentTo(new List<string> { "/test", "Get", "200", "application/json" });
        }

        [Fact]
        public void WalkDOMWithCycles()
        {
            var loopySchema = new AsyncApiSchema()
            {
                Type = "object",
                Properties = new Dictionary<string, AsyncApiSchema>()
                {
                    ["name"] = new AsyncApiSchema() { Type = "string" }
                }
            };

            loopySchema.Properties.Add("parent", loopySchema);

            var doc = new AsyncApiDocument()
            {
                Paths = new AsyncApiPaths(),
                Components = new AsyncApiComponents()
                {
                    Schemas = new Dictionary<string, AsyncApiSchema>
                    {
                        ["loopy"] = loopySchema
                    }
                }
            };

            var locator = new LocatorVisitor();
            var walker = new AsyncApiWalker(locator);
            walker.Walk(doc);

            locator.Locations.Should().BeEquivalentTo(new List<string> {
                "#/servers",
                "#/paths",
                "#/components",
                "#/components/schemas/loopy",
                "#/components/schemas/loopy/properties/name",
                "#/tags"
            });
        }

        /// <summary>
        /// Walk document and discover all references to components, including those inside components
        /// </summary>
        [Fact]
        public void LocateReferences()
        {

            var baseSchema = new AsyncApiSchema()
            {
                Reference = new AsyncApiReference()
                {
                    Id = "base",
                    Type = ReferenceType.Schema
                },
                UnresolvedReference = false
            };

            var derivedSchema = new AsyncApiSchema
            {
                AnyOf = new List<AsyncApiSchema>() { baseSchema },
                Reference = new AsyncApiReference()
                {
                    Id = "derived",
                    Type = ReferenceType.Schema
                },
                UnresolvedReference = false
            };

            var testHeader = new AsyncApiHeader()
            {
                Schema = derivedSchema,
                Reference = new AsyncApiReference()
                {
                    Id = "test-header",
                    Type = ReferenceType.Header
                },
                UnresolvedReference = false
            };

            var doc = new AsyncApiDocument
            {
                Paths = new AsyncApiPaths()
                {
                    ["/"] = new AsyncApiPathItem()
                    {
                        Operations = new Dictionary<OperationType, AsyncApiOperation>()
                        {
                            [OperationType.Get] = new AsyncApiOperation()
                            {
                                Responses = new AsyncApiResponses()
                                {
                                    ["200"] = new AsyncApiResponse()
                                    {
                                        Content = new Dictionary<string, AsyncApiMediaType>()
                                        {
                                            ["application/json"] = new AsyncApiMediaType()
                                            {
                                                Schema = derivedSchema
                                            }
                                        },
                                        Headers = new Dictionary<string, AsyncApiHeader>()
                                        {
                                            ["test-header"] = testHeader
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                Components = new AsyncApiComponents()
                {
                    Schemas = new Dictionary<string, AsyncApiSchema>()
                    {
                        ["derived"] = derivedSchema,
                        ["base"] = baseSchema,
                    },
                    Headers = new Dictionary<string, AsyncApiHeader>()
                    {
                        ["test-header"] = testHeader
                    }
                }
            };

            var locator = new LocatorVisitor();
            var walker = new AsyncApiWalker(locator);
            walker.Walk(doc);

            locator.Locations.Where(l => l.StartsWith("referenceAt:")).Should().BeEquivalentTo(new List<string> {
                "referenceAt: #/paths/~1/get/responses/200/content/application~1json/schema",
                "referenceAt: #/paths/~1/get/responses/200/headers/test-header",
                "referenceAt: #/components/schemas/derived/anyOf/0",
                "referenceAt: #/components/headers/test-header/schema"
            });
        }
    }

    internal class LocatorVisitor : AsyncApiVisitorBase
    {
        public List<string> Locations = new List<string>();
        public List<string> Keys = new List<string>();

        public override void Visit(AsyncApiInfo info)
        {
            Locations.Add(this.PathString);
        }

        public override void Visit(AsyncApiComponents components)
        {
            Locations.Add(this.PathString);
        }

        public override void Visit(AsyncApiExternalDocs externalDocs)
        {
            Locations.Add(this.PathString);
        }

        public override void Visit(AsyncApiPaths paths)
        {
            Locations.Add(this.PathString);
        }

        public override void Visit(AsyncApiPathItem pathItem)
        {
            Keys.Add(CurrentKeys.Path);
            Locations.Add(this.PathString);
        }

        public override void Visit(AsyncApiResponses responses)
        {
            Locations.Add(this.PathString);
        }

        public override void Visit(AsyncApiOperation operation)
        {
            Keys.Add(CurrentKeys.Operation.ToString());
            Locations.Add(this.PathString);
        }
        public override void Visit(AsyncApiResponse response)
        {
            Keys.Add(CurrentKeys.Response);
            Locations.Add(this.PathString);
        }

        public override void Visit(IAsyncApiReferenceable referenceable)
        {
            Locations.Add("referenceAt: " + this.PathString);
        }
        public override void Visit(IDictionary<string, AsyncApiMediaType> content)
        {
            Locations.Add(this.PathString);
        }

        public override void Visit(AsyncApiMediaType mediaType)
        {
            Keys.Add(CurrentKeys.Content);
            Locations.Add(this.PathString);
        }

        public override void Visit(AsyncApiSchema schema)
        {
            Locations.Add(this.PathString);
        }

        public override void Visit(IList<AsyncApiTag> AsyncApiTags)
        {
            Locations.Add(this.PathString);
        }

        public override void Visit(IList<AsyncApiServer> servers)
        {
            Locations.Add(this.PathString);
        }

        public override void Visit(AsyncApiServer server)
        {
            Locations.Add(this.PathString);
        }
    }
}
