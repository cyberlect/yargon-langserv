﻿using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Virtlink.Utilib;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// A JSON message.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class JsonMessage
    {
        /// <summary>
        /// Gets the message identifier.
        /// </summary>
        /// <value>The identifier, which is either a string, a number, or <see langword="null"/>.</value>
        [CanBeNull]
        [JsonProperty("id")]
        public object Id { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonMessage"/> class.
        /// </summary>
        /// <param name="id">The message identifier, which is either a string, a number, or <see langword="null"/>.</param>
        protected JsonMessage([CanBeNull] object id)
        {
            #region Contract
            if (!IsValidIdentifier(id))
                throw new ArgumentException("The identifier is not valid. It must either be a string, a number, or null.", nameof(id));
            #endregion
            
            this.Id = id;
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public bool Equals(JsonMessage other)
        {
            if (Object.ReferenceEquals(other, null))
                return false;
            return CompareIds(this.Id, other.Id);
        }

        /// <summary>
        /// Compares two IDs.
        /// </summary>
        /// <param name="thisId">This ID.</param>
        /// <param name="otherId">The other ID.</param>
        /// <returns><see langword="true"/> when the IDs are equal;
        /// otherwise, <see langword="false"/>.</returns>
        private static bool CompareIds(object thisId, object otherId)
        {
            if (Numeric.HasNumericType(thisId) && Numeric.HasNumericType(otherId))
                return Numeric.Compare(thisId, otherId) == 0;
            else
                return Object.Equals(thisId, otherId);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Id?.GetHashCode() ?? 0;
            }
            return hash;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as JsonMessage);
        #endregion
        
        /// <summary>
        /// Determines whether the specified identifier is valid.
        /// </summary>
        /// <param name="id">The identifier to check.</param>
        /// <returns><see langword="true"/> when the identifier is valid;
        /// otherwise, <see langword="false"/>.</returns>
        [Pure]
        public static bool IsValidIdentifier(object id)
        {
            return id == null
                || Numeric.HasIntegerType(id)
                || (id is string && (string)id != "");
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"id: {this.Id}";
        }
    }
}