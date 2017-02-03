using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yargon.LanguageServer
{
    /// <summary>
    /// A Language Server Protocol message content header.
    /// </summary>
    public partial class LSContentHeader : ContentHeader
    {
        /// <summary>
        /// Gets the content length.
        /// </summary>
        /// <value>The content length.</value>
        public int ContentLength
        {
            get { return Get<int>("Content-Length"); }
            set
            {
                #region Contract
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));
                #endregion

                Set("Content-Length", value);
            }
        }

        /// <summary>
        /// Gets the content type.
        /// </summary>
        /// <value>The content type.</value>
        public string ContentType
        {
            get { return Get<string>("Content-Type") ?? "application/vscode-jsonrpc; charset=utf8"; }
            set
            {
                #region Contract
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                #endregion

                Set("Content-Type", value);
            }
        }

        /// <summary>
        /// Gets the reader/writer.
        /// </summary>
        /// <value>The reader/writer.</value>
        protected override ContentHeader.ReaderWriter ReaderWriterInstance { get; } = new ReaderWriter();

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LSContentHeader"/> class.
        /// </summary>
        public LSContentHeader()
            : base(new Dictionary<String, Object>())
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LSContentHeader"/> class.
        /// </summary>
        /// <param name="headers">The headers.</param>
        public LSContentHeader(Dictionary<String, Object> headers)
            : base(headers)
        {
            // Nothing to do.
        }
        #endregion
    }
}
