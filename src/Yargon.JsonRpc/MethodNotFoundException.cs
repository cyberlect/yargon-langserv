using System;
using JetBrains.Annotations;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// A JSON RPC "Method not found" exception.
    /// </summary>
    public sealed class MethodNotFoundException : JsonRpcException
    {
        private const long ErrorCode = -32601;
        private const string DefaultMessage = "Method not found";

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodNotFoundException"/> class.
        /// </summary>
        public MethodNotFoundException()
            : this(null, null)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodNotFoundException"/> class.
        /// </summary>
        public MethodNotFoundException([CanBeNull] object data)
            : this(null, data, null)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The error message; or <see langword="null"/>.</param>
        /// <param name="data">The error data; or <see langword="null"/>.</param>
        public MethodNotFoundException([CanBeNull] string message, [CanBeNull] object data)
            : this(message, data, null)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The error message; or <see langword="null"/>.</param>
        /// <param name="data">The error data; or <see langword="null"/>.</param>
        /// <param name="inner">The exception that caused this exception; or <see langword="null"/>.</param>
        public MethodNotFoundException([CanBeNull] string message, [CanBeNull] object data, [CanBeNull] Exception inner)
            : base(ErrorCode, message ?? DefaultMessage, data, inner)
        {
            // Nothing to do.
        }
    }
}
