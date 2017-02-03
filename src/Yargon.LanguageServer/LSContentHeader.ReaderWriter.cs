using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Yargon.LanguageServer
{
    partial class LSContentHeader
    {
        /// <summary>
        /// A conent header reader/writer for the Language Server Protocol.
        /// </summary>
        public new class ReaderWriter : ContentHeader.ReaderWriter
        {
            /// <summary>
            /// Reads the content headers from the specified text reader.
            /// </summary>
            /// <param name="reader">The text reader to read from.</param>
            /// <returns>The content headers.</returns>
            public new LSContentHeader Read(TextReader reader)
            {
                #region Contract
                if (reader == null)
                    throw new ArgumentNullException(nameof(reader));
                #endregion

                return new LSContentHeader(ReadToDictionary(reader));
            }

            /// <inheritdoc />
            protected override string Serialize(string name, object obj)
            {
                #region Contract
                if (name == null)
                    throw new ArgumentNullException(nameof(name));
                #endregion

                switch (name)
                {
                    case "Content-Length":
                        return ((int)obj).ToString(CultureInfo.InvariantCulture);
                    default:
                        return base.Serialize(name, obj);
                }
            }

            /// <inheritdoc />
            protected override object Deserialize(string name, string value)
            {
                #region Contract
                if (name == null)
                    throw new ArgumentNullException(nameof(name));
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                #endregion

                switch (name)
                {
                    case "Content-Length":
                        return Int32.Parse(value, CultureInfo.InvariantCulture);
                    default:
                        return base.Deserialize(name, value);
                }
            }
        }
    }
}
