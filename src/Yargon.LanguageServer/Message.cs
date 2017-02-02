using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Yargon.JsonRpc;

namespace Yargon.LanguageServer
{
    /// <summary>
    /// An LSP message.
    /// </summary>
    public sealed class Message
    {
        /// <summary>
        /// Gets the JSON RPC message.
        /// </summary>
        /// <value>The JSON RPC message.</value>
        public JsonMessage Content { get; }

        // TODO: Change type to `System.Net.Mime.ContentType` in the future.
        /// <summary>
        /// Gets the content type.
        /// </summary>
        /// <value>The content type.</value>
        public string ContentType { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="content">The JSON RPC message.</param>
        /// <param name="contentType">The content type.</param>
        public Message(JsonMessage content, string contentType)
        {
            #region Contract
            if (content == null)
                throw new ArgumentNullException(nameof(content));
            // TODO: Verify ContentType.
            #endregion

            this.Content = content;
            this.ContentType = contentType;
        }
        #endregion

        /// <summary>
        /// Reads a message from the specified text reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The message.</returns>
        public static Message Read(TextReader reader)
        {
            #region Contract
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            #endregion

            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes a message to the specified text writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="message">The message.</param>
        public static void Write(TextWriter writer, Message message)
        {
            #region Contract
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            #endregion

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var writer = new StringWriter();
            Write(writer, this);
            return writer.ToString();
        }
    }
}
