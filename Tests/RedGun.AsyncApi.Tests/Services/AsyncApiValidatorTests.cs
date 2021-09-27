// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using FluentAssertions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;
using RedGun.AsyncApi.Services;
using RedGun.AsyncApi.Validations;
using RedGun.AsyncApi.Validations.Rules;
using RedGun.AsyncApi.Writers;
using Xunit;
using Xunit.Abstractions;

namespace RedGun.AsyncApi.Tests.Services
{
    [Collection("DefaultSettings")]
    public class AsyncApiValidatorTests
    {
        private readonly ITestOutputHelper _output;

        public AsyncApiValidatorTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void ResponseMustHaveADescription()
        {
            var AsyncApiDocument = new AsyncApiDocument();
            AsyncApiDocument.Info = new AsyncApiInfo()
            {
                Title = "foo",
                Version = "1.2.2"
            };
            AsyncApiDocument.Paths = new AsyncApiPaths();
            AsyncApiDocument.Paths.Add(
                "/test",
                new AsyncApiPathItem
                {
                    Operations =
                    {
                        [OperationType.Get] = new AsyncApiOperation
                        {
                            Responses =
                            {
                                ["200"] = new AsyncApiResponse()
                            }
                        }
                    }
                });

            var validator = new AsyncApiValidator(ValidationRuleSet.GetDefaultRuleSet());
            var walker = new AsyncApiWalker(validator);
            walker.Walk(AsyncApiDocument);

            validator.Errors.Should().BeEquivalentTo(
                    new List<AsyncApiError>
                    {
                        new AsyncApiValidatorError(nameof(AsyncApiResponseRules.ResponseRequiredFields),"#/paths/~1test/get/responses/200/description",
                            String.Format(SRResource.Validation_FieldIsRequired, "description", "response"))
        });
        }

        [Fact]
        public void ServersShouldBeReferencedByIndex()
        {
            var AsyncApiDocument = new AsyncApiDocument
            {
                Info = new AsyncApiInfo()
                {
                    Title = "foo",
                    Version = "1.2.2"
                },
                Servers = new List<AsyncApiServer> {
                new AsyncApiServer
                {
                    Url = "http://example.org"
                },
                new AsyncApiServer
                {

                },
            },
                Paths = new AsyncApiPaths()
            };

            var validator = new AsyncApiValidator(ValidationRuleSet.GetDefaultRuleSet());
            var walker = new AsyncApiWalker(validator);
            walker.Walk(AsyncApiDocument);

            validator.Errors.Should().BeEquivalentTo(
                    new List<AsyncApiError>
                    {
                        new AsyncApiValidatorError(nameof(AsyncApiServerRules.ServerRequiredFields), "#/servers/1/url",
                            String.Format(SRResource.Validation_FieldIsRequired, "url", "server"))
        });
        }


        [Fact]
        public void ValidateCustomExtension()
        {
            var ruleset = ValidationRuleSet.GetDefaultRuleSet();

            ruleset.Add(
             new ValidationRule<FooExtension>(
                 (context, item) =>
                 {
                     if (item.Bar == "hey")
                     {
                         context.AddError(new AsyncApiValidatorError("FooExtensionRule", context.PathString, "Don't say hey"));
                     }
                 }));

            var AsyncApiDocument = new AsyncApiDocument
            {
                Info = new AsyncApiInfo()
                {
                    Title = "foo",
                    Version = "1.2.2"
                },
                Paths = new AsyncApiPaths()
            };

            var fooExtension = new FooExtension()
            {
                Bar = "hey",
                Baz = "baz"
            };

            AsyncApiDocument.Info.Extensions.Add("x-foo", fooExtension);

            var validator = new AsyncApiValidator(ruleset);
            var walker = new AsyncApiWalker(validator);
            walker.Walk(AsyncApiDocument);

            validator.Errors.Should().BeEquivalentTo(
                   new List<AsyncApiError>
                   {
                       new AsyncApiValidatorError("FooExtensionRule", "#/info/x-foo", "Don't say hey")
                   });
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
