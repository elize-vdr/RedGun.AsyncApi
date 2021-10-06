// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Exceptions;
using SharpYaml.Serialization;

namespace RedGun.AsyncApi.Readers.Exceptions
{
    /// <summary>
    /// Defines an exception indicating AsyncAPI Reader encountered an issue while reading.
    /// </summary>
    [Serializable]
    public class AsyncApiReaderException : AsyncApiException
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiReaderException"/> class.
        /// </summary>
        public AsyncApiReaderException() { }

        /// <summary>
        /// Initializes the <see cref="AsyncApiReaderException"/> class with a custom message.
        /// </summary>
        /// <param name="message">Plain text error message for this exception.</param>
        public AsyncApiReaderException(string message) : base(message) { }

        /// <summary>
        /// Initializes the <see cref="AsyncApiReaderException"/> class with a custom message.
        /// </summary>
        /// <param name="message">Plain text error message for this exception.</param>
        /// <param name="context">Context of current parsing process.</param>
        public AsyncApiReaderException(string message, ParsingContext context) : base(message) {
            Pointer = context.GetLocation();
        }

        /// <summary>
        /// Initializes the <see cref="AsyncApiReaderException"/> class with a message and line, column location of error.
        /// </summary>
        /// <param name="message">Plain text error message for this exception.</param>
        /// <param name="node">Parsing node where error occured</param>
        public AsyncApiReaderException(string message, YamlNode node) : base(message)
        {
            // This only includes line because using a char range causes tests to break due to CR/LF & LF differences
            // See https://tools.ietf.org/html/rfc5147 for syntax
            Pointer = $"#line={node.Start.Line}";
        }

        /// <summary>
        /// Initializes the <see cref="AsyncApiReaderException"/> class with a custom message and inner exception.
        /// </summary>
        /// <param name="message">Plain text error message for this exception.</param>
        /// <param name="innerException">Inner exception that caused this exception to be thrown.</param>
        public AsyncApiReaderException(string message, Exception innerException) : base(message, innerException) { }

    }
}
