using System;
using JetBrains.Annotations;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// An attribute that indicates that a particular method is a JSON RPC method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class JsonRpcMethod : Attribute
    {
        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        /// <value>The name of the method;
        /// or <see langword="null"/> to use the name of the method this attribute was applied to.</value>
        [CanBeNull]
        public string MethodName { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonRpcMethod"/> attribute.
        /// </summary>
        /// <param name="methodName">The name of the method; or <see langword="null"/> to use the default.</param>
        public JsonRpcMethod([CanBeNull] string methodName)
        {
            #region Contract
            if (methodName != null && !JsonRequest.IsValidMethodName(methodName))
                throw new ArgumentException("The method name is not valid.", nameof(methodName));
            #endregion

            this.MethodName = methodName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonRpcMethod"/> attribute.
        /// </summary>
        public JsonRpcMethod()
            : this(null)
        {
            // Nothing to do.
        }
        #endregion

        /// <inheritdoc />
        public override string ToString() => $"[JsonRpcMethod({this.MethodName ?? "null"})]";
    }
}
