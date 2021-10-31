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
        public void BasicContract()
        {
            var input = @"asyncapi: '2.2.0'
id: 'urn:com:smartylighting:streetlights:server'
info:
    title: AsyncAPI Sample App
    description: This is a sample server.
    termsOfService: https://asyncapi.org/terms/
    contact:
      name: API Support
      url: https://www.asyncapi.org/support
      email: support@asyncapi.org
    license:
      name: Apache 2.0
      url: https://www.apache.org/licenses/LICENSE-2.0.html
    version: 1.0.1
servers:
    production:
        url: production.gigantic-server.com
        description: Production server
        protocol: kafka
        protocolVersion: '2.1.3'
    development:
        url: development.gigantic-server.com
        description: Development server
        protocol: kafka
        protocolVersion: '2.1.3'
    test:
        url: test.gigantic-server.com
        description: Test server
        protocol: kafka
        protocolVersion: '2.1.2'        
tags:
    - name: user
      description: User-related messages
    - name: search
      description: Search a data set
      externalDocs:
        description: Find more info here
        url: https://exampledocs.com
channels:
    user/signedup:
        description: User signed up and created account
        servers:
            - production
            - development";

            var reader = new AsyncApiStringReader();
            var asyncApiDocument = reader.Read(input, out var diagnostic);

            diagnostic.Errors.Should().BeEmpty();
        }
        
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

