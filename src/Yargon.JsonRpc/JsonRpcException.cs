using System;
using JetBrains.Annotations;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// A JSON RPC exception.
    /// </summary>
    public class JsonRpcException : InvalidOperationException
    {
        private const string DefaultMessage = "Unspecified error";

        /// <summary>
        /// Gets the error code.
        /// </summary>
        /// <value>The error code.</value>
        public long Code { get; }

        /// <summary>
        /// Gets the exception data.
        /// </summary>
        /// <value>The exception data; or <see langword="null"/>.</value>
        [CanBeNull]
        public new object Data { get; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonRpcException"/> class.
        /// </summary>
        /// <param name="code">The error code.</param>
        /// <param name="message">The error message; or <see langword="null"/>.</param>
        public JsonRpcException(long code, [CanBeNull] string message)
            : this(code, message, null)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonRpcException"/> class.
        /// </summary>
        /// <param name="code">The error code.</param>
        /// <param name="message">The error message; or <see langword="null"/>.</param>
        /// <param name="data">The error data; or <see langword="null"/>.</param>
        public JsonRpcException(long code, [CanBeNull] string message, [CanBeNull] object data)
            : this(code, message, data, null)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonRpcException"/> class.
        /// </summary>
        /// <param name="code">The error code.</param>
        /// <param name="message">The error message; or <see langword="null"/>.</param>
        /// <param name="data">The error data; or <see langword="null"/>.</param>
        /// <param name="inner">The exception that caused this exception; or <see langword="null"/>.</param>
        public JsonRpcException(long code, [CanBeNull] string message, [CanBeNull] object data, [CanBeNull] Exception inner)
            : base(message ?? DefaultMessage, inner)
        {
            this.Code = code;
            this.Data = data;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.Code}: {this.Message}";
        }
    }
}
