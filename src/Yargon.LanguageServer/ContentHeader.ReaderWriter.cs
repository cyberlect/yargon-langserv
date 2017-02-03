using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Yargon.LanguageServer
{
    partial class ContentHeader
    {
        /// <summary>
        /// Reads and writes content headers.
        /// </summary>
        public class ReaderWriter
        {
            private int preferredLineLength = 78;
            /// <summary>
            /// Gets or sets the preferred line length.
            /// </summary>
            /// <value>The preferred line length, in characters.</value>
            public int PreferredLineLength
            {
                get { return this.preferredLineLength; }
                set
                {
                    #region Contract
                    if (value < 1 || value >= this.MaximumLineLength)
                        throw new ArgumentOutOfRangeException(nameof(value));
                    #endregion

                    this.preferredLineLength = value;
                }
            }

            private int maximumLineLength = 998;
            /// <summary>
            /// Gets or sets the maximum line length.
            /// </summary>
            /// <value>The maximum line length, in characters.</value>
            public int MaximumLineLength
            {
                get { return this.maximumLineLength; }
                set
                {
                    #region Contract
                    if (value < 1)
                        throw new ArgumentOutOfRangeException(nameof(value));
                    #endregion

                    this.maximumLineLength = value;
                    this.preferredLineLength = Math.Min(this.preferredLineLength, this.maximumLineLength);
                }
            }

            /// <summary>
            /// Reads the content headers from the specified text reader.
            /// </summary>
            /// <param name="reader">The text reader to read from.</param>
            /// <returns>The content headers.</returns>
            public ContentHeader Read(TextReader reader)
            {
                #region Contract
                if (reader == null)
                    throw new ArgumentNullException(nameof(reader));
                #endregion
                
                return new ContentHeader(ReadToDictionary(reader));
            }

            /// <summary>
            /// Reads the content headers from the specified text reader.
            /// </summary>
            /// <param name="reader">The text reader to read from.</param>
            /// <returns>The content headers.</returns>
            protected Dictionary<String, Object> ReadToDictionary(TextReader reader)
            {
                #region Contract
                if (reader == null)
                    throw new ArgumentNullException(nameof(reader));
                #endregion

                var rawHeaders = ReadHeaders(reader);
                var headers = ParseHeaders(rawHeaders);

                return headers;
            }

            /// <summary>
            /// Writes the content headers to the specified text writer.
            /// </summary>
            /// <param name="writer">The text writer to write to.</param>
            /// <param name="headers">The content headers.</param>
            public void Write(TextWriter writer, ContentHeader headers)
            {
                #region Contract
                if (writer == null)
                    throw new ArgumentNullException(nameof(writer));
                if (headers == null)
                    throw new ArgumentNullException(nameof(headers));
                #endregion

                var unparsedHeaders = UnparseHeaders(headers.headers);
                foreach (var header in unparsedHeaders)
                {
                    string headerLine = CombineHeaderLine(header.Key, header.Value);
                    writer.WriteLine(headerLine);
                }
                writer.WriteLine();
            }

            /// <summary>
            /// Serializes the specified object to a string representation.
            /// </summary>
            /// <param name="name">The header name.</param>
            /// <param name="obj">The object to serialize; or <see langword="null"/>.</param>
            /// <returns>The serialized object.</returns>
            protected virtual string Serialize(string name, [CanBeNull] object obj)
            {
                #region Contract
                if (name == null)
                    throw new ArgumentNullException(nameof(name));
                #endregion

                return obj?.ToString() ?? String.Empty;
            }

            /// <summary>
            /// Deserializes the specified string representation to an object.
            /// </summary>
            /// <param name="name">The header name.</param>
            /// <param name="value">The string representation to deserialize.</param>
            /// <returns>The deserialized object; or <see langword="null"/>.</returns>
            [CanBeNull]
            protected virtual object Deserialize(string name, string value)
            {
                #region Contract
                if (name == null)
                    throw new ArgumentNullException(nameof(name));
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                #endregion

                return String.IsNullOrWhiteSpace(value) ? null : value;
            }

            /// <summary>
            /// Reads the headers from the reader.
            /// </summary>
            /// <param name="reader">The reader to read from.</param>
            /// <returns>The dictionary of headers.</returns>
            private IReadOnlyDictionary<String, String> ReadHeaders(TextReader reader)
            {
                #region Contract
                Debug.Assert(reader != null);
                #endregion

                var headers = new Dictionary<String, String>();
                string lastHeaderName = null;
                while (true)
                {
                    // NOTE: Apart from "\r\n" this also accepts "\r" and "\n".
                    // This is not technically correct.
                    string headerLine = reader.ReadLine();
                    if (String.IsNullOrEmpty(headerLine))
                    {
                        // We've read all header lines.
                        break;
                    }
                    if (Char.IsWhiteSpace(headerLine[0]))
                    {
                        // This is the continuation of the last header line.
                        if (lastHeaderName == null)
                            throw new FormatException("Header line starts with whitespace.");
                        headers[lastHeaderName] = headers[lastHeaderName] + headerLine;
                    }
                    else
                    {
                        // This is a new header line.
                        var header = SplitHeaderLine(headerLine);
                        lastHeaderName = header.Item1;
                        headers.Add(header.Item1, header.Item2);
                    }
                }
                return headers;
            }

            /// <summary>
            /// Splits a header line.
            /// </summary>
            /// <param name="headerLine">The header line.</param>
            /// <returns>The header name and value.</returns>
            private Tuple<string, string> SplitHeaderLine(string headerLine)
            {
                #region Contract
                Debug.Assert(headerLine != null);
                #endregion

                int separator = headerLine.IndexOf(':');
                if (separator == -1)
                    throw new FormatException("The header is invalid.");

                // NOTE: This allows whitespace around the header name and header value,
                // but it is trimmed off in the result. So the following are equivalent:
                // 
                //     "Content-Type   : application/json"
                //     "Content-Type:    application/json"
                //     "Content-Type: application/json   "
                //     "Content-Type: application/json"
                //     "Content-Type:application/json"

                string headerName = headerLine.Substring(0, separator).Trim();
                string headerValue = headerLine.Substring(separator + 1).Trim();
                return Tuple.Create(headerName, headerValue);
            }

            /// <summary>
            /// Parses the headers.
            /// </summary>
            /// <param name="headers">The headers.</param>
            /// <returns>The parsed headers.</returns>
            private Dictionary<String, Object> ParseHeaders(IReadOnlyDictionary<String, String> headers)
            {
                #region Contract
                Debug.Assert(headers != null);
                #endregion

                var parsedHeaders = new Dictionary<String, Object>();
                foreach (var header in headers)
                {
                    var headerValue = Deserialize(header.Key, header.Value);
                    parsedHeaders.Add(header.Key, headerValue);
                }
                return parsedHeaders;
            }

            /// <summary>
            /// Unparses the headers.
            /// </summary>
            /// <param name="headers">The headers.</param>
            /// <returns>The unparsed headers.</returns>
            private IReadOnlyDictionary<String, String> UnparseHeaders(IReadOnlyDictionary<String, Object> headers)
            {
                #region Contract
                Debug.Assert(headers != null);
                #endregion

                var unparsedHeaders = new Dictionary<String, String>();
                foreach (var header in headers)
                {
                    var headerValue = Serialize(header.Key, header.Value);
                    unparsedHeaders.Add(header.Key, headerValue);
                }
                return unparsedHeaders;
            }

            /// <summary>
            /// Splits a header line.
            /// </summary>
            /// <param name="headerName">The header name.</param>
            /// <param name="headerValue">The header value.</param>
            /// <returns>The header line.</returns>
            private string CombineHeaderLine(string headerName, string headerValue)
            {
                #region Contract
                Debug.Assert(headerName != null);
                Debug.Assert(headerValue != null);
                #endregion

                var WSP = new[] {' ', '\t'};
                var sb = new StringBuilder();
                sb.Append(headerName).Append(": ");

                int lineStart = sb.Length;
                int offset = 0;
                while (headerValue.Length - offset > this.PreferredLineLength - lineStart)
                {
                    int scanLength = Math.Min(headerValue.Length - offset, this.PreferredLineLength - lineStart);
                    int lineCap = headerValue.LastIndexOfAny(WSP, offset + scanLength, scanLength);
                    if (lineCap == -1)
                    {
                        scanLength = Math.Min(headerValue.Length - offset, this.MaximumLineLength - lineStart);
                        lineCap = headerValue.IndexOfAny(WSP, offset, scanLength);
                    }
                    if (lineCap == -1)
                        throw new FormatException("Line too long.");

                    sb.Append(headerValue, offset, lineCap - offset);
                    sb.Append("\r\n");
                    lineStart = 1;
                    offset = lineCap;
                }
                sb.Append(headerValue, offset, headerValue.Length - offset);

                return sb.ToString();
            }
        }
    }
}
