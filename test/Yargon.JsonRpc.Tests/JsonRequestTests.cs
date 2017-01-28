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
    public class JsonRequestTests
    {
        [Test]
        public void SimpleRequest_ShouldSerializeThenDeserializeBackToEqualRequest()
        {
            // Arrange
            var request = new JsonRequest("1", "foobar");

            // Act
            var json = JsonConvert.SerializeObject(request);
            var result = JsonConvert.DeserializeObject<JsonRequest>(json);

            // Assert
            Assert.That(result, Is.EqualTo(request));
        }

        [Test]
        public void SimpleRequest_ShouldSerializeTwiceToSameJson()
        {
            // Arrange
            var request = new JsonRequest("1", "foobar");

            // Act
            var json1 = JsonConvert.SerializeObject(request);
            var json2 = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<JsonRequest>(json1));

            // Assert
            Assert.That(json2, Is.EqualTo(json1));
        }

        [Test]
        public void SimpleRequest_ShouldDeserializeCorrectly()
        {
            // Arrange
            string json = @"{
                jsonrpc: '2.0',
                method: 'foobar',
                id: 1
            }";

            // Act
            var result = JsonConvert.DeserializeObject<JsonRequest>(json);

            // Assert
            var expected = new JsonRequest("1", "foobar");
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void RequestWithParamList_ShouldSerializeThenDeserializeBackToEqualRequest()
        {
            // Arrange
            var request = new JsonRequest(null, "update", new JArray { 1, 2, 3, 4, 5 });

            // Act
            var json = JsonConvert.SerializeObject(request);
            var result = JsonConvert.DeserializeObject<JsonRequest>(json);

            // Assert
            Assert.That(result, Is.EqualTo(request));
        }

        [Test]
        public void RequestWithParamList_ShouldSerializeTwiceToSameJson()
        {
            // Arrange
            var request = new JsonRequest(null, "update", new JArray { 1, 2, 3, 4, 5 });

            // Act
            var json1 = JsonConvert.SerializeObject(request);
            var json2 = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<JsonRequest>(json1));

            // Assert
            Assert.That(json2, Is.EqualTo(json1));
        }

        [Test]
        public void RequestWithParamList_ShouldDeserializeCorrectly()
        {
            // Arrange
            string json = @"{
                jsonrpc: '2.0',
                method: 'update',
                params: [1,2,3,4,5]
            }";

            // Act
            var result = JsonConvert.DeserializeObject<JsonRequest>(json);

            // Assert
            var expected = new JsonRequest(null, "update", new JArray { 1, 2, 3, 4, 5 });
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void RequestWithComplexParams_ShouldSerializeThenDeserializeBackToEqualRequest()
        {
            // Arrange
            var request = new JsonRequest("3", "subtract", new JObject
            {
                { "subtrahend", 23 },
                {  "minuend", 42 }
            });

            // Act
            var json = JsonConvert.SerializeObject(request);
            var result = JsonConvert.DeserializeObject<JsonRequest>(json);

            // Assert
            Assert.That(result, Is.EqualTo(request));
        }

        [Test]
        public void RequestWithComplexParams_ShouldSerializeTwiceToSameJson()
        {
            // Arrange
            var request = new JsonRequest("3", "subtract", new JObject
            {
                { "subtrahend", 23 },
                { "minuend", 42 }
            });

            // Act
            var json1 = JsonConvert.SerializeObject(request);
            var json2 = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<JsonRequest>(json1));

            // Assert
            Assert.That(json2, Is.EqualTo(json1));
        }

        [Test]
        public void RequestWithComplexParams_ShouldDeserializeCorrectly()
        {
            // Arrange
            string json = @"{
                jsonrpc: '2.0',
                method: 'subtract',
                params: {
                    'subtrahend': 23,
                    'minuend': 42
                },
                'id': 3
            }";

            // Act
            var result = JsonConvert.DeserializeObject<JsonRequest>(json);

            // Assert
            var expected = new JsonRequest("3", "subtract", new JObject
            {
                { "subtrahend", 23 },
                {  "minuend", 42 }
            });
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
