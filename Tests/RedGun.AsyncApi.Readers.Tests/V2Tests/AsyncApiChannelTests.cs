// Licensed under the MIT license. 

using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.ParseNodes;
using RedGun.AsyncApi.Readers.V2;
using SharpYaml.Serialization;
using Xunit;

namespace RedGun.AsyncApi.Readers.Tests.V2Tests
{
    [Collection("DefaultSettings")]
    public class AsyncApiChannelTests
    {
        [Fact]
        public void ParsePrimitiveStringChannelFragmentShouldSucceed()
        {
            var input = @"title: AsyncAPI Sample App
description: This is a sample server.
termsOfService: https://asyncapi.org/terms/
contact:
  name: API Support
  url: https://www.asyncapi.org/support
  email: support@asyncapi.org
license:
  name: Apache 2.0
  url: https://www.apache.org/licenses/LICENSE-2.0.html
version: 1.0.1";
            var reader = new AsyncApiStringReader();
            var diagnostic = new AsyncApiDiagnostic();

            // Act
            var schema = reader.ReadFragment<AsyncApiInfo>(input, AsyncApiSpecVersion.AsyncApi2_0, out diagnostic);

            // Assert
            diagnostic.Should().BeEquivalentTo(new AsyncApiDiagnostic());
            diagnostic.Errors.Should().BeEmpty();

            /*
            schema.Should().BeEquivalentTo(
                new AsyncApiSchema
                {
                    Type = "integer",
                    Format = "int64",
                    Default = new AsyncApiLong(88)
                });
                */
        }
    }
}