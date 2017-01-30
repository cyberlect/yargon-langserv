using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// A JSON message.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class JsonMessage
    {
        /// <summary>
        /// Gets the default JSON RPC protocol version string.
        /// </summary>
        /// <value>The default version string.</value>
        public static string DefaultProtocolVersion => "2.0";

        /// <summary>
        /// Gets the JSON RPC protocol version.
        /// </summary>
        /// <value>The version string.</value>
        [JsonProperty("jsonrpc")]
        public string ProtocolVersion { get; }

        /// <summary>
        /// Gets the message identifier.
        /// </summary>
        /// <value>The identifier string; or <see langword="null"/>.</value>
        [CanBeNull]
        [JsonProperty("id")]
        public string Id { get; }

        // NOTE: We treat any message where `id=null` as a notification.
        // This is not strictly correct according to the specification,
        // as it distinguishes between `id=null` and `id` is absent, where
        // only the latter should be treated as a notification.

        /// <summary>
        /// Gets whether this message is a notification.
        /// </summary>
        /// <value><see langword="true"/> when this message is a notification;
        /// otherwise, <see langword="false"/>.</value>
        public bool IsNotification => this.Id == null;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonMessage"/> class.
        /// </summary>
        /// <param name="id">The message identifier string; or <see langword="null"/>.</param>
        /// <param name="jsonrpc">The JSON RPC protocol version.</param>
        internal JsonMessage([CanBeNull] string id, string jsonrpc)
        {
            #region Contract
            if (!IsValidProtocolVersion(jsonrpc))
                throw new ArgumentException("The JSON RPC protocol version string is invalid.", nameof(jsonrpc));
            #endregion
            
            this.Id = id;
            this.ProtocolVersion = jsonrpc ?? DefaultProtocolVersion;
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public bool Equals(JsonMessage other)
        {
            if (Object.ReferenceEquals(other, null))
                return false;
            return this.ProtocolVersion == other.ProtocolVersion
                && this.Id == other.Id;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.ProtocolVersion.GetHashCode();
                hash = hash * 29 + this.Id?.GetHashCode() ?? 0;
            }
            return hash;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as JsonMessage);
        #endregion

        /// <summary>
        /// Determines whether the specified JSON RPC protocol version is valid.
        /// </summary>
        /// <param name="version">The version string.</param>
        /// <returns><see langword="true"/> when the version string is valid;
        /// otherwise, <see langword="false"/>.</returns>
        [Pure]
        public static bool IsValidProtocolVersion(string version)
        {
            return version == "2.0";
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.Id ?? "-";
        }
    }
}
