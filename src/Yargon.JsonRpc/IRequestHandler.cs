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

        /// <summary>
        /// Gets whether the handler can handle a request for the specified method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns><see langword="true"/> when this handler can handle the request;
        /// otherwise, <see langword="false"/>.</returns>
        bool CanHandle(string method);
    }
}
