// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using FluentAssertions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.V2;
using Xunit;

namespace RedGun.AsyncApi.Readers.Tests
{
    public class ConvertToAsyncApiReferenceV3Tests
    {

        [Fact]
        public void ParseExternalReference()
        {
            // Arrange
            var versionService = new AsyncApiV2VersionService();
            var externalResource = "externalSchema.json";
            var id = "/externalPathSegment1/externalPathSegment2/externalPathSegment3";
            var input = $"{externalResource}#{id}";

            // Act
            var reference = versionService.ConvertToAsyncApiReference(input, null);

            // Assert
            reference.Type.Should().BeNull();
            reference.ExternalResource.Should().Be(externalResource);
            reference.Id.Should().Be(id);
        }

        [Fact]
        public void ParseLocalParameterReference()
        {
            // Arrange
            var versionService = new AsyncApiV2VersionService();
            var referenceType = ReferenceType.Parameter;
            var id = "parameterId";
            var input = $"#/components/parameters/{id}";

            // Act
            var reference = versionService.ConvertToAsyncApiReference(input, referenceType);

            // Assert
            reference.Type.Should().Be(referenceType);
            reference.ExternalResource.Should().BeNull();
            reference.Id.Should().Be(id);
        }

        [Fact]
        public void ParseLocalSchemaReference()
        {
            // Arrange
            var versionService = new AsyncApiV2VersionService();
            var referenceType = ReferenceType.Schema;
            var id = "schemaId";
            var input = $"#/components/schemas/{id}";

            // Act
            var reference = versionService.ConvertToAsyncApiReference(input, referenceType);

            // Assert
            reference.Type.Should().Be(referenceType);
            reference.ExternalResource.Should().BeNull();
            reference.Id.Should().Be(id);
        }

        [Fact]
        public void ParseTagReference()
        {
            // Arrange
            var versionService = new AsyncApiV2VersionService();
            var referenceType = ReferenceType.Tag;
            var id = "tagId";
            var input = $"{id}";

            // Act
            var reference = versionService.ConvertToAsyncApiReference(input, referenceType);

            // Assert
            reference.Type.Should().Be(referenceType);
            reference.ExternalResource.Should().BeNull();
            reference.Id.Should().Be(id);
        }

        [Fact]
        public void ParseSecuritySchemeReference()
        {
            // Arrange
            var versionService = new AsyncApiV2VersionService();
            var referenceType = ReferenceType.SecurityScheme;
            var id = "securitySchemeId";
            var input = $"{id}";

            // Act
            var reference = versionService.ConvertToAsyncApiReference(input, referenceType);

            // Assert
            reference.Type.Should().Be(referenceType);
            reference.ExternalResource.Should().BeNull();
            reference.Id.Should().Be(id);
        }
    }
}
