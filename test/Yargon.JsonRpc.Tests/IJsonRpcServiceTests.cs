using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// Tests the <see cref="IJsonRpcService"/> interface.
    /// </summary>
    [TestFixture]
    public abstract class IJsonRpcServiceTests
    {
        /// <summary>
        /// Creates a new instance of the subject-under-test.
        /// </summary>
        /// <returns>The created instance.</returns>
        public abstract IJsonRpcService CreateNew();

        [Test]
        public void GivenAnEmptyBatch_ReturnsInvalidRequestError()
        {
            // Arrange
            var json = "[]";
            var rpcService = CreateNew();

            // Act
            string output = rpcService.Process(json);

            // Assert
            var result = JsonConvert.DeserializeObject<JsonResponse>(output);
            var expected = new JsonError(null, new JsonErrorObject(-32600, "Invalid request"));
            Assert.That(result, Is.EqualTo(expected).Using<JsonResponse>(new JsonResponseComparer()));
        }

        [Test]
        public void GivenABatchWithAnInvalidRequest_ReturnsABatchWithAnInvalidRequestError()
        {
            // Arrange
            var json = "[1]";
            var rpcService = CreateNew();

            // Act
            string output = rpcService.Process(json);

            // Assert
            var result = JsonConvert.DeserializeObject<List<JsonResponse>>(output);
            var expected = new List<JsonResponse>
            {
                new JsonError(null, new JsonErrorObject(-32600, "Invalid request"))
            };
            Assert.That(result, Is.EqualTo(expected).Using<JsonResponse>(new JsonResponseComparer()));
        }

        [Test]
        public void GivenABatchOfInvalidRequests_ReturnsAnInvalidRequestErrorForEachRequest()
        {
            // Arrange
            var json = "[1, 2, 3]";
            var rpcService = CreateNew();

            // Act
            string output = rpcService.Process(json);

            // Assert
            var result = JsonConvert.DeserializeObject<List<JsonResponse>>(output);
            var expected = new List<JsonResponse>
            {
                new JsonError(null, new JsonErrorObject(-32600, "Invalid request")),
                new JsonError(null, new JsonErrorObject(-32600, "Invalid request")),
                new JsonError(null, new JsonErrorObject(-32600, "Invalid request"))
            };
            Assert.That(result, Is.EqualTo(expected).Using<JsonResponse>(new JsonResponseComparer()));
        }

        [Test]
        public void GivenAnInvalidRequest_ReturnsAnInvalidRequestError()
        {
            // Arrange
            var json = "1";
            var rpcService = CreateNew();

            // Act
            string output = rpcService.Process(json);

            // Assert
            var result = JsonConvert.DeserializeObject<JsonResponse>(output);
            var expected = new JsonError(null, new JsonErrorObject(-32600, "Invalid request"));
            Assert.That(result, Is.EqualTo(expected).Using<JsonResponse>(new JsonResponseComparer()));
        }

        [Test]
        public void GivenInvalidJson_ReturnsAParseError()
        {
            // Arrange
            var json = "{jsonrpc: '2.0', method: 'foobar, 'params': 'bar', 'baz]";
            var rpcService = CreateNew();

            // Act
            string output = rpcService.Process(json);

            // Assert
            var result = JsonConvert.DeserializeObject<JsonResponse>(output);
            var expected = new JsonError(null, new JsonErrorObject(-32700, "Parse error"));
            Assert.That(result, Is.EqualTo(expected).Using<JsonResponse>(new JsonResponseComparer()));
        }

        [Test]
        public void GivenInvalidBatchJson_ReturnsAParseError()
        {
            // Arrange
            var json = @"[
                { 'jsonrpc': '2.0', 'method': 'sum', 'params': [1,2,4], 'id': '1'},
                {'jsonrpc': '2.0', 'method'
            ]";
            var rpcService = CreateNew();

            // Act
            string output = rpcService.Process(json);

            // Assert
            var result = JsonConvert.DeserializeObject<JsonResponse>(output);
            var expected = new JsonError(null, new JsonErrorObject(-32700, "Parse error"));
            Assert.That(result, Is.EqualTo(expected).Using<JsonResponse>(new JsonResponseComparer()));
        }
    }
}
