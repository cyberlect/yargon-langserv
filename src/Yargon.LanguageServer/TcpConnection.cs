using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Virtlink.Utilib.IO;

namespace Yargon.LanguageServer
{
    /// <summary>
    /// A TCP connection.
    /// </summary>
    public sealed class TcpConnection : IConnection
    {
        /// <summary>
        /// The TCP client.
        /// </summary>
        private readonly TcpClient tcpClient;

        /// <inheritdoc />
        public TextReader Reader { get; }

        /// <inheritdoc />
        public TextWriter Writer { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TcpConnection"/> class.
        /// </summary>
        /// <param name="tcpClient">The TCP client.</param>
        public TcpConnection(TcpClient tcpClient)
        {
            #region Contract
            if (tcpClient == null)
                throw new ArgumentNullException(nameof(tcpClient));
            #endregion

            this.tcpClient = tcpClient;
            var stream = this.tcpClient.GetStream();
            this.Reader = stream.ReadText();
            this.Writer = stream.WriteText();
        }
        #endregion

        /// <inheritdoc />
        void IDisposable.Dispose()
        {
            Close();
        }

        /// <inheritdoc />
        public void Close()
        {
#if NET45
            this.tcpClient.GetStream().Close();
            this.tcpClient.Close();
#else
            this.tcpClient.GetStream().Dispose();
            this.tcpClient.Dispose();
#endif
        }
    }
}
