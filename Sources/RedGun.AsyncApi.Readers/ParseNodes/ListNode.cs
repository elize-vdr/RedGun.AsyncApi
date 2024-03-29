﻿// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Readers.Exceptions;
using SharpYaml.Serialization;

namespace RedGun.AsyncApi.Readers.ParseNodes
{
    internal class ListNode : ParseNode, IEnumerable<ParseNode>
    {
        private readonly YamlSequenceNode _nodeList;

        public ListNode(ParsingContext context, YamlSequenceNode sequenceNode) : base(
            context)
        {
            _nodeList = sequenceNode;
        }

        public override List<T> CreateList<T>(Func<MapNode, T> map)
        {
            if (_nodeList == null)
            {
                throw new AsyncApiReaderException(
                    $"Expected list at line {_nodeList.Start.Line} while parsing {typeof(T).Name}", _nodeList);
            }

            return _nodeList.Select(n => map(new MapNode(Context, n as YamlMappingNode)))
                .Where(i => i != null)
                .ToList();
        }

        public override List<IAsyncApiAny> CreateListOfAny()
        {
            return _nodeList.Select(n => ParseNode.Create(Context, n).CreateAny())
                .Where(i => i != null)
                .ToList();
        }

        public override List<T> CreateSimpleList<T>(Func<ValueNode, T> mapFunc)
        {
            if (_nodeList == null)
            {
                throw new AsyncApiReaderException(
                    $"Expected list at line {_nodeList.Start.Line} while parsing {typeof(T).Name}", _nodeList);
            }

            return _nodeList.Select(n => mapFunc(new ValueNode(Context, n))).ToList();
        }

        public IEnumerator<ParseNode> GetEnumerator()
        {
            return _nodeList.Select(n => Create(Context, n)).ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Create a <see cref="AsyncApiArray"/>
        /// </summary>
        /// <returns>The created Any object.</returns>
        public override IAsyncApiAny CreateAny()
        {
            var array = new AsyncApiArray();
            foreach (var node in this)
            {
                array.Add(node.CreateAny());
            }

            return array;
        }
    }
}
