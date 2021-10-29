// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using FluentAssertions;
using Xunit;

namespace RedGun.AsyncApi.Readers.Tests.AsyncApiReaderTests
{
    [Collection("DefaultSettings")]
    public class AsyncApiDiagnosticTests
    {
        [Fact]
        public void DetectedSpecificationVersionShouldBeV2_0()
        {
            using (var stream = Resources.GetStream("V2Tests/Samples/AsyncApiDocument/minimalDocument.yaml"))
            {
                new AsyncApiStreamReader().Read(stream, out var diagnostic);

                diagnostic.Should().NotBeNull();
                diagnostic.SpecificationVersion.Should().Be(AsyncApiSpecVersion.AsyncApi2_0);
            }
        }
    }
}
