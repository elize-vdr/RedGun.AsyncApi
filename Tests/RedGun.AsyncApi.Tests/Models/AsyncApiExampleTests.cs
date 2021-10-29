// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Globalization;
using System.IO;
using System.Text;
using FluentAssertions;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Writers;
using Xunit;
using Xunit.Abstractions;

namespace RedGun.AsyncApi.Tests.Models
{
    // TODO: Perhaps delete this because there is no test for Open API V3, which is ou Async API V2 equivalent??
    [Collection("DefaultSettings")]
    public class AsyncApiExampleTests
    {
        public static AsyncApiExample AdvancedExample = new AsyncApiExample
        {
            Value = new AsyncApiObject
            {
                ["versions"] = new AsyncApiArray
                {
                    new AsyncApiObject
                    {
                        ["status"] = new AsyncApiString("Status1"),
                        ["id"] = new AsyncApiString("v1"),
                        ["links"] = new AsyncApiArray
                        {
                            new AsyncApiObject
                            {
                                ["href"] = new AsyncApiString("http://example.com/1"),
                                ["rel"] = new AsyncApiString("sampleRel1"),
                                ["bytes"] = new AsyncApiByte(new byte[] { 1, 2, 3 }),
                                ["binary"] = new AsyncApiBinary(Encoding.UTF8.GetBytes("Ñ😻😑♮Í☛oƞ♑😲☇éǋžŁ♻😟¥a´Ī♃ƠąøƩ"))
                            }
                        }
                    },

                    new AsyncApiObject
                    {
                        ["status"] = new AsyncApiString("Status2"),
                        ["id"] = new AsyncApiString("v2"),
                        ["links"] = new AsyncApiArray
                        {
                            new AsyncApiObject
                            {
                                ["href"] = new AsyncApiString("http://example.com/2"),
                                ["rel"] = new AsyncApiString("sampleRel2")
                            }
                        }
                    }
                }
            }
        };

        public static AsyncApiExample ReferencedExample = new AsyncApiExample
        {
            Reference = new AsyncApiReference
            {
                Type = ReferenceType.Example,
                Id = "example1",
            },
            Value = new AsyncApiObject
            {
                ["versions"] = new AsyncApiArray
                {
                    new AsyncApiObject
                    {
                        ["status"] = new AsyncApiString("Status1"),
                        ["id"] = new AsyncApiString("v1"),
                        ["links"] = new AsyncApiArray
                        {
                            new AsyncApiObject
                            {
                                ["href"] = new AsyncApiString("http://example.com/1"),
                                ["rel"] = new AsyncApiString("sampleRel1")
                            }
                        }
                    },

                    new AsyncApiObject
                    {
                        ["status"] = new AsyncApiString("Status2"),
                        ["id"] = new AsyncApiString("v2"),
                        ["links"] = new AsyncApiArray
                        {
                            new AsyncApiObject
                            {
                                ["href"] = new AsyncApiString("http://example.com/2"),
                                ["rel"] = new AsyncApiString("sampleRel2")
                            }
                        }
                    }
                }
            }
        };

        private readonly ITestOutputHelper _output;

        public AsyncApiExampleTests(ITestOutputHelper output)
        {
            _output = output;
        }
    }
}
