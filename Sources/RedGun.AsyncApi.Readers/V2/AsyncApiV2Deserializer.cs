﻿// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK.
// Licensed under the MIT license. 

using System.Collections.Generic;
using System.Linq;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Exceptions;
using RedGun.AsyncApi.Expressions;
using RedGun.AsyncApi.Interfaces;
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
        private static void ParseMap<T>(
            MapNode mapNode,
            T domainObject,
            FixedFieldMap<T> fixedFieldMap,
            PatternFieldMap<T> patternFieldMap)
        {
            if (mapNode == null)
            {
                return;
            }

            foreach (var propertyNode in mapNode)
            {
                propertyNode.ParseField(domainObject, fixedFieldMap, patternFieldMap);
            }

        }

        private static void ProcessAnyFields<T>(
            MapNode mapNode,
            T domainObject,
            AnyFieldMap<T> anyFieldMap)
        {
            foreach (var anyFieldName in anyFieldMap.Keys.ToList())
            {
                try
                {
                    mapNode.Context.StartObject(anyFieldName);

                    var convertedAsyncApiAny = AsyncApiAnyConverter.GetSpecificAsyncApiAny(
                        anyFieldMap[anyFieldName].PropertyGetter(domainObject),
                        anyFieldMap[anyFieldName].SchemaGetter(domainObject));

                    anyFieldMap[anyFieldName].PropertySetter(domainObject, convertedAsyncApiAny);
                }
                catch (AsyncApiException exception)
                {
                    exception.Pointer = mapNode.Context.GetLocation();
                    mapNode.Context.Diagnostic.Errors.Add(new AsyncApiError(exception));
                }
                finally
                {
                    mapNode.Context.EndObject();
                }
            }
        }

        private static void ProcessAnyListFields<T>(
            MapNode mapNode,
            T domainObject,
            AnyListFieldMap<T> anyListFieldMap)
        {
            foreach (var anyListFieldName in anyListFieldMap.Keys.ToList())
            {
                try
                {
                    var newProperty = new List<IAsyncApiAny>();

                    mapNode.Context.StartObject(anyListFieldName);

                    foreach (var propertyElement in anyListFieldMap[anyListFieldName].PropertyGetter(domainObject))
                    {
                        newProperty.Add(
                            AsyncApiAnyConverter.GetSpecificAsyncApiAny(
                                propertyElement,
                                anyListFieldMap[anyListFieldName].SchemaGetter(domainObject)));
                    }

                    anyListFieldMap[anyListFieldName].PropertySetter(domainObject, newProperty);
                }
                catch (AsyncApiException exception)
                {
                    exception.Pointer = mapNode.Context.GetLocation();
                    mapNode.Context.Diagnostic.Errors.Add(new AsyncApiError(exception));
                }
                finally
                {
                    mapNode.Context.EndObject();
                }
            }
        }

        private static void ProcessAnyMapFields<T, U>(
            MapNode mapNode,
            T domainObject,
            AnyMapFieldMap<T, U> anyMapFieldMap)
        {
            foreach (var anyMapFieldName in anyMapFieldMap.Keys.ToList())
            {
                try
                {
                    var newProperty = new List<IAsyncApiAny>();

                    mapNode.Context.StartObject(anyMapFieldName);

                    foreach (var propertyMapElement in anyMapFieldMap[anyMapFieldName].PropertyMapGetter(domainObject))
                    {
                        mapNode.Context.StartObject(propertyMapElement.Key);

                        if (propertyMapElement.Value != null)
                        {
                            var any = anyMapFieldMap[anyMapFieldName].PropertyGetter(propertyMapElement.Value);

                            var newAny = AsyncApiAnyConverter.GetSpecificAsyncApiAny(
                                    any,
                                    anyMapFieldMap[anyMapFieldName].SchemaGetter(domainObject));

                            anyMapFieldMap[anyMapFieldName].PropertySetter(propertyMapElement.Value, newAny);
                        }
                    }
                }
                catch (AsyncApiException exception)
                {
                    exception.Pointer = mapNode.Context.GetLocation();
                    mapNode.Context.Diagnostic.Errors.Add(new AsyncApiError(exception));
                }
                finally
                {
                    mapNode.Context.EndObject();
                }
            }
        }

        private static RuntimeExpression LoadRuntimeExpression(ParseNode node)
        {
            var value = node.GetScalarValue();
            return RuntimeExpression.Build(value);
        }

        private static RuntimeExpressionAnyWrapper LoadRuntimeExpressionAnyWrapper(ParseNode node)
        {
            var value = node.GetScalarValue();

            if (value != null && value.StartsWith("$"))
            {
                return new RuntimeExpressionAnyWrapper
                {
                    Expression = RuntimeExpression.Build(value)
                };
            }

            return new RuntimeExpressionAnyWrapper
            {
                Any = AsyncApiAnyConverter.GetSpecificAsyncApiAny(node.CreateAny())
            };
        }

        public static IAsyncApiAny LoadAny(ParseNode node)
        {
            return AsyncApiAnyConverter.GetSpecificAsyncApiAny(node.CreateAny());
        }

        private static IAsyncApiExtension LoadExtension(string name, ParseNode node)
        {
            if (node.Context.ExtensionParsers.TryGetValue(name, out var parser))
            {
                return parser(
                    AsyncApiAnyConverter.GetSpecificAsyncApiAny(node.CreateAny()),
                    AsyncApiSpecVersion.AsyncApi2_0);
            }
            else
            {
                return AsyncApiAnyConverter.GetSpecificAsyncApiAny(node.CreateAny());
            }
        }

        private static string LoadString(ParseNode node)
        {
            return node.GetScalarValue();
        }
    }
}
