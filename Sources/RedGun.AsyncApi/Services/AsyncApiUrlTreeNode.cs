// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using RedGun.AsyncApi.Models;

namespace RedGun.AsyncApi.Services
{
    /// <summary>
    /// A directory structure representing the paths of an AsyncAPI document.
    /// </summary>
    public class AsyncApiUrlTreeNode
    {
        private const string RootPathSegment = "/";
        private const string PathSeparator = "\\";

        /// <summary>
        /// All the subdirectories of a node.
        /// </summary>
        public IDictionary<string, AsyncApiUrlTreeNode> Children { get; } = new Dictionary<string, AsyncApiUrlTreeNode>();

        /// <summary>
        /// The relative directory path of the current node from the root node.
        /// </summary>
        public string Path { get; private set; } = "";

        /// <summary>
        /// Dictionary of labels and Path Item objects that describe the operations available on a node.
        /// </summary>
        public IDictionary<string, AsyncApiPathItem> PathItems { get; } = new Dictionary<string, AsyncApiPathItem>();

        /// <summary>
        /// A dictionary of key value pairs that contain information about a node.
        /// </summary>
        public IDictionary<string, List<string>> AdditionalData { get; set; } = new Dictionary<string, List<string>>();

        /// <summary>
        /// Flag indicating whether a node segment is a path parameter.
        /// </summary>
        public bool IsParameter => Segment.StartsWith("{");

        /// <summary>
        /// The subdirectory of a relative path.
        /// </summary>
        public string Segment { get; private set; }

        /// <summary>
        /// Flag indicating whether the node's PathItems dictionary has operations
        /// under a given label.
        /// </summary>
        /// <param name="label">The name of the key for the target operations
        /// in the node's PathItems dictionary.</param>
        /// <returns>true or false.</returns>
        public bool HasOperations(string label)
        {
            Utils.CheckArgumentNullOrEmpty(label, nameof(label));

            if (!(PathItems?.ContainsKey(label) ?? false))
            {
                return false;
            }

            return PathItems[label].Operations?.Any() ?? false;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="segment">The subdirectory of a relative path.</param>
        private AsyncApiUrlTreeNode(string segment)
        {
            Segment = segment;
        }

        /// <summary>
        /// Creates an empty structured directory of <see cref="AsyncApiUrlTreeNode"/> node.
        /// </summary>
        /// <returns>The root node of the created <see cref="AsyncApiUrlTreeNode"/> directory structure.</returns>
        public static AsyncApiUrlTreeNode Create()
        {
            return new AsyncApiUrlTreeNode(RootPathSegment);
        }

        /// <summary>
        /// Appends a path and the PathItem to an <see cref="AsyncApiUrlTreeNode"/> node.
        /// </summary>
        /// <param name="path">An AsyncAPI path.</param>
        /// <param name="pathItem">Path Item object that describes the operations available on an AsyncAPI path.</param>
        /// <param name="label">A name tag for labelling the <see cref="AsyncApiUrlTreeNode"/> node.</param>
        /// <returns>An <see cref="AsyncApiUrlTreeNode"/> node describing an AsyncAPI path.</returns>
        public AsyncApiUrlTreeNode Attach(string path,
                                         AsyncApiPathItem pathItem,
                                         string label)
        {
            Utils.CheckArgumentNullOrEmpty(label, nameof(label));
            Utils.CheckArgumentNullOrEmpty(path, nameof(path));
            Utils.CheckArgumentNull(pathItem, nameof(pathItem));

            if (path.StartsWith(RootPathSegment))
            {
                // Remove leading slash
                path = path.Substring(1);
            }

            var segments = path.Split('/');

            return Attach(segments: segments,
                          pathItem: pathItem,
                          label: label,
                          currentPath: "");
        }

        /// <summary>
        /// Assembles the constituent properties of an <see cref="AsyncApiUrlTreeNode"/> node.
        /// </summary>
        /// <param name="segments">IEnumerable subdirectories of a relative path.</param>
        /// <param name="pathItem">Path Item object that describes the operations available on an AsyncAPI path.</param>
        /// <param name="label">A name tag for labelling the <see cref="AsyncApiUrlTreeNode"/> node.</param>
        /// <param name="currentPath">The relative path of a node.</param>
        /// <returns>An <see cref="AsyncApiUrlTreeNode"/> node with all constituent properties assembled.</returns>
        private AsyncApiUrlTreeNode Attach(IEnumerable<string> segments,
                                          AsyncApiPathItem pathItem,
                                          string label,
                                          string currentPath)
        {
            var segment = segments.FirstOrDefault();
            if (string.IsNullOrEmpty(segment))
            {
                if (PathItems.ContainsKey(label))
                {
                    throw new ArgumentException("A duplicate label already exists for this node.", nameof(label));
                }

                Path = currentPath;
                PathItems.Add(label, pathItem);
                return this;
            }

            // If the child segment has already been defined, then insert into it
            if (Children.ContainsKey(segment))
            {
                var newPath = currentPath + PathSeparator + segment;

                return Children[segment].Attach(segments: segments.Skip(1),
                                                pathItem: pathItem,
                                                label: label,
                                                currentPath: newPath);
            }
            else
            {
                var newPath = currentPath + PathSeparator + segment;

                var node = new AsyncApiUrlTreeNode(segment)
                {
                    Path = newPath
                };

                Children[segment] = node;

                return node.Attach(segments: segments.Skip(1),
                                   pathItem: pathItem,
                                   label: label,
                                   currentPath: newPath);
            }
        }

        /// <summary>
        /// Adds additional data information to the AdditionalData property of the node.
        /// </summary>
        /// <param name="additionalData">A dictionary of key value pairs that contain information about a node.</param>
        public void AddAdditionalData(Dictionary<string, List<string>> additionalData)
        {
            Utils.CheckArgumentNull(additionalData, nameof(additionalData));

            foreach (var item in additionalData)
            {
                if (AdditionalData.ContainsKey(item.Key))
                {
                    AdditionalData[item.Key] = item.Value;
                }
                else
                {
                    AdditionalData.Add(item.Key, item.Value);
                }
            }
        }
    }
}
