// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license.

using System.IO;
using System.Threading.Tasks;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.Interface;

namespace RedGun.AsyncApi.Readers
{
    /// <summary>
    /// Service class for converting streams into AsyncApiDocument instances
    /// </summary>
    public class AsyncApiStreamReader : IAsyncApiReader<Stream, AsyncApiDiagnostic>
    {
        private readonly AsyncApiReaderSettings _settings;

        /// <summary>
        /// Create stream reader with custom settings if desired.
        /// </summary>
        /// <param name="settings"></param>
        public AsyncApiStreamReader(AsyncApiReaderSettings settings = null)
        {
            _settings = settings ?? new AsyncApiReaderSettings();
        }

        /// <summary>
        /// Reads the stream input and parses it into an Async API document.
        /// </summary>
        /// <param name="input">Stream containing AsyncAPI description to parse.</param>
        /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing.</param>
        /// <returns>Instance of newly created AsyncApiDocument.</returns>
        public AsyncApiDocument Read(Stream input, out AsyncApiDiagnostic diagnostic)
        {
            var reader = new StreamReader(input);
            var result = new AsyncApiTextReaderReader(_settings).Read(reader, out diagnostic);
            if (!_settings.LeaveStreamOpen)
            {
                reader.Dispose();
            }

            return result;
        }

        /// <summary>
        /// Reads the stream input and parses it into an Async API document.
        /// </summary>
        /// <param name="input">Stream containing AsyncAPI description to parse.</param>
        /// <returns>Instance result containing newly created AsyncApiDocument and diagnostics object from the process</returns>
        public async Task<ReadResult> ReadAsync(Stream input)
        {
            MemoryStream bufferedStream;
            if (input is MemoryStream)
            {
                bufferedStream = (MemoryStream)input;
            }
            else
            {
                // Buffer stream so that AsyncApiTextReaderReader can process it synchronously
                // YamlDocument doesn't support async reading.
                bufferedStream = new MemoryStream();
                await input.CopyToAsync(bufferedStream);
                bufferedStream.Position = 0;
            }

            var reader = new StreamReader(bufferedStream);

            return await new AsyncApiTextReaderReader(_settings).ReadAsync(reader);
        }

        /// <summary>
        /// Reads the stream input and parses the fragment of an AsyncAPI description into an Async API Element.
        /// </summary>
        /// <param name="input">Stream containing AsyncAPI description to parse.</param>
        /// <param name="version">Version of the AsyncAPI specification that the fragment conforms to.</param>
        /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing</param>
        /// <returns>Instance of newly created AsyncApiDocument</returns>
        public T ReadFragment<T>(Stream input, AsyncApiSpecVersion version, out AsyncApiDiagnostic diagnostic) where T : IAsyncApiReferenceable
        {
            using (var reader = new StreamReader(input))
            {
                return new AsyncApiTextReaderReader(_settings).ReadFragment<T>(reader, version, out diagnostic);
            }
        }
    }
}
