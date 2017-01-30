using System;
using System.Collections.Generic;
using System.Linq;
using Virtlink.Utilib.Collections;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// A batch of JSON messages.
    /// </summary>
    public sealed partial class JsonBatch<T>
        where T : JsonMessage
    {
        /// <summary>
        /// Gets the messages in the batch.
        /// </summary>
        /// <value>The messages, each of which may be <see langword="null"/>.</value>
        public IReadOnlyList<T> Messages { get; }

        /// <summary>
        /// Gets whether the batch was actually just a single message.
        /// </summary>
        /// <value><see langword="true"/> when the batch was actually just a single message;
        /// otherwise, <see langword="false"/>.</value>
        public bool IsSingleton { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonBatch{T}"/> class.
        /// </summary>
        /// <param name="messages">The JSON messages in the batch.</param>
        public JsonBatch(IEnumerable<T> messages)
        {
            #region Contract
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));
            #endregion

            this.Messages = messages.ToArray();
            this.IsSingleton = (this.Messages.Count == 1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonBatch{T}"/> class.
        /// </summary>
        /// <param name="messages">The JSON messages in the batch.</param>
        /// <param name="isSingleton">Whether the batch is actually just a single message.</param>
        internal JsonBatch(IReadOnlyList<T> messages, bool isSingleton)
        {
            #region Contract
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));
            if (isSingleton && messages.Count != 1)
                throw new ArgumentException("The singleton batch must contain exactly one message.", nameof(messages));
            #endregion

            this.Messages = messages;
            this.IsSingleton = isSingleton;
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public bool Equals(JsonBatch<T> other)
        {
            if (Object.ReferenceEquals(other, null) ||      // When 'other' is null
                other.GetType() != this.GetType())          // or of a different type
                return false;                               // they are not equal.
            return ListComparer<T>.Default.Equals(this.Messages, other.Messages)
                && this.IsSingleton == other.IsSingleton;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + ListComparer<T>.Default.GetHashCode(this.Messages);
                hash = hash * 29 + this.IsSingleton.GetHashCode();
            }
            return hash;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as JsonBatch<T>);
        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            if (this.IsSingleton)
                return this.Messages.Single().ToString();
            else
                return "[" + String.Join(", ", this.Messages) + "]";
        }
    }
}
