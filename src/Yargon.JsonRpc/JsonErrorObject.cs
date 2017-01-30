using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// A JSON error object.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class JsonErrorObject
    {
        /// <summary>
        /// Gets the error code.
        /// </summary>
        /// <value>The error code.</value>
        [JsonProperty("code")]
        public long Code { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>The error message.</value>
        [JsonProperty("message")]
        public string Message { get; }

        /// <summary>
        /// Gets the additional information about the error.
        /// </summary>
        /// <value>Additional information; or a null <see cref="JValue"/>.</value>
        [JsonProperty("data")]
        public JToken Data { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonErrorObject"/> class.
        /// </summary>
        /// <param name="code">The error code.</param>
        /// <param name="message">The error message.</param>
        /// <param name="data">Additional data; or <see langword="null"/>.</param>
        [JsonConstructor]
        public JsonErrorObject(long code, string message, [CanBeNull] JToken data)
        {
            #region Contract
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            #endregion
            
            this.Code = code;
            this.Message = message;
            this.Data = data ?? JValue.CreateNull();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonErrorObject"/> class.
        /// </summary>
        /// <param name="code">The error code.</param>
        /// <param name="message">The error message.</param>
        public JsonErrorObject(long code, string message)
            : this(code, message, null)
        {
            // Nothing to do.
        }
        #endregion

            #region Equality
            /// <inheritdoc />
        public bool Equals(JsonErrorObject other)
        {
            if (Object.ReferenceEquals(other, null))
                return false;
            return this.Code == other.Code
                && this.Message == other.Message
                && JToken.DeepEquals(this.Data, other.Data);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Code.GetHashCode();
                hash = hash * 29 + this.Message.GetHashCode();
                hash = hash * 29 + this.Data?.GetHashCode() ?? 0;
            }
            return hash;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as JsonErrorObject);
        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            if (this.Data != null)
                return $"{this.Code}: {this.Message} -- {this.Data}";
            else
                return $"{this.Code}: {this.Message}";
        }
    }
}
