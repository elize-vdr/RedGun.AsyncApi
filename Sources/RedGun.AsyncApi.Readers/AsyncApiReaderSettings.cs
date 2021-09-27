// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Readers.Interface;
using RedGun.AsyncApi.Readers.ParseNodes;
using RedGun.AsyncApi.Readers.Services;
using RedGun.AsyncApi.Validations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedGun.AsyncApi.Readers
{
    /// <summary>
    /// Indicates if and when the reader should convert unresolved references into resolved objects
    /// </summary>
    public enum ReferenceResolutionSetting
    {
        /// <summary>
        /// Create placeholder objects with an AsyncApiReference instance and UnresolvedReference set to true.
        /// </summary>
        DoNotResolveReferences,
        /// <summary>
        /// Convert local references to references of valid domain objects.
        /// </summary>
        ResolveLocalReferences,
        /// <summary>
        /// Convert all references to references of valid domain objects.
        /// </summary>
        ResolveAllReferences
    }

    /// <summary>
    /// Configuration settings to control how AsyncAPI documents are parsed
    /// </summary>
    public class AsyncApiReaderSettings
    {
        /// <summary>
        /// Indicates how references in the source document should be handled.
        /// </summary>
        public ReferenceResolutionSetting ReferenceResolution { get; set; } = ReferenceResolutionSetting.ResolveLocalReferences;

        /// <summary>
        /// Dictionary of parsers for converting extensions into strongly typed classes
        /// </summary>
        public Dictionary<string, Func<IAsyncApiAny, AsyncApiSpecVersion, IAsyncApiExtension>> ExtensionParsers { get; set; } = new Dictionary<string, Func<IAsyncApiAny, AsyncApiSpecVersion, IAsyncApiExtension>>();

        /// <summary>
        /// Rules to use for validating AsyncAPI specification.  If none are provided a default set of rules are applied.
        /// </summary>
        public ValidationRuleSet RuleSet { get; set; } = ValidationRuleSet.GetDefaultRuleSet();

        /// <summary>
        /// URL where relative references should be resolved from if the description does not contain Server definitions
        /// </summary>
        public Uri BaseUrl { get; set; }

        /// <summary>
        /// Function used to provide an alternative loader for accessing external references.
        /// </summary>
        /// <remarks>
        /// Default loader will attempt to dereference http(s) urls and file urls.
        /// </remarks>
        public IStreamLoader CustomExternalLoader { get; set; }

        /// <summary>
        /// Whether to leave the <see cref="Stream"/> object open after reading
        /// from an <see cref="AsyncApiStreamReader"/> object.
        /// </summary>
        public bool LeaveStreamOpen { get; set; }
    }
}
