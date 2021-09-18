// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RedGun.AsyncApi.Exceptions;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.Interface;
using RedGun.AsyncApi.Readers.Services;
using RedGun.AsyncApi.Services;
using SharpYaml.Serialization;

namespace RedGun.AsyncApi.Readers
{
    /// <summary>
    /// Service class for converting contents of TextReader into AsyncApiDocument instances
    /// </summary>
    internal class AsyncApiYamlDocumentReader : IAsyncApiReader<YamlDocument, AsyncApiDiagnostic>
    {
        private readonly AsyncApiReaderSettings _settings;

        /// <summary>
        /// Create stream reader with custom settings if desired.
        /// </summary>
        /// <param name="settings"></param>
        public AsyncApiYamlDocumentReader(AsyncApiReaderSettings settings = null)
        {
            _settings = settings ?? new AsyncApiReaderSettings();
        }

        /// <summary>
        /// Reads the stream input and parses it into an Async API document.
        /// </summary>
        /// <param name="input">TextReader containing OpenAPI description to parse.</param>
        /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing</param>
        /// <returns>Instance of newly created AsyncApiDocument</returns>
        public AsyncApiDocument Read(YamlDocument input, out AsyncApiDiagnostic diagnostic)
        {
            diagnostic = new AsyncApiDiagnostic();
            var context = new ParsingContext(diagnostic)
            {
                ExtensionParsers = _settings.ExtensionParsers,
                BaseUrl = _settings.BaseUrl
            };

            AsyncApiDocument document = null;
            try
            {
                // Parse the OpenAPI Document
                document = context.Parse(input);

                ResolveReferences(diagnostic, document);
            }
            catch (OpenApiException ex)
            {
                diagnostic.Errors.Add(new OpenApiError(ex));
            }

            // Validate the document
            if (_settings.RuleSet != null && _settings.RuleSet.Rules.Count > 0)
            {
                var errors = document.Validate(_settings.RuleSet);
                foreach (var item in errors)
                {
                    diagnostic.Errors.Add(item);
                }
            }

            return document;
        }

        public async Task<ReadResult> ReadAsync(YamlDocument input)
        {
            var diagnostic = new AsyncApiDiagnostic();
            var context = new ParsingContext(diagnostic)
            {
                ExtensionParsers = _settings.ExtensionParsers,
                BaseUrl = _settings.BaseUrl
            };

            AsyncApiDocument document = null;
            try
            {
                // Parse the OpenAPI Document
                document = context.Parse(input);

                await ResolveReferencesAsync(diagnostic, document);
            }
            catch (OpenApiException ex)
            {
                diagnostic.Errors.Add(new OpenApiError(ex));
            }

            // Validate the document
            if (_settings.RuleSet != null && _settings.RuleSet.Rules.Count > 0)
            {
                var errors = document.Validate(_settings.RuleSet);
                foreach (var item in errors)
                {
                    diagnostic.Errors.Add(item);
                }
            }

            return new ReadResult()
            {
                AsyncApiDocument = document,
                AsyncApiDiagnostic = diagnostic
            };
        }


        private void ResolveReferences(AsyncApiDiagnostic diagnostic, AsyncApiDocument document)
        {
            // Resolve References if requested
            switch (_settings.ReferenceResolution)
            {
                case ReferenceResolutionSetting.ResolveAllReferences:
                    throw new ArgumentException("Cannot resolve all references via a synchronous call. Use ReadAsync.");
                case ReferenceResolutionSetting.ResolveLocalReferences:
                    var errors = document.ResolveReferences(false);

                    foreach (var item in errors)
                    {
                        diagnostic.Errors.Add(item);
                    }
                    break;
                case ReferenceResolutionSetting.DoNotResolveReferences:
                    break;
            }
        }

        private async Task ResolveReferencesAsync(AsyncApiDiagnostic diagnostic, AsyncApiDocument document)
        {
            List<OpenApiError> errors = new List<OpenApiError>();

            // Resolve References if requested
            switch (_settings.ReferenceResolution)
            {
                case ReferenceResolutionSetting.ResolveAllReferences:

                    // Create workspace for all documents to live in.
                    var openApiWorkSpace = new AsyncApiWorkspace();

                    // Load this root document into the workspace
                    var streamLoader = new DefaultStreamLoader();
                    var workspaceLoader = new AsyncApiWorkspaceLoader(openApiWorkSpace, _settings.CustomExternalLoader ?? streamLoader, _settings);
                    await workspaceLoader.LoadAsync(new AsyncApiReference() { ExternalResource = "/" }, document);

                    // Resolve all references in all the documents loaded into the AsyncApiWorkspace
                    foreach (var doc in openApiWorkSpace.Documents)
                    {
                        errors.AddRange(doc.ResolveReferences(true));
                    }
                    break;
                case ReferenceResolutionSetting.ResolveLocalReferences:
                    errors.AddRange(document.ResolveReferences(false));
                    break;
                case ReferenceResolutionSetting.DoNotResolveReferences:
                    break;
            }

            foreach (var item in errors)
            {
                diagnostic.Errors.Add(item);
            }
        }


        /// <summary>
        /// Reads the stream input and parses the fragment of an OpenAPI description into an Async API Element.
        /// </summary>
        /// <param name="input">TextReader containing OpenAPI description to parse.</param>
        /// <param name="version">Version of the OpenAPI specification that the fragment conforms to.</param>
        /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing</param>
        /// <returns>Instance of newly created AsyncApiDocument</returns>
        public T ReadFragment<T>(YamlDocument input, OpenApiSpecVersion version, out AsyncApiDiagnostic diagnostic) where T : IAsyncApiElement
        {
            diagnostic = new AsyncApiDiagnostic();
            var context = new ParsingContext(diagnostic)
            {
                ExtensionParsers = _settings.ExtensionParsers
            };

            IAsyncApiElement element = null;
            try
            {
                // Parse the OpenAPI element
                element = context.ParseFragment<T>(input, version);
            }
            catch (OpenApiException ex)
            {
                diagnostic.Errors.Add(new OpenApiError(ex));
            }

            // Validate the element
            if (_settings.RuleSet != null && _settings.RuleSet.Rules.Count > 0)
            {
                var errors = element.Validate(_settings.RuleSet);
                foreach (var item in errors)
                {
                    diagnostic.Errors.Add(item);
                }
            }

            return (T)element;
        }
    }
}
