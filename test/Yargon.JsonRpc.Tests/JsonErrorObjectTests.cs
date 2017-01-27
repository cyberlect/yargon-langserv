using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// Tests the <see cref="JsonErrorObject"/> class.
    /// </summary>
    [TestFixture]
    public sealed class JsonErrorObjectTests
    {
        [Test]
        public void SimpleError_ShouldSerializeThenDeserializeBackToEqualObject()
        {
            // Arrange
            var error = new JsonErrorObject(-32600, "Invalid Request", null);

            // Act
            var json = JsonConvert.SerializeObject(error);
            var result = JsonConvert.DeserializeObject<JsonErrorObject>(json);

            // Assert
            Assert.That(result, Is.EqualTo(error));
        }

        [Test]
        public void SimpleError_ShouldSerializeTwiceToSameJson()
        {
            // Arrange
            var error = new JsonErrorObject(-32600, "Invalid Request", null);

            // Act
            var json1 = JsonConvert.SerializeObject(error);
            var json2 = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<JsonErrorObject>(json1));

            // Assert
            Assert.That(json2, Is.EqualTo(json1));
        }

        [Test]
        public void SimpleError_ShouldDeserializeCorrectly()
        {
            // Arrange
            string json = @"{
                code: -32600,
                message: 'Invalid Request'
            }";

            // Act
            var result = JsonConvert.DeserializeObject<JsonErrorObject>(json);

            // Assert
            var expected = new JsonErrorObject(-32600, "Invalid Request", null);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ErrorWithComplexData_ShouldSerializeThenDeserializeBackToEqualObject()
        {
            // Arrange
            var error = new JsonErrorObject(3, "Execution Error", new JArray
            {
                new JObject
                {
                    { "code", 102 },
                    { "message", "Insufficient gas" },
                },
                new JObject
                {
                    { "code", 103 },
                    { "message", "Gas limit exceeded" },
                },
            });

            // Act
            var json = JsonConvert.SerializeObject(error);
            var result = JsonConvert.DeserializeObject<JsonErrorObject>(json);

            // Assert
            Assert.That(result, Is.EqualTo(error));
        }

        [Test]
        public void ErrorWithComplexData_ShouldSerializeTwiceToSameJson()
        {
            // Arrange
            var error = new JsonErrorObject(3, "Execution Error", new JArray
            {
                new JObject
                {
                    { "code", 102 },
                    { "message", "Insufficient gas" },
                },
                new JObject
                {
                    { "code", 103 },
                    { "message", "Gas limit exceeded" },
                },
            });

            // Act
            var json1 = JsonConvert.SerializeObject(error);
            var json2 = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<JsonErrorObject>(json1));

            // Assert
            Assert.That(json2, Is.EqualTo(json1));
        }

        [Test]
        public void ErrorWithComplexData_ShouldDeserializeCorrectly()
        {
            // Arrange
            string json = @"{
                code: 3,
                message: 'Execution Error',
                data: [{
                    code: 102,
                    message: 'Insufficient gas'
                },
                {
                    code: 103,
                    message: 'Gas limit exceeded'
                }]
            }";

            // Act
            var result = JsonConvert.DeserializeObject<JsonErrorObject>(json);

            // Assert
            var expected = new JsonErrorObject(3, "Execution Error", new JArray
            {
                new JObject
                {
                    { "code", 102 },
                    { "message", "Insufficient gas" },
                },
                new JObject
                {
                    { "code", 103 },
                    { "message", "Gas limit exceeded" },
                },
            });
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
