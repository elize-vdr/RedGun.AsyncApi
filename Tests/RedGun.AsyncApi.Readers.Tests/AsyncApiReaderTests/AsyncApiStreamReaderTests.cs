// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license.

using System.IO;
using Xunit;

namespace RedGun.AsyncApi.Readers.Tests.AsyncApiReaderTests
{
    public class AsyncApiStreamReaderTests
    {
        private const string SampleFolderPath = "V2Tests/Samples/AsyncApiDocument/";

        [Fact]
        public void StreamShouldCloseIfLeaveStreamOpenSettingEqualsFalse()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "petStore.yaml")))
            {
                var reader = new AsyncApiStreamReader(new AsyncApiReaderSettings { LeaveStreamOpen = false });
                reader.Read(stream, out _);
                Assert.False(stream.CanRead);
            }
        }

        [Fact]
        public void StreamShouldNotCloseIfLeaveStreamOpenSettingEqualsTrue()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "petStore.yaml")))
            {
                var reader = new AsyncApiStreamReader(new AsyncApiReaderSettings { LeaveStreamOpen = true});
                reader.Read(stream, out _);
                Assert.True(stream.CanRead);
            }
        }
    }
}
