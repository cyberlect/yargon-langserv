using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;

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
        }
        #endregion

        /// <summary>
        /// Runs the language server asynchronously.
        /// </summary>
        /// <returns>The task.</returns>
        public async Task Run()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Connection?.Dispose();
            this.manager.RemoveLanguageServer(this);
        }
    }
}
