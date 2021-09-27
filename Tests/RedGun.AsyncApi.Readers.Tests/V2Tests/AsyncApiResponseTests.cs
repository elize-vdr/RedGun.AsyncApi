// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license.

using System.IO;
using System.Linq;
using Xunit;

namespace RedGun.AsyncApi.Readers.Tests.V2Tests
{
    [Collection("DefaultSettings")]
    public class AsyncApiResponseTests
    {
        private const string SampleFolderPath = "V2Tests/Samples/AsyncApiResponse/";

        [Fact]
        public void ResponseWithReferencedHeaderShouldReferenceComponent()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "responseWithHeaderReference.yaml")))
            {
                var openApiDoc = new AsyncApiStreamReader().Read(stream, out var diagnostic);

                var response = openApiDoc.Components.Responses["Test"];

                Assert.Same(response.Headers.First().Value, openApiDoc.Components.Headers.First().Value);
            }
        }
    }
}
