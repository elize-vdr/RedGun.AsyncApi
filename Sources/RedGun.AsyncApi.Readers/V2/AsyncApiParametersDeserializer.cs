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
        private static readonly FixedFieldMap<AsyncApiParameters> _parametersFixedFields = new FixedFieldMap<AsyncApiParameters>();

        private static readonly PatternFieldMap<AsyncApiParameters> _parametersPatternFields =
            new PatternFieldMap<AsyncApiParameters> {
                                                     {s => !s.StartsWith("x-"), (o,  k, n) => o.Add(k, LoadParameter(n))},
                                                     {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p, n))}
                                                 };

        public static AsyncApiParameters LoadParameters(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.Parameters);

            var domainObject = new AsyncApiParameters();

            ParseMap(mapNode, domainObject, _parametersFixedFields, _parametersPatternFields);

            return domainObject;
        }
    }
}