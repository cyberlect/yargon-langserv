using System;
using JetBrains.Annotations;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// A JSON RPC "Parse error" exception.
    /// </summary>
    public sealed class ParseException : JsonRpcException
    {
        private const long ErrorCode = -32700;
        private const string DefaultMessage = "Parse error";

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException"/> class.
        /// </summary>
        public ParseException()
            : this(null, null)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException"/> class.
        /// </summary>
        public ParseException([CanBeNull] object data)
            : this(null, data, null)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException"/> class.
        /// </summary>
        /// <param name="message">The error message; or <see langword="null"/>.</param>
        /// <param name="data">The error data; or <see langword="null"/>.</param>
        public ParseException([CanBeNull] string message, [CanBeNull] object data)
            : this(message, data, null)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException"/> class.
        /// </summary>
        /// <param name="message">The error message; or <see langword="null"/>.</param>
        /// <param name="data">The error data; or <see langword="null"/>.</param>
        /// <param name="inner">The exception that caused this exception; or <see langword="null"/>.</param>
        public ParseException([CanBeNull] string message, [CanBeNull] object data, [CanBeNull] Exception inner)
            : base(ErrorCode, message ?? DefaultMessage, data, inner)
        {
            // Nothing to do.
        }
    }
}
