// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using FluentAssertions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers;
using RedGun.AsyncApi.Readers.Exceptions;
using Xunit;

namespace RedGun.AsyncApi.Tests
{
    public class ParseNodeTests
    {
        [Fact]
        public void BrokenSimpleList()
        {
            var input = @"swagger: 2.0
info:
  title: hey
  version: 1.0.0
schemes: [ { ""hello"" }]
paths: { }";

            var reader = new AsyncApiStringReader();
            reader.Read(input, out var diagnostic);

            diagnostic.Errors.Should().BeEquivalentTo(new List<AsyncApiError>() {
                new AsyncApiError(new AsyncApiReaderException("Expected a value.") {
                    Pointer = "#line=4"
                })
            });
        }

        [Fact]
        public void BadSchema()
        {
            var input = @"openapi: 3.0.0
info:
  title: foo
  version: bar
paths:
  '/foo':
    get:
      responses:
        200: 
          description: ok
          content:
            application/json:  
              schema: asdasd
";

            var reader = new AsyncApiStringReader();
            reader.Read(input, out var diagnostic);

            diagnostic.Errors.Should().BeEquivalentTo(new List<AsyncApiError>() {
                new AsyncApiError(new AsyncApiReaderException("schema must be a map/object") {
                    Pointer = "#/paths/~1foo/get/responses/200/content/application~1json/schema"
                })
            });
        }
    }
}

