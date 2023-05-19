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
        - development
      subscribe:
          operationId: registerUser
          summary: Action to sign a user up.
          description: A longer description of the message.
          tags:
            - name: user
            - name: signup
            - name: register
          message:
            - headers:
                type: object
                properties:
                  applicationInstanceId:
                    description: Unique identifier for a given instance of the publishing application
                    type: string
              payload:
                type: object
                properties:
                  user:
                    $ref: ""#/components/schemas/user""
                  signup:
                    type: string
      bindings:
        ws:
          method: GET
          query:
            type: object
            required:
              - name
            properties:
              name:
                type: string
              address:
                type: string
              size:
                type: integer
                format: int32
                minimum: 0
  user/{userId}/signup:
    parameters:
      userId:
        description: Id of the user.
        schema:
          type: string
components:
  schemas:
    user:
      type: object
      properties:
        age:
          type: integer
          format: int64
        firstname:
          type: string
        surname:
          type: string";

            var reader = new AsyncApiStringReader();
            var asyncApiDocument = reader.Read(input, out var diagnostic);

            diagnostic.Errors.Should().BeEmpty();
        }
        
                [Fact]
        public void BusinessEventsContract()
        {
            var input = @"asyncapi: '2.2.0'
info:
  title: An admin has been added to a company
  version: 1.0.1
channels:
  businessevent.platform.company:
    subscribe:
      message:
        $ref: '#/components/messages/adminAdded'
components:
  messages:
    adminAdded:
      name: adminAdded
      title: Admin added
      summary: A company admin has been added to a company
      contentType: aplication/json
      bindings:
        kafka:
          key:
            type: string
            description: Event Id
      payload:
        $ref: '#/components/schemas/adminAdded'
      traits:
        - $ref: 'https://gist.githubusercontent.com/abhishekamte-mx/2b3b45e893135dcc026302a05d08df43/raw/1c5a0c2d75f632cff045b452b7e9add133e049b7/cloudevents-v1.0.1-asyncapi-trait.yml'
  schemas:
    adminAdded:
      type: object
      properties:
        company_id:
          type: string
          ""maxLength"": 200
        user_id:
          type: string
          ""maxLength"": 36";

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

