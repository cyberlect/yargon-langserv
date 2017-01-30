using System;
using JetBrains.Annotations;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// A JSON RPC "Invalid params" exception.
    /// </summary>
    public sealed class InvalidParamsException : JsonRpcException
    {
        private const long ErrorCode = -32602;
        private const string DefaultMessage = "Invalid params";

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidParamsException"/> class.
        /// </summary>
        public InvalidParamsException()
            : this(null, null)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidParamsException"/> class.
        /// </summary>
        public InvalidParamsException([CanBeNull] object data)
            : this(null, data, null)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidParamsException"/> class.
        /// </summary>
        /// <param name="message">The error message; or <see langword="null"/>.</param>
        /// <param name="data">The error data; or <see langword="null"/>.</param>
        public InvalidParamsException([CanBeNull] string message, [CanBeNull] object data)
            : this(message, data, null)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidParamsException"/> class.
        /// </summary>
        /// <param name="message">The error message; or <see langword="null"/>.</param>
        /// <param name="data">The error data; or <see langword="null"/>.</param>
        /// <param name="inner">The exception that caused this exception; or <see langword="null"/>.</param>
        public InvalidParamsException([CanBeNull] string message, [CanBeNull] object data, [CanBeNull] Exception inner)
            : base(ErrorCode, message ?? DefaultMessage, data, inner)
        {
            // Nothing to do.
        }
    }
}
