using System;
using JetBrains.Annotations;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// A JSON RPC "Internal error" exception.
    /// </summary>
    public sealed class JsonRpcInternalException : JsonRpcException
    {
        private const long ErrorCode = -32603;
        private const string DefaultMessage = "Internal error";

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonRpcInternalException"/> class.
        /// </summary>
        public JsonRpcInternalException()
            : this(null, null)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonRpcInternalException"/> class.
        /// </summary>
        public JsonRpcInternalException([CanBeNull] object data)
            : this(null, data, null)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonRpcInternalException"/> class.
        /// </summary>
        /// <param name="message">The error message; or <see langword="null"/>.</param>
        /// <param name="data">The error data; or <see langword="null"/>.</param>
        public JsonRpcInternalException([CanBeNull] string message, [CanBeNull] object data)
            : this(message, data, null)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonRpcInternalException"/> class.
        /// </summary>
        /// <param name="message">The error message; or <see langword="null"/>.</param>
        /// <param name="data">The error data; or <see langword="null"/>.</param>
        /// <param name="inner">The exception that caused this exception; or <see langword="null"/>.</param>
        public JsonRpcInternalException([CanBeNull] string message, [CanBeNull] object data, [CanBeNull] Exception inner)
            : base(ErrorCode, message ?? DefaultMessage, data, inner)
        {
            // Nothing to do.
        }
    }
}
