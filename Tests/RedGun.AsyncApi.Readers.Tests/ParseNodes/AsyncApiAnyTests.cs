// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.IO;
using System.Linq;
using FluentAssertions;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Readers.ParseNodes;
using SharpYaml.Serialization;
using Xunit;

namespace RedGun.AsyncApi.Readers.Tests.ParseNodes
{
    [Collection("DefaultSettings")]
    public class OpenApiAnyTests
    {
        [Fact]
        public void ParseMapAsAnyShouldSucceed()
        {
            var input = @"
aString: fooBar
aInteger: 10
aDouble: 2.34
aDateTime: 2017-01-01
                ";
            var yamlStream = new YamlStream();
            yamlStream.Load(new StringReader(input));
            var yamlNode = yamlStream.Documents.First().RootNode;

            var diagnostic = new AsyncApiDiagnostic();
            var context = new ParsingContext(diagnostic);

            var node = new MapNode(context, (YamlMappingNode)yamlNode);

            var anyMap = node.CreateAny();

            diagnostic.Errors.Should().BeEmpty();

            anyMap.Should().BeEquivalentTo(
                new AsyncApiObject
                {
                    ["aString"] = new AsyncApiString("fooBar"),
                    ["aInteger"] = new AsyncApiString("10"),
                    ["aDouble"] = new AsyncApiString("2.34"),
                    ["aDateTime"] = new AsyncApiString("2017-01-01")
                });
        }

        [Fact]
        public void ParseListAsAnyShouldSucceed()
        {
            var input = @"
- fooBar
- 10
- 2.34
- 2017-01-01
                ";
            var yamlStream = new YamlStream();
            yamlStream.Load(new StringReader(input));
            var yamlNode = yamlStream.Documents.First().RootNode;

            var diagnostic = new AsyncApiDiagnostic();
            var context = new ParsingContext(diagnostic);

            var node = new ListNode(context, (YamlSequenceNode)yamlNode);

            var any = node.CreateAny();

            diagnostic.Errors.Should().BeEmpty();

            any.Should().BeEquivalentTo(
                new AsyncApiArray
                {
                    new AsyncApiString("fooBar"),
                    new AsyncApiString("10"),
                    new AsyncApiString("2.34"),
                    new AsyncApiString("2017-01-01")
                });
        }

        [Fact]
        public void ParseScalarIntegerAsAnyShouldSucceed()
        {
            var input = @"
10
                ";
            var yamlStream = new YamlStream();
            yamlStream.Load(new StringReader(input));
            var yamlNode = yamlStream.Documents.First().RootNode;

            var diagnostic = new AsyncApiDiagnostic();
            var context = new ParsingContext(diagnostic);

            var node = new ValueNode(context, (YamlScalarNode)yamlNode);

            var any = node.CreateAny();

            diagnostic.Errors.Should().BeEmpty();

            any.Should().BeEquivalentTo(
                new AsyncApiString("10")
            );
        }

        [Fact]
        public void ParseScalarDateTimeAsAnyShouldSucceed()
        {
            var input = @"
2012-07-23T12:33:00
                ";
            var yamlStream = new YamlStream();
            yamlStream.Load(new StringReader(input));
            var yamlNode = yamlStream.Documents.First().RootNode;

            var diagnostic = new AsyncApiDiagnostic();
            var context = new ParsingContext(diagnostic);

            var node = new ValueNode(context, (YamlScalarNode)yamlNode);

            var any = node.CreateAny();

            diagnostic.Errors.Should().BeEmpty();

            any.Should().BeEquivalentTo(
                new AsyncApiString("2012-07-23T12:33:00")
            );
        }
    }
}
