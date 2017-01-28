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
        /// <param name="id">The message identifier string; or <see langword="null"/>.</param>
        /// <param name="jsonrpc">The JSON RPC protocol version string.</param>
        [JsonConstructor]
        internal JsonResponse([CanBeNull] string id, string jsonrpc)
            : base(id, jsonrpc)
        {
            // Nothing to do.
        }
        #endregion
    }
}
