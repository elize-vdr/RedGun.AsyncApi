// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK.
// Licensed under the MIT license. 

using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.ParseNodes;

namespace RedGun.AsyncApi.Readers.V2
{
    /// <summary>
    /// Class containing logic to deserialize Open API V2 document into
    /// runtime Open API object model.
    /// </summary>
    internal static partial class AsyncApiV2Deserializer
    {
        private static readonly FixedFieldMap<AsyncApiOperation> _operationFixedFields =
            new FixedFieldMap<AsyncApiOperation>
            {
                {
                    "summary", (o, n) =>
                    {
                        o.Summary = n.GetScalarValue();
                    }
                },
                {
                    "description", (o, n) =>
                    {
                        o.Description = n.GetScalarValue();
                    }
                },
                {
                    "tags", (o, n) => {o.Tags = n.CreateList(LoadTag);
                        foreach (var tag in o.Tags)
                        {
                            tag.Reference = new AsyncApiReference()
                            {
                                Id = tag.Name,
                                Type = ReferenceType.Tag
                            };
                        }
                    }
                },
                {
                    "externalDocs", (o, n) =>
                    {
                        o.ExternalDocs = LoadExternalDocs(n);
                    }
                },
                {
                    "operationId", (o, n) =>
                    {
                        o.OperationId = n.GetScalarValue();
                    }
                },
                // TODO: Add correct fields
                {
                    "parameters", (o, n) =>
                    {
                        o.Parameters = n.CreateList(LoadParameter);
                    }
                },
                {
                    "requestBody", (o, n) =>
                    {
                        o.RequestBody = LoadRequestBody(n);
                    }
                },
                {
                    "responses", (o, n) =>
                    {
                        o.Responses = LoadResponses(n);
                    }
                },
                {
                    "callbacks", (o, n) =>
                    {
                        o.Callbacks = n.CreateMap(LoadCallback);
                    }
                },
                {
                    "servers", (o, n) =>
                    {
                        o.Servers = n.CreateList(LoadServer);
                    }
                },
            };

        private static readonly PatternFieldMap<AsyncApiOperation> _operationPatternFields =
            new PatternFieldMap<AsyncApiOperation>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))},
            };

        internal static AsyncApiOperation LoadOperation(ParseNode node)
        {
            var mapNode = node.CheckMapNode("Operation");

            var operation = new AsyncApiOperation();

            ParseMap(mapNode, operation, _operationFixedFields, _operationPatternFields);

            return operation;
        }

        private static AsyncApiTag LoadTagByReference(
            ParsingContext context,
            string tagName)
        {
            var tagObject = new AsyncApiTag()
            {
                UnresolvedReference = true,
                Reference = new AsyncApiReference()
                {
                    Type = ReferenceType.Tag,
                    Id = tagName
                }
            };

            return tagObject;
        }
    }
}
