using NUnit.Framework;
using Newtonsoft.Json;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// Tests the <see cref="JsonBatch{T}"/> class.
    /// </summary>
    [TestFixture]
    public sealed class JsonBatchTests
    {
        [Test]
        public void SingleRequest_ShouldSerializeThenDeserializeBackToEqualBatch()
        {
            // Arrange
            var batch = new JsonBatch<JsonRequest>(new[] { new JsonRequest(null, "foobar") });

            // Act
            var json = JsonConvert.SerializeObject(batch, new JsonBatch<JsonRequest>.Converter());
            var result = JsonConvert.DeserializeObject<JsonBatch<JsonRequest>>(json, new JsonBatch<JsonRequest>.Converter());

            // Assert
            Assert.That(result, Is.EqualTo(batch));
        }

        [Test]
        public void SingleRequest_ShouldSerializeTwiceToSameJson()
        {
            // Arrange
            var batch = new JsonBatch<JsonRequest>(new[] { new JsonRequest(null, "foobar") });

            // Act
            var json1 = JsonConvert.SerializeObject(batch, new JsonBatch<JsonRequest>.Converter());
            var json2 = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<JsonBatch<JsonRequest>>(json1, new JsonBatch<JsonRequest>.Converter()), new JsonBatch<JsonRequest>.Converter());

            // Assert
            Assert.That(json2, Is.EqualTo(json1));
        }

        [Test]
        public void SingleRequest_ShouldDeserializeCorrectly()
        {
            // Arrange
            string json = @"{
                'jsonrpc': '2.0',
                'method': 'foobar'
            }";

            // Act
            var result = JsonConvert.DeserializeObject<JsonBatch<JsonRequest>>(json, new JsonBatch<JsonRequest>.Converter());

            // Assert
            var expected = new JsonBatch<JsonRequest>(new[] { new JsonRequest(null, "foobar") } );
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void MultipleRequests_ShouldSerializeThenDeserializeBackToEqualBatch()
        {
            // Arrange
            var batch = new JsonBatch<JsonRequest>(new[] { new JsonRequest(null, "foo"), new JsonRequest(null, "bar") });

            // Act
            var json = JsonConvert.SerializeObject(batch, new JsonBatch<JsonRequest>.Converter());
            var result = JsonConvert.DeserializeObject<JsonBatch<JsonRequest>>(json, new JsonBatch<JsonRequest>.Converter());

            // Assert
            Assert.That(result, Is.EqualTo(batch));
        }

        [Test]
        public void MultipleRequests_ShouldSerializeTwiceToSameJson()
        {
            // Arrange
            var batch = new JsonBatch<JsonRequest>(new[] { new JsonRequest(null, "foo"), new JsonRequest(null, "bar") });

            // Act
            var json1 = JsonConvert.SerializeObject(batch, new JsonBatch<JsonRequest>.Converter());
            var json2 = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<JsonBatch<JsonRequest>>(json1, new JsonBatch<JsonRequest>.Converter()), new JsonBatch<JsonRequest>.Converter());

            // Assert
            Assert.That(json2, Is.EqualTo(json1));
        }

        [Test]
        public void MultipleRequests_ShouldDeserializeCorrectly()
        {
            // Arrange
            string json = @"[{
                'jsonrpc': '2.0',
                'method': 'foo'
            },{
                'jsonrpc': '2.0',
                'method': 'bar'
            }]";

            // Act
            var result = JsonConvert.DeserializeObject<JsonBatch<JsonRequest>>(json, new JsonBatch<JsonRequest>.Converter());

            // Assert
            var expected = new JsonBatch<JsonRequest>(new[] { new JsonRequest(null, "foo"), new JsonRequest(null, "bar") });
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
