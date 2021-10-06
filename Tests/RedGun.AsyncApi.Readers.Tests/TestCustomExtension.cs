// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using FluentAssertions;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;
using Xunit;

namespace RedGun.AsyncApi.Readers.Tests
{
    public class TestCustomExtension
    {
        [Fact]
        public void ParseCustomExtension()
        {
            var description = @"
AsyncApi: 3.0.0
info: 
    title: A doc with an extension
    version: 1.0.0
    x-foo: 
        bar: hey
        baz: hi!
paths: {}
";
            var settings = new AsyncApiReaderSettings()
            {
                ExtensionParsers = { { "x-foo", (a,v) => {
                        var fooNode = (AsyncApiObject)a;
                        return new FooExtension() {
                              Bar = (fooNode["bar"] as AsyncApiString)?.Value,
                              Baz = (fooNode["baz"] as AsyncApiString)?.Value
                        };
                } } }
            };

            var reader = new AsyncApiStringReader(settings);

            var diag = new AsyncApiDiagnostic();
            var doc = reader.Read(description, out diag);

            var fooExtension = doc.Info.Extensions["x-foo"] as FooExtension;

            fooExtension.Should().NotBeNull();
            fooExtension.Bar.Should().Be("hey");
            fooExtension.Baz.Should().Be("hi!");
        }
    }

    internal class FooExtension : IAsyncApiExtension, IAsyncApiElement
    {
        public string Baz { get; set; }

        public string Bar { get; set; }

        public void Write(IAsyncApiWriter writer, AsyncApiSpecVersion specVersion)
        {
            writer.WriteStartObject();
            writer.WriteProperty("baz", Baz);
            writer.WriteProperty("bar", Bar);
            writer.WriteEndObject();
        }
    }
}
