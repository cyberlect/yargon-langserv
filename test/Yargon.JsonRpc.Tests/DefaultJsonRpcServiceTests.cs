using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// Tests the <see cref="DefaultJsonRpcService"/> class.
    /// </summary>
    [TestFixture]
    public sealed class DefaultJsonRpcServiceTests : IJsonRpcServiceTests
    {
        /// <inheritdoc />
        public override IJsonRpcService CreateNew()
        {
            return new DefaultJsonRpcService();
        }
    }
}
