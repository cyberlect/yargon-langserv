using System;
using JetBrains.Annotations;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// A JSON RPC "Invalid request" exception.
    /// </summary>
    public sealed class InvalidRequestException : JsonRpcException
    {
        private const long ErrorCode = -32600;
        private const string DefaultMessage = "Invalid request";

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRequestException"/> class.
        /// </summary>
        public InvalidRequestException()
            : this(null, null)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRequestException"/> class.
        /// </summary>
        public InvalidRequestException([CanBeNull] object data)
            : this(null, data, null)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRequestException"/> class.
        /// </summary>
        /// <param name="message">The error message; or <see langword="null"/>.</param>
        /// <param name="data">The error data; or <see langword="null"/>.</param>
        public InvalidRequestException([CanBeNull] string message, [CanBeNull] object data)
            : this(message, data, null)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRequestException"/> class.
        /// </summary>
        /// <param name="message">The error message; or <see langword="null"/>.</param>
        /// <param name="data">The error data; or <see langword="null"/>.</param>
        /// <param name="inner">The exception that caused this exception; or <see langword="null"/>.</param>
        public InvalidRequestException([CanBeNull] string message, [CanBeNull] object data, [CanBeNull] Exception inner)
            : base(ErrorCode, message ?? DefaultMessage, data, inner)
        {
            // Nothing to do.
        }
    }
}
