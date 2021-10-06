// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Services;
using Xunit;

namespace RedGun.AsyncApi.Tests
{
    
    public class AsyncApiWorkspaceTests
    {
        [Fact]
        public void AsyncApiWorkspaceCanHoldMultipleDocuments()
        {
            var workspace = new AsyncApiWorkspace();

            workspace.AddDocument("root", new AsyncApiDocument());
            workspace.AddDocument("common", new AsyncApiDocument());

            Assert.Equal(2, workspace.Documents.Count());
        }

        [Fact]
        public void AsyncApiWorkspacesAllowDocumentsToReferenceEachOther()
        {
            var workspace = new AsyncApiWorkspace();

            workspace.AddDocument("root", new AsyncApiDocument() {
                Paths = new AsyncApiPaths()
                {
                    ["/"] = new AsyncApiPathItem()
                    {
                        Operations  = new Dictionary<OperationType, AsyncApiOperation>()
                        {
                            [OperationType.Get] = new AsyncApiOperation() {
                                Responses = new AsyncApiResponses()
                                {
                                    ["200"] = new AsyncApiResponse()
                                    {
                                       Content = new Dictionary<string,AsyncApiMediaType>()
                                       {
                                           ["application/json"] = new AsyncApiMediaType()
                                           {
                                               Schema = new AsyncApiSchema()
                                               {
                                                   Reference = new AsyncApiReference()
                                                   {
                                                       Id = "test",
                                                       Type = ReferenceType.Schema
                                                   }
                                               }
                                           }
                                       }
                                    }
                                }
                            }
                        }
                    } 
                }
            });
            workspace.AddDocument("common", new AsyncApiDocument() {
                Components = new AsyncApiComponents()
                {
                    Schemas = {
                        ["test"] = new AsyncApiSchema() {
                            Type = "string",
                            Description = "The referenced one"
                        }
                    }
                }
            });

            Assert.Equal(2, workspace.Documents.Count());
        }

        [Fact]
        public void AsyncApiWorkspacesCanResolveExternalReferences()
        {
            var workspace = new AsyncApiWorkspace();
            workspace.AddDocument("common", CreateCommonDocument());
            var schema = workspace.ResolveReference(new AsyncApiReference()
            {
                Id = "test",
                Type = ReferenceType.Schema,
                ExternalResource ="common"
            }) as AsyncApiSchema;

            Assert.NotNull(schema);
            Assert.Equal("The referenced one", schema.Description);
        }

        [Fact]
        public void AsyncApiWorkspacesAllowDocumentsToReferenceEachOther_short()
        {
            var workspace = new AsyncApiWorkspace();

            var doc = new AsyncApiDocument();
            doc.CreatePathItem("/", p =>
            {
                p.Description = "Consumer";
                p.CreateOperation(OperationType.Get, op =>
                  op.CreateResponse("200", re =>
                  {
                      re.Description = "Success";
                      re.CreateContent("application/json", co =>
                          co.Schema = new AsyncApiSchema()
                          {
                              Reference = new AsyncApiReference()  // Reference 
                              {
                                  Id = "test",
                                  Type = ReferenceType.Schema,
                                  ExternalResource = "common"
                              },
                              UnresolvedReference = true
                          }
                      );
                  })
                );
            });

            workspace.AddDocument("root", doc);
            workspace.AddDocument("common", CreateCommonDocument());
            var errors = doc.ResolveReferences(true);
            Assert.Empty(errors);

            var schema = doc.Paths["/"].Operations[OperationType.Get].Responses["200"].Content["application/json"].Schema;
            Assert.False(schema.UnresolvedReference);
        }

        [Fact]
        public void AsyncApiWorkspacesShouldNormalizeDocumentLocations()
        {
            var workspace = new AsyncApiWorkspace();
            workspace.AddDocument("hello", new AsyncApiDocument());
            workspace.AddDocument("hi", new AsyncApiDocument());

            Assert.True(workspace.Contains("./hello"));
            Assert.True(workspace.Contains("./foo/../hello"));
            Assert.True(workspace.Contains("file://" + Environment.CurrentDirectory + "/./foo/../hello"));

            Assert.False(workspace.Contains("./goodbye"));
        }

        // Enable Workspace to load from any reader, not just streams.

        // Test fragments
        public void AsyncApiWorkspacesShouldLoadDocumentFragments()
        {
            Assert.True(false);
        }

        [Fact]
        public void AsyncApiWorkspacesCanResolveReferencesToDocumentFragments()
        {
            // Arrange
            var workspace = new AsyncApiWorkspace();
            var schemaFragment = new AsyncApiSchema { Type = "string", Description = "Schema from a fragment" };
            workspace.AddFragment("fragment", schemaFragment);

            // Act
            var schema = workspace.ResolveReference(new AsyncApiReference()
            {
                ExternalResource = "fragment"
            }) as AsyncApiSchema;

            // Assert
            Assert.NotNull(schema);
            Assert.Equal("Schema from a fragment", schema.Description);
        }

        [Fact]
        public void AsyncApiWorkspacesCanResolveReferencesToDocumentFragmentsWithJsonPointers()
        {
            // Arrange
            var workspace = new AsyncApiWorkspace();
            var responseFragment = new AsyncApiResponse()
            {
                Headers = new Dictionary<string, AsyncApiHeader>
                {
                    { "header1", new AsyncApiHeader() }
                }
            };
            workspace.AddFragment("fragment", responseFragment);

            // Act
            var resolvedElement = workspace.ResolveReference(new AsyncApiReference()
            {
                Id = "headers/header1",
                ExternalResource = "fragment"
            });

            // Assert
            Assert.Same(responseFragment.Headers["header1"], resolvedElement);
        }


        // Test artifacts

        private static AsyncApiDocument CreateCommonDocument()
        {
            return new AsyncApiDocument()
            {
                Components = new AsyncApiComponents()
                {
                    Schemas = {
                        ["test"] = new AsyncApiSchema() {
                            Type = "string",
                            Description = "The referenced one"
                        }
                    }
                }
            };
        }
    }

    public static class AsyncApiFactoryExtensions {

    public static AsyncApiDocument CreatePathItem(this AsyncApiDocument document, string path, Action<AsyncApiPathItem> config)
    {
        var pathItem = new AsyncApiPathItem();
        config(pathItem);
        document.Paths = new AsyncApiPaths();
        document.Paths.Add(path, pathItem);
        return document;
    }

    public static AsyncApiPathItem CreateOperation(this AsyncApiPathItem parent, OperationType opType, Action<AsyncApiOperation> config)
    {
        var child = new AsyncApiOperation();
        config(child);
        parent.Operations.Add(opType, child);
        return parent;
    }

    public static AsyncApiOperation CreateResponse(this AsyncApiOperation parent, string status, Action<AsyncApiResponse> config)
    {
        var child = new AsyncApiResponse();
        config(child);
        parent.Responses.Add(status, child);
        return parent;
    }

    public static AsyncApiResponse CreateContent(this AsyncApiResponse parent, string mediaType, Action<AsyncApiMediaType> config)
    {
        var child = new AsyncApiMediaType();
        config(child);
        parent.Content.Add(mediaType, child);
        return parent;
    }

}
}
