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
    public class JsonResultTests
    {
        [Test]
        public void SimpleResult_ShouldSerializeThenDeserializeBackToEqualResult()
        {
            // Arrange
            var resultMsg = new JsonResult(1, new JValue(7));

            // Act
            var json = JsonConvert.SerializeObject(resultMsg);
            var result = JsonConvert.DeserializeObject<JsonResult>(json);

            // Assert
            Assert.That(result, Is.EqualTo(resultMsg));
        }

        [Test]
        public void SimpleResult_ShouldSerializeTwiceToSameJson()
        {
            // Arrange
            var resultMsg = new JsonResult(1, new JValue(7));

            // Act
            var json1 = JsonConvert.SerializeObject(resultMsg);
            var json2 = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<JsonResult>(json1));

            // Assert
            Assert.That(json2, Is.EqualTo(json1));
        }

        [Test]
        public void SimpleResult_ShouldDeserializeCorrectly()
        {
            // Arrange
            string json = @"{
                jsonrpc: '2.0',
                result: 7,
                id: 1
            }";

            // Act
            var result = JsonConvert.DeserializeObject<JsonResult>(json);

            // Assert
            var expected = new JsonResult(1, new JValue(7));
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ComplexResult_ShouldSerializeThenDeserializeBackToEqualResult()
        {
            // Arrange
            var resultMsg = new JsonResult(1, new JArray { "hello", 5 });

            // Act
            var json = JsonConvert.SerializeObject(resultMsg);
            var result = JsonConvert.DeserializeObject<JsonResult>(json);

            // Assert
            Assert.That(result, Is.EqualTo(resultMsg));
        }

        [Test]
        public void ComplexResult_ShouldSerializeTwiceToSameJson()
        {
            // Arrange
            var resultMsg = new JsonResult(1, new JArray { "hello", 5 });

            // Act
            var json1 = JsonConvert.SerializeObject(resultMsg);
            var json2 = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<JsonResult>(json1));

            // Assert
            Assert.That(json2, Is.EqualTo(json1));
        }

        [Test]
        public void ComplexResult_ShouldDeserializeCorrectly()
        {
            // Arrange
            string json = @"{
                jsonrpc: '2.0',
                result: ['hello', 5],
                id: 1
            }";

            // Act
            var result = JsonConvert.DeserializeObject<JsonResult>(json);

            // Assert
            var expected = new JsonResult(1, new JArray { "hello", 5 });
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
