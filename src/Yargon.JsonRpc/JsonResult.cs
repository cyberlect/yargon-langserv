using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// A JSON result.
    /// </summary>
    public sealed class JsonResult : JsonResponse
    {
        /// <summary>
        /// Gets the result object.
        /// </summary>
        /// <value>The result object.</value>
        [JsonProperty("result")]
        public JToken Result { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonResult"/> class.
        /// </summary>
        /// <param name="id">The message identifier, which is either a string, a number, or <see langword="null"/>.</param>
        /// <param name="result">The result object.</param>
        /// <param name="jsonrpc">The JSON RPC protocol version string.</param>
        [JsonConstructor]
        internal JsonResult([CanBeNull] object id, JToken result, string jsonrpc)
            : base(id, jsonrpc)
        {
            #region Contract
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            #endregion

            this.Result = result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonResult"/> class.
        /// </summary>
        /// <param name="id">The message identifier, which is either a string, a number, or <see langword="null"/>.</param>
        /// <param name="result">The result object.</param>
        internal JsonResult([CanBeNull] object id, JToken result)
            : this(id, result, DefaultProtocolVersion)
        {
            // Nothing to do.
        }
        #endregion
        
        #region Equality
        /// <inheritdoc />
        public bool Equals(JsonResult other)
        {
            return base.Equals(other)
                && JToken.DeepEquals(this.Result, other.Result);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = base.GetHashCode();
            unchecked
            {
                hash = hash * 29 + this.Result.GetHashCode();
            }
            return hash;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as JsonResult);
        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return this.Result.ToString();
        }
    }
}
