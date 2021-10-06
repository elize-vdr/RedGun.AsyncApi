// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using FluentAssertions;
using RedGun.AsyncApi.Readers.Exceptions;
using Xunit;

namespace RedGun.AsyncApi.Readers.Tests.AsyncApiReaderTests
{
    [Collection("DefaultSettings")]
    public class UnsupportedSpecVersionTests
    {
        [Fact]
        public void ThrowAsyncApiUnsupportedSpecVersionException()
        {
            using (var stream = Resources.GetStream("AsyncApiReaderTests/Samples/unsupported.v1.yaml"))
            {
                try
                {
                    new AsyncApiStreamReader().Read(stream, out var diagnostic);
                }
                catch (AsyncApiUnsupportedSpecVersionException exception)
                {
                    exception.SpecificationVersion.Should().Be("1.0.0");
                }
            }
        }
    }
}
