using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// A JSON response.
    /// </summary>
    public abstract class JsonResponse : JsonMessage
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonResponse"/> class.
        /// </summary>
        /// <param name="id">The message identifier, which is either a string, a number, or <see langword="null"/>.</param>
        /// <param name="jsonrpc">The JSON RPC protocol version; or <see langword="null"/> to use the default.</param>
        [JsonConstructor]
        internal JsonResponse([CanBeNull] object id, [CanBeNull] string jsonrpc)
            : base(id, jsonrpc)
        {
            // Nothing to do.
        }
        #endregion
    }
}
