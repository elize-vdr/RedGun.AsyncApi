using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers;
using RedGun.AsyncApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace RedGun.AsyncApi.SmokeTests
{
    public class GraphTests
    {
        AsyncApiDocument _graphSyncApi;
        HttpClient _httpClient;
        private readonly ITestOutputHelper _output;
        // TODO: What should this be?
        const string graphOpenApiUrl = "https://github.com/microsoftgraph/microsoft-graph-openapi/blob/master/v1.0.json?raw=true";

        public GraphTests(ITestOutputHelper output)
        {
            _output = output;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _httpClient = new HttpClient(new HttpClientHandler()
            {                AutomaticDecompression = DecompressionMethods.GZip
            });
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("AsyncApi.Net.Tests", "1.0"));

            var response = _httpClient.GetAsync(graphOpenApiUrl)
                                .GetAwaiter().GetResult();

            if (!response.IsSuccessStatusCode)
            {
                _output.WriteLine($"Couldn't load graph openapi");
                return;
            }

            var stream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult(); ;

            var reader = new AsyncApiStreamReader();
            _graphSyncApi = reader.Read(stream, out var diagnostic);

            if (diagnostic.Errors.Count > 0)
            {
                _output.WriteLine($"Errors parsing");
                _output.WriteLine(String.Join("\n", diagnostic.Errors));
                //               Assert.True(false);  // Uncomment to identify descriptions with errors.
            }


        }

        //[Fact(Skip="Run manually")]
        public void LoadOpen()
        {
            var operations = new[] { "foo","bar" };
            var workspace = new AsyncApiWorkspace();
            workspace.AddDocument(graphOpenApiUrl, _graphSyncApi);
            var subset = new AsyncApiDocument();
            workspace.AddDocument("subset", subset);

            Assert.NotNull(_graphSyncApi);
        }
    }
}
