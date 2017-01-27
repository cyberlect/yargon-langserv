using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// A JSON error.
    /// </summary>
    public class JsonError : JsonResponse
    {
        /// <summary>
        /// Gets the error object.
        /// </summary>
        /// <value>The error object.</value>
        [JsonProperty("error")]
        public JsonErrorObject Error { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonError"/> class.
        /// </summary>
        /// <param name="id">The message identifier, which is either a string, a number, or <see langword="null"/>.</param>
        /// <param name="error">The error object.</param>
        /// <param name="jsonrpc">The JSON RPC protocol version; or <see langword="null"/> to use the default.</param>
        [JsonConstructor]
        internal JsonError([CanBeNull] object id, JsonErrorObject error, [CanBeNull] string jsonrpc)
            : base(id, jsonrpc)
        {
            #region Contract
            if (error == null)
                throw new ArgumentNullException(nameof(error));
            #endregion

            this.Error = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonError"/> class.
        /// </summary>
        /// <param name="id">The message identifier, which is either a string, a number, or <see langword="null"/>.</param>
        /// <param name="error">The error object.</param>
        internal JsonError([CanBeNull] object id, JsonErrorObject error)
            : this(id, error, null)
        {
            // Nothing to do.
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public bool Equals(JsonError other)
        {
            return base.Equals(other)
                && this.Error.Equals(other.Error);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = base.GetHashCode();
            unchecked
            {
                hash = hash * 29 + this.Error.GetHashCode();
            }
            return hash;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as JsonError);
        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return this.Error.ToString();
        }
    }
}
