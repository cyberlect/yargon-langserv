using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using Virtlink.Utilib.IO;
using Yargon.JsonRpc;

namespace Yargon.LanguageServer
{
    /// <summary>
    /// A language server.
    /// </summary>
    public class LanguageServer : IDisposable
    {
        /// <summary>
        /// The language server manager.
        /// </summary>
        private readonly LanguageServerManager manager;

        /// <summary>
        /// The JSON RPC service.
        /// </summary>
        private readonly IJsonRpcService jsonRpcService;

        /// <summary>
        /// The header reader/writer.
        /// </summary>
        private LSContentHeader.ReaderWriter headerReaderWriter = new LSContentHeader.ReaderWriter();

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>The connection.</value>
        public IConnection Connection { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageServer"/> class.
        /// </summary>
        /// <param name="manager">The language server manager.</param>
        /// <param name="connection">The connection.</param>
        public LanguageServer(LanguageServerManager manager, IConnection connection)
        {
            #region Contract
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            #endregion

            this.manager = manager;
            this.Connection = connection;
            this.jsonRpcService = new DefaultJsonRpcService();
        }
        #endregion

        /// <summary>
        /// Runs the language server asynchronously.
        /// </summary>
        /// <returns>The task.</returns>
        public async Task Run()
        {            
            while (true)
            {
                string request = ReadMessage();
                string response = this.jsonRpcService.Process(request);
                WriteMessage(response);
            }
        }

        /// <summary>
        /// Reads a JSON message with headers from the connection.
        /// </summary>
        /// <returns>The read JSON message.</returns>
        private string ReadMessage()
        {
            var headers = this.headerReaderWriter.Read(this.Connection.Reader);
            if (headers.ContentLength <= 0)
                throw new FormatException("Content-Length not given or invalid.");

            return this.Connection.Reader.ReadString(headers.ContentLength);
        }

        /// <summary>
        /// Writes a JSON message with headers to the connection.
        /// </summary>
        /// <param name="message">The JSON message.</param>
        private void WriteMessage(string message)
        {
            if (String.IsNullOrWhiteSpace(message))
            {
                // No response.
                return;
            }

            var headers = new LSContentHeader();
            headers.ContentLength = message.Length;
            this.headerReaderWriter.Write(this.Connection.Writer, headers);

            this.Connection.Writer.Write(message);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Connection?.Dispose();
            this.manager.RemoveLanguageServer(this);
        }
    }
}
