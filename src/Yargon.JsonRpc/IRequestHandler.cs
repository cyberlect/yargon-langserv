using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// A JSON RPC request handler.
    /// </summary>
    public interface IRequestHandler
    {
        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request to handle.</param>
        /// <returns>The response.</returns>
        JsonResponse Handle(JsonRequest request);
    }
}
