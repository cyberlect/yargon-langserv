using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// A JSON RPC service.
    /// </summary>
    public interface IJsonRpcService
    {
        /// <summary>
        /// Handles JSON RPC requests and may return responses.
        /// </summary>
        /// <param name="jsonRequests">The JSON requests to handle.</param>
        /// <returns>The JSON responses; or an empty string when there is no response.</returns>
        string Handle(string jsonRequests);
    }
}
