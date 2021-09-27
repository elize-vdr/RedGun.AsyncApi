using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.Interface;
using RedGun.AsyncApi.Services;
using SharpYaml.Model;

namespace RedGun.AsyncApi.Readers.Services
{
    internal class AsyncApiWorkspaceLoader 
    {
        private AsyncApiWorkspace _workspace;
        private IStreamLoader _loader;
        private readonly AsyncApiReaderSettings _readerSettings;

        public AsyncApiWorkspaceLoader(AsyncApiWorkspace workspace, IStreamLoader loader, AsyncApiReaderSettings readerSettings)
        {
            _workspace = workspace;
            _loader = loader;
            _readerSettings = readerSettings;
        }

        internal async Task LoadAsync(AsyncApiReference reference, AsyncApiDocument document)
        {
            _workspace.AddDocument(reference.ExternalResource, document);
            document.Workspace = _workspace;

            // Collect remote references by walking document
            var referenceCollector = new AsyncApiRemoteReferenceCollector(document);
            var collectorWalker = new AsyncApiWalker(referenceCollector);
            collectorWalker.Walk(document);

            var reader = new AsyncApiStreamReader(_readerSettings);

            // Walk references
            foreach (var item in referenceCollector.References)
            {
                // If not already in workspace, load it and process references
                if (!_workspace.Contains(item.ExternalResource))
                {
                    var input = await _loader.LoadAsync(new Uri(item.ExternalResource, UriKind.RelativeOrAbsolute));
                    var result = await reader.ReadAsync(input); // TODO merge _diagnositics
                    await LoadAsync(item, result.AsyncApiDocument);
                }
            }
        }
    }
}
