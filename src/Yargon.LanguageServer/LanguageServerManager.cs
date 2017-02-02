using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Virtlink.Utilib.Threading.Tasks;

namespace Yargon.LanguageServer
{
    /// <summary>
    /// Manages language servers and listens for and accepts connections.
    /// </summary>
    public sealed class LanguageServerManager
    {
        private readonly List<LanguageServer> servers = new List<LanguageServer>();
        /// <summary>
        /// Gets the active language servers.
        /// </summary>
        /// <value>A collection of active language servers.</value>
        public IReadOnlyCollection<LanguageServer> Servers => this.servers;

        /// <summary>
        /// Gets whether to continue listening when all language servers have been closed.
        /// </summary>
        /// <value><see langword="true"/> to keep the manager listening after the last language server has been closed;
        /// otherwise, <see langword="false"/>.</value>
        public bool KeepAlive { get; }

        /// <summary>
        /// Gets the end point to listen to.
        /// </summary>
        /// <value>The end point to listen to.</value>
        public IPEndPoint EndPoint { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageServerManager"/> class.
        /// </summary>
        /// <param name="endPoint">The end point to listen to.</param>
        /// <param name="keepAlive">Whether to continue listening when all language servers have been closed.</param>
        public LanguageServerManager(IPEndPoint endPoint, bool keepAlive)
        {
            this.EndPoint = endPoint;
            this.KeepAlive = keepAlive;
        }
        #endregion

        /// <summary>
        /// Starts listening for and accepting connections.
        /// </summary>
        public async Task ListenAsync()
        {
            var cancellation = new CancellationTokenSource();
            var listener = new TcpListener(this.EndPoint);
            listener.Start();

            while (true)
            {
                var client = await Task.Run(listener.AcceptTcpClientAsync, cancellation.Token);
                var server = CreateAndAddLanguageServer(client);
                TaskExt.RunSafe(server.Run, default(CancellationToken));
            }
        }

        /// <summary>
        /// Creates a new language server.
        /// </summary>
        /// <param name="client">The TCP client.</param>
        internal LanguageServer CreateAndAddLanguageServer(TcpClient client)
        {
            #region Contract
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            #endregion

            var server = new LanguageServer(this, new TcpConnection(client));
            this.servers.Add(server);
            return server;
        }

        /// <summary>
        /// Removes a disposed language server.
        /// </summary>
        /// <param name="server">The language server.</param>
        internal void RemoveLanguageServer(LanguageServer server)
        {
            #region Contract
            if (server == null)
                throw new ArgumentNullException(nameof(server));
            #endregion

            this.servers.Remove(server);
        }
    }
}
