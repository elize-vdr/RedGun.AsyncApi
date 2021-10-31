// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.IO;
using System.Linq;
using FluentAssertions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.ParseNodes;
using RedGun.AsyncApi.Readers.V2;
using Xunit;

namespace RedGun.AsyncApi.Readers.Tests.V2Tests
{
    public class AsyncApiOperationTests
    {
        private const string SampleFolderPath = "V2Tests/Samples/AsyncApiOperation/";

        
        /* TODO: Commenting out for now, have to change for AsyncAPI
        [Fact]
        public void OperationWithSecurityRequirementShouldReferenceSecurityScheme()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "securedOperation.yaml")))
            {
                var AsyncApiDoc = new AsyncApiStreamReader().Read(stream, out var diagnostic);

                var securityRequirement = AsyncApiDoc.Paths["/"].Operations[Models.OperationType.Get].Security.First();

                Assert.Same(securityRequirement.Keys.First(), AsyncApiDoc.Components.SecuritySchemes.First().Value);
            }
        }

        [Fact]
        public void ParseOperationWithParameterWithNoLocationShouldSucceed()
        {
            // Arrange
            MapNode node;
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "operationWithParameterWithNoLocation.json")))
            {
                node = TestHelper.CreateYamlMapNode(stream);
            }

            // Act
            var operation = AsyncApiV2Deserializer.LoadOperation(node);

            // Assert
            operation.Should().BeEquivalentTo(new AsyncApiOperation()
            {
                Tags =
                {
                    new AsyncApiTag
                    {
                        UnresolvedReference = true,
                        Reference = new AsyncApiReference()
                        {
                            Id = "user",
                            Type = ReferenceType.Tag
                        }
                    }
                },
                Summary = "Logs user into the system",
                Description = "",
                OperationId = "loginUser",
                Parameters =
                {
                    new AsyncApiParameter
                    {
                        Name = "username",
                        Description = "The user name for login",
                        Required = true,
                        Schema = new AsyncApiSchema
                        {
                            Type = "string"
                        }
                    },
                    new AsyncApiParameter
                    {
                        Name = "password",
                        Description = "The password for login in clear text",
                        In = ParameterLocation.Query,
                        Required = true,
                        Schema = new AsyncApiSchema
                        {
                            Type = "string"
                        }
                    }
                }
            });
        }
         */
    }
}
