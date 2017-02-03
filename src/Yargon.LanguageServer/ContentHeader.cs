using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace Yargon.LanguageServer
{
    /// <summary>
    /// A content header.
    /// </summary>
    public partial class ContentHeader
    {
        /// <summary>
        /// The dictionary with headers.
        /// </summary>
        private readonly Dictionary<String, Object> headers;

        /// <summary>
        /// Gets the header names.
        /// </summary>
        /// <value>The header names.</value>
        public IEnumerable<String> Names => this.headers.Keys;

        /// <summary>
        /// Gets the reader/writer.
        /// </summary>
        /// <value>The reader/writer.</value>
        protected virtual ReaderWriter ReaderWriterInstance { get; } = new ReaderWriter();

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentHeader"/> class.
        /// </summary>
        public ContentHeader()
            : this(new Dictionary<String, Object>())
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentHeader"/> class.
        /// </summary>
        /// <param name="headers">The headers.</param>
        public ContentHeader(Dictionary<String, Object> headers)
        {
            #region Contract
            if (headers == null)
                throw new ArgumentNullException(nameof(headers));
            #endregion

            this.headers = headers;
        }
        #endregion

        /// <summary>
        /// Gets a header object with the specified header name and type.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="name">The header name.</param>
        /// <returns>The header object;
        /// or the default of <typeparamref name="T"/> if not found or not convertible.</returns>
        [CanBeNull]
        public T Get<T>(string name)
        {
            #region Contract
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            #endregion

            T value;
            if (!TryGetValue(name, out value))
                return default(T);
            return value;
        }

        /// <summary>
        /// Sets a header object for the specified header name and type.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header object.</param>
        public void Set<T>(string name, [CanBeNull] T value)
        {
            #region Contract
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            #endregion

            this.headers[name] = value;
        }

        /// <summary>
        /// Attempts to get a header object with the specified type and header name.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header object.</param>
        /// <returns><see langword="true"/> when the header was present;
        /// otherwise, <see langword="false"/>.</returns>
        public bool TryGetValue<T>(string name, [CanBeNull] out T value)
        {
            #region Contract
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            #endregion

            object obj;
            if (this.headers.TryGetValue(name, out obj) && (obj == null || obj is T))
            {
                value = (T)obj;
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        /// <summary>
        /// Determines whether there is a header with the specified name.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <returns><see langword="true"/> when a value is present;
        /// otherwise, <see langword="false"/>.</returns>
        public bool ContainsKey(string name)
        {
            #region Contract
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            #endregion

            return this.headers.ContainsKey(name);
        }

        /// <summary>
        /// Returns the headers as a dictionary.
        /// </summary>
        /// <returns>A dictionary with the headers.</returns>
        public IReadOnlyDictionary<String, Object> ToDictionary()
        {
            return this.headers.ToDictionary(p => p.Key, p => p.Value);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            using (var writer = new StringWriter())
            {
                this.ReaderWriterInstance.Write(writer, this);
                return writer.ToString();
            }
        }
    }
}
