using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.Interface;
using RedGun.AsyncApi.Readers.Services;
using RedGun.AsyncApi.Services;
using RedGun.AsyncApi.Models;
using Xunit;

namespace RedGun.AsyncApi.Readers.Tests.AsyncApiWorkspaceTests
{
    public class OpenApiWorkspaceStreamTests
    {
        private const string SampleFolderPath = "V2Tests/Samples/AsyncApiWorkspace/";

        // Use OpenApiWorkspace to load a document and a referenced document

        [Fact]
        public async Task LoadDocumentIntoWorkspace()
        {
            // Create a reader that will resolve all references
            var reader = new AsyncApiStreamReader(new AsyncApiReaderSettings()
            {
                ReferenceResolution = ReferenceResolutionSetting.ResolveAllReferences,
                CustomExternalLoader = new MockLoader()
            });

            // Todo: this should be ReadAsync
            var stream = new MemoryStream();
            var doc = @"openapi: 3.0.0
info:
  title: foo
  version: 1.0.0
paths: {}";
            var wr = new StreamWriter(stream);
            wr.Write(doc);
            wr.Flush();
            stream.Position = 0;

            var result = await reader.ReadAsync(stream);

            Assert.NotNull(result.AsyncApiDocument.Workspace);

        }


        [Fact]
        public async Task LoadTodoDocumentIntoWorkspace()
        {
            // Create a reader that will resolve all references
            var reader = new AsyncApiStreamReader(new AsyncApiReaderSettings()
            {
                ReferenceResolution = ReferenceResolutionSetting.ResolveAllReferences,
                CustomExternalLoader = new ResourceLoader()
            });

            ReadResult result;
            using (var stream = Resources.GetStream("V2Tests/Samples/AsyncApiWorkspace/TodoMain.yaml"))
            {
                result = await reader.ReadAsync(stream);
            }

            // TODO: Commenting this out for now, have to change for AsyncAPI
            /*
            Assert.NotNull(result.AsyncApiDocument.Workspace);
            Assert.True(result.AsyncApiDocument.Workspace.Contains("TodoComponents.yaml"));
            var referencedSchema = result.AsyncApiDocument
                                            .Paths["/todos"]
                                            .Operations[OperationType.Get]
                                            .Responses["200"]
                                            .Content["application/json"]
                                                .Schema;
            Assert.Equal("object", referencedSchema.Type);
            Assert.Equal("string", referencedSchema.Properties["subject"].Type);
            Assert.False(referencedSchema.UnresolvedReference);

            var referencedParameter = result.AsyncApiDocument
                                            .Paths["/todos"]
                                            .Operations[OperationType.Get]
                                            .Parameters
                                            .Where(p => p.Name == "filter").FirstOrDefault();
            Assert.Equal("string", referencedParameter.Schema.Type);
            */

        }


    }

    public class MockLoader : IStreamLoader
    {
        public Stream Load(Uri uri)
        {
            return null;
        }

        public Task<Stream> LoadAsync(Uri uri)
        {
            return null;
        }
    }
    

    public class ResourceLoader : IStreamLoader
    {
        public Stream Load(Uri uri)
        {
            return null;
        }

        public Task<Stream> LoadAsync(Uri uri)
        {
            var path = new Uri(new Uri("http://example.org/V2Tests/Samples/AsyncApiWorkspace/"), uri).AbsolutePath;
            path = path.Substring(1); // remove leading slash
            return Task.FromResult(Resources.GetStream(path));
        }
    }
}
