using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Yargon.JsonRpc
{
    [TestFixture]
    public class JsonResponseTests
    {
        [Test]
        public void ErrorMessage_ShouldSerializeThenDeserializeBackToEqualErrorMessage()
        {
            // Arrange
            var error = new JsonError(null, new JsonErrorObject(-32600, "Invalid Request"));

            // Act
            var json = JsonConvert.SerializeObject(error);
            var result = JsonConvert.DeserializeObject<JsonError>(json, new JsonResponseConverter());

            // Assert
            Assert.That(result, Is.EqualTo(error));
        }

        [Test]
        public void ErrorMessage_ShouldSerializeTwiceToSameJson()
        {
            // Arrange
            var error = new JsonError(null, new JsonErrorObject(-32600, "Invalid Request"));

            // Act
            var json1 = JsonConvert.SerializeObject(error);
            var json2 = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<JsonError>(json1, new JsonResponseConverter()));

            // Assert
            Assert.That(json2, Is.EqualTo(json1));
        }

        [Test]
        public void ErrorMessage_ShouldDeserializeToAJsonError()
        {
            // Arrange
            string json = @"{
                jsonrpc: '2.0',
                error: {
                    code: -32600,
                    message: 'Invalid Request'
                },
                id: null
            }";

            // Act
            var result = JsonConvert.DeserializeObject<JsonError>(json, new JsonResponseConverter());

            // Assert
            var expected = new JsonError(null, new JsonErrorObject(-32600, "Invalid Request"));
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ResultMessage_ShouldSerializeThenDeserializeBackToEqualResult()
        {
            // Arrange
            var resultMsg = new JsonResult("1", new JValue(7));

            // Act
            var json = JsonConvert.SerializeObject(resultMsg);
            var result = JsonConvert.DeserializeObject<JsonResult>(json, new JsonResponseConverter());

            // Assert
            Assert.That(result, Is.EqualTo(resultMsg));
        }

        [Test]
        public void ResultMessage_ShouldSerializeTwiceToSameJson()
        {
            // Arrange
            var resultMsg = new JsonResult("1", new JValue(7));

            // Act
            var json1 = JsonConvert.SerializeObject(resultMsg);
            var json2 = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<JsonResult>(json1, new JsonResponseConverter()));

            // Assert
            Assert.That(json2, Is.EqualTo(json1));
        }

        [Test]
        public void ResultMessage_ShouldDeserializeCorrectly()
        {
            // Arrange
            string json = @"{
                jsonrpc: '2.0',
                result: 7,
                id: 1
            }";

            // Act
            var result = JsonConvert.DeserializeObject<JsonResult>(json, new JsonResponseConverter());

            // Assert
            var expected = new JsonResult("1", new JValue(7));
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
