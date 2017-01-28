using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// A JSON RPC request.
    /// </summary>
    public sealed class JsonRequest : JsonMessage
    {
        /// <summary>
        /// Gets the name of the method to be invoked.
        /// </summary>
        /// <value>The method name.</value>
        [JsonProperty("method")]
        public string Method { get; }

        /// <summary>
        /// Gets the arguments to the method invocation.
        /// </summary>
        /// <value>The arguments; or a null <see cref="JValue"/>.</value>
        [JsonProperty("params")]
        public JToken Parameters { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonRequest"/> class.
        /// </summary>
        /// <param name="id">The message identifier string; or <see langword="null"/>.</param>
        /// <param name="method">The method name.</param>
        /// <param name="parameters">The method arguments; or <see langword="null"/>.</param>
        /// <param name="jsonrpc">The JSON RPC protocol version string.</param>
        [JsonConstructor]
        public JsonRequest([CanBeNull] string id, string method, [CanBeNull] JToken parameters, string jsonrpc)
            : base(id, jsonrpc)
        {
            #region Contract
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            if (!IsValidMethodName(method))
                throw new ArgumentException("The method name is not valid.", nameof(method));
            #endregion

            this.Method = method;
            this.Parameters = parameters ?? JValue.CreateNull();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonRequest"/> class.
        /// </summary>
        /// <param name="id">The message identifier string; or <see langword="null"/>.</param>
        /// <param name="method">The method name.</param>
        /// <param name="parameters">The method arguments; or <see langword="null"/>.</param>
        public JsonRequest([CanBeNull] string id, string method, [CanBeNull] JToken parameters)
            : this(id, method, parameters, DefaultProtocolVersion)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonRequest"/> class.
        /// </summary>
        /// <param name="id">The message identifier string; or <see langword="null"/>.</param>
        /// <param name="method">The method name.</param>
        public JsonRequest([CanBeNull] string id, string method)
            : this(id, method, null)
        {
            // Nothing to do.
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public bool Equals(JsonRequest other)
        {
            return base.Equals(other)
                && this.Method == other.Method
                && JToken.DeepEquals(this.Parameters, other.Parameters);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = base.GetHashCode();
            unchecked
            {
                hash = hash * 29 + this.Method.GetHashCode();
                hash = hash * 29 + this.Parameters?.GetHashCode() ?? 0;
            }
            return hash;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as JsonRequest);
        #endregion

        /// <summary>
        /// Determines whether the specified method name is valid.
        /// </summary>
        /// <param name="name">The method name to check.</param>
        /// <returns><see langword="true"/> when the method name is valid;
        /// otherwise, <see langword="false"/>.</returns>
        [Pure]
        public static bool IsValidMethodName(string name)
        {
            return !String.IsNullOrEmpty(name)
                && !name.StartsWith("rpc.");
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.Method;
        }
    }
}
