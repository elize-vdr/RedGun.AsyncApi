using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers;
using RedGun.AsyncApi.Services;
using RedGun.AsyncApi.Validations;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.CommandlineTool {
    static class AsyncApiService {
        public static void ProcessAsyncApiDocument(string input,
                                                  FileInfo output,
                                                  AsyncApiSpecVersion version,
                                                  AsyncApiFormat format,
                                                  bool inline,
                                                  bool resolveExternal)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            var stream = GetStream(input);

            AsyncApiDocument document;

            var result = new AsyncApiStreamReader(new AsyncApiReaderSettings
            {
                ReferenceResolution = resolveExternal == true ? ReferenceResolutionSetting.ResolveAllReferences : ReferenceResolutionSetting.ResolveLocalReferences,
                RuleSet = ValidationRuleSet.GetDefaultRuleSet()
            }
            ).ReadAsync(stream).GetAwaiter().GetResult();

            document = result.AsyncApiDocument;
            var context = result.AsyncApiDiagnostic;

            if (context.Errors.Count != 0)
            {
                var errorReport = new StringBuilder();

                foreach (var error in context.Errors)
                {
                    errorReport.AppendLine(error.ToString());
                }

                throw new ArgumentException(String.Join(Environment.NewLine, context.Errors.Select(e => e.Message).ToArray()));
            }

            using (var outputStream = output?.Create())
            {
                TextWriter textWriter;

                if (outputStream != null)
                {
                    textWriter = new StreamWriter(outputStream);
                }
                else
                {
                    textWriter = Console.Out;
                }

                var settings = new AsyncApiWriterSettings()
                {
                    ReferenceInline = inline == true ? ReferenceInlineSetting.InlineLocalReferences : ReferenceInlineSetting.DoNotInlineReferences
                };
                IAsyncApiWriter writer;
                switch (format)
                {
                    case AsyncApiFormat.Json:
                        writer = new AsyncApiJsonWriter(textWriter, settings);
                        break;
                    case AsyncApiFormat.Yaml:
                        writer = new AsyncApiYamlWriter(textWriter, settings);
                        break;
                    default:
                        throw new ArgumentException("Unknown format");
                }

                document.Serialize(writer, version);

                textWriter.Flush();
            }
        }

        private static Stream GetStream(string input)
        {
            Stream stream;
            if (input.StartsWith("http"))
            {
                var httpClient = new HttpClient(new HttpClientHandler()
                {
                    SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                })
                {
                    DefaultRequestVersion = HttpVersion.Version20
                };
                stream = httpClient.GetStreamAsync(input).Result;
            }
            else
            {
                var fileInput = new FileInfo(input);
                stream = fileInput.OpenRead();
            }

            return stream;
        }

        internal static void ValidateAsyncApiDocument(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            var stream = GetStream(input);

            AsyncApiDocument document;

            document = new AsyncApiStreamReader(new AsyncApiReaderSettings
            {
                //ReferenceResolution = resolveExternal == true ? ReferenceResolutionSetting.ResolveAllReferences : ReferenceResolutionSetting.ResolveLocalReferences,
                RuleSet = ValidationRuleSet.GetDefaultRuleSet()
            }
            ).Read(stream, out var context);

            if (context.Errors.Count != 0)
            {
                foreach (var error in context.Errors)
                {
                    Console.WriteLine(error.ToString());
                }
            }

            var statsVisitor = new StatsVisitor();
            var walker = new AsyncApiWalker(statsVisitor);
            walker.Walk(document);

            Console.WriteLine(statsVisitor.GetStatisticsReport());
        }
    }
}