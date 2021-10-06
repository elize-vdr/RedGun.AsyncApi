// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using RedGun.AsyncApi.Exceptions;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;
using Xunit;

namespace RedGun.AsyncApi.Tests.Workspaces
{

    public class AsyncApiReferencableTests
    {
        private static readonly AsyncApiCallback _callbackFragment = new AsyncApiCallback();
        private static readonly AsyncApiExample _exampleFragment = new AsyncApiExample();
        private static readonly AsyncApiLink _linkFragment = new AsyncApiLink();
        private static readonly AsyncApiHeader _headerFragment = new AsyncApiHeader()
        {
            Schema = new AsyncApiSchema(),
            Examples = new Dictionary<string, AsyncApiExample>
            {
                { "example1", new AsyncApiExample() }
            }
        };
        private static readonly AsyncApiParameter _parameterFragment = new AsyncApiParameter
        {
            Schema = new AsyncApiSchema(),
            Examples = new Dictionary<string, AsyncApiExample>
            {
                { "example1", new AsyncApiExample() }
            }
        };
        private static readonly AsyncApiRequestBody _requestBodyFragment = new AsyncApiRequestBody();
        private static readonly AsyncApiResponse _responseFragment = new AsyncApiResponse()
        {
            Headers = new Dictionary<string, AsyncApiHeader>
            {
                { "header1", new AsyncApiHeader() }
            },
            Links = new Dictionary<string, AsyncApiLink>
            {
                { "link1", new AsyncApiLink() }
            }
        };
        private static readonly AsyncApiSchema _schemaFragment = new AsyncApiSchema();
        private static readonly AsyncApiSecurityScheme _securitySchemeFragment = new AsyncApiSecurityScheme();
        private static readonly AsyncApiTag _tagFragment = new AsyncApiTag();

        public static IEnumerable<object[]> ResolveReferenceCanResolveValidJsonPointersTestData =>
        new List<object[]>
        {
            new object[] { _callbackFragment, "/", _callbackFragment },
            new object[] { _exampleFragment, "/", _exampleFragment },
            new object[] { _linkFragment, "/", _linkFragment },
            new object[] { _headerFragment, "/", _headerFragment },
            new object[] { _headerFragment, "/schema", _headerFragment.Schema },
            new object[] { _headerFragment, "/examples/example1", _headerFragment.Examples["example1"] },
            new object[] { _parameterFragment, "/", _parameterFragment },
            new object[] { _parameterFragment, "/schema", _parameterFragment.Schema },
            new object[] { _parameterFragment, "/examples/example1", _parameterFragment.Examples["example1"] },
            new object[] { _requestBodyFragment, "/", _requestBodyFragment },
            new object[] { _responseFragment, "/", _responseFragment },
            new object[] { _responseFragment, "/headers/header1", _responseFragment.Headers["header1"] },
            new object[] { _responseFragment, "/links/link1", _responseFragment.Links["link1"] },
            new object[] { _schemaFragment, "/", _schemaFragment},
            new object[] { _securitySchemeFragment, "/", _securitySchemeFragment},
            new object[] { _tagFragment, "/", _tagFragment}
        };

        [Theory]
        [MemberData(nameof(ResolveReferenceCanResolveValidJsonPointersTestData))]
        public void ResolveReferenceCanResolveValidJsonPointers(
            IAsyncApiReferenceable element,
            string jsonPointer,
            IAsyncApiElement expectedResolvedElement)
        {
            // Act
            var actualResolvedElement = element.ResolveReference(new JsonPointer(jsonPointer));

            // Assert
            Assert.Same(expectedResolvedElement, actualResolvedElement);
        }

        public static IEnumerable<object[]> ResolveReferenceShouldThrowOnInvalidReferenceIdTestData =>
        new List<object[]>
        {
            new object[] { _callbackFragment, "/a" },
            new object[] { _headerFragment, "/a" },
            new object[] { _headerFragment, "/examples" },
            new object[] { _headerFragment, "/examples/" },
            new object[] { _headerFragment, "/examples/a" },
            new object[] { _parameterFragment, "/a" },
            new object[] { _parameterFragment, "/examples" },
            new object[] { _parameterFragment, "/examples/" },
            new object[] { _parameterFragment, "/examples/a" },
            new object[] { _responseFragment, "/a" },
            new object[] { _responseFragment, "/headers" },
            new object[] { _responseFragment, "/headers/" },
            new object[] { _responseFragment, "/headers/a" },
            new object[] { _responseFragment, "/content" },
            new object[] { _responseFragment, "/content/" },
            new object[] { _responseFragment, "/content/a" }

        };

        [Theory]
        [MemberData(nameof(ResolveReferenceShouldThrowOnInvalidReferenceIdTestData))]
        public void ResolveReferenceShouldThrowOnInvalidReferenceId(IAsyncApiReferenceable element, string jsonPointer)
        {
            // Act
            Action resolveReference = () => element.ResolveReference(new JsonPointer(jsonPointer));

            // Assert
            var exception = Assert.Throws<AsyncApiException>(resolveReference);
            Assert.Equal(string.Format(SRResource.InvalidReferenceId, jsonPointer), exception.Message);
        }
    }
}
