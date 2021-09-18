// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Expressions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// The wrapper either for <see cref="IAsyncApiAny"/> or <see cref="RuntimeExpression"/>
    /// </summary>
    public class RuntimeExpressionAnyWrapper : IAsyncApiElement
    {
        private IAsyncApiAny _any;
        private RuntimeExpression _expression;

        /// <summary>
        /// Gets/Sets the <see cref="IAsyncApiAny"/>
        /// </summary>
        public IAsyncApiAny Any
        {
            get
            {
                return _any;
            }
            set
            {
                _expression = null;
                _any = value;
            }
        }

        /// <summary>
        /// Gets/Set the <see cref="RuntimeExpression"/>
        /// </summary>
        public RuntimeExpression Expression
        {
            get
            {
                return _expression;
            }
            set
            {
                _any = null;
                _expression = value;
            }
        }

        /// <summary>
        /// Write <see cref="RuntimeExpressionAnyWrapper"/>
        /// </summary>
        public void WriteValue(IOpenApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            if (_any != null)
            {
                writer.WriteAny(_any);
            }
            else if (_expression != null)
            {
                writer.WriteValue(_expression.Expression);
            }
        }
    }
}
