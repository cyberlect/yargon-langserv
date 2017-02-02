using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Yargon.LanguageServer
{
    /// <summary>
    /// A connection.
    /// </summary>
    public interface IConnection : IDisposable
    {
        /// <summary>
        /// Gets the reader used to read from the connection.
        /// </summary>
        /// <value>The reader.</value>
        TextReader Reader { get; }

        /// <summary>
        /// Gets the writer used to write to the connection.
        /// </summary>
        /// <value>The writer.</value>
        TextWriter Writer { get; }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        void Close();
    }
}
