using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace RedGun.AsyncApi.CommandlineTool {
    class Program {
        static async Task Main(string[] args) {
            var rootCommand = new RootCommand() {
                                                };

            var validateCommand = new Command("validate")
                                  {
                                      new Option("--input", "Input AsyncAPI description file path or URL", typeof(string) )
                                  };
            validateCommand.Handler = CommandHandler.Create<string>(AsyncApiService.ValidateAsyncApiDocument);

            var transformCommand = new Command("transform")
                                   {
                                       new Option("--input", "Input AsyncAPI description file path or URL", typeof(string) ),
                                       new Option("--output","Output AsyncAPI description file", typeof(FileInfo), arity: ArgumentArity.ZeroOrOne),
                                       new Option("--version", "AsyncAPI specification version", typeof(AsyncApiSpecVersion)),
                                       new Option("--format", "File format",typeof(AsyncApiFormat) ),
                                       new Option("--inline", "Inline $ref instances", typeof(bool) ),
                                       new Option("--resolveExternal","Resolve external $refs", typeof(bool))
                                   };
            transformCommand.Handler = CommandHandler.Create<string, FileInfo, AsyncApiSpecVersion, AsyncApiFormat, bool, bool>(
             AsyncApiService.ProcessAsyncApiDocument);

            rootCommand.Add(transformCommand);
            rootCommand.Add(validateCommand);

            // Parse the incoming args and invoke the handler
            var result = await rootCommand.InvokeAsync(args);
        }
    }
}