using System;
using System.Collections.Generic;
using System.Linq;
using Virtlink.Utilib.Collections;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// The default <see cref="IJsonRpcService"/> implementation.
    /// </summary>
    /// <remarks>
    /// The implementation picks a handler non-deterministically.
    /// </remarks>
    public class DefaultJsonRpcService : JsonRpcServiceBase
    {
        private readonly ExtHashSet<IRequestHandler> handlers = new ExtHashSet<IRequestHandler>();
        /// <summary>
        /// Gets a collection of registered handlers.
        /// </summary>
        /// <value>A collection of registered handlers.</value>
        public IReadOnlyCollection<IRequestHandler> Handlers => this.handlers;

        /// <summary>
        /// Registers a request handler.
        /// </summary>
        /// <param name="handler">The handler to register.</param>
        /// <remarks>
        /// When <paramref name="handler"/> is already registered, it is not registered twice.
        /// </remarks>
        public void Register(IRequestHandler handler)
        {
            #region Contract
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            #endregion

            this.handlers.Add(handler);
        }

        /// <summary>
        /// Unregisters a request handler.
        /// </summary>
        /// <param name="handler">The handler to unregister.</param>
        /// <remarks>
        /// When <paramref name="handler"/> is not registered, nothing happens.
        /// </remarks>
        public void Unregister(IRequestHandler handler)
        {
            #region Contract
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            #endregion

            this.handlers.Remove(handler);
        }

        /// <inheritdoc />
        protected override JsonResponse Handle(JsonRequest request)
        {
            // We go through the handlers in no particular order.
            var handler = this.Handlers.FirstOrDefault(h => h.CanHandle(request.Method));
            if (handler == null)
            {
                // Error!
                // TODO
                throw new NotImplementedException();
            }

            return handler.Handle(request);
        }
    }
}
