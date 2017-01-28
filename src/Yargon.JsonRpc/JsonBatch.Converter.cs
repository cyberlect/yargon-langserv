using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Yargon.JsonRpc
{
    partial class JsonBatch<T>
    {
        /// <summary>
        /// Reads and writes the JSON representation of a <see cref="JsonBatch{T}"/>.
        /// </summary>
        public sealed class Converter : JsonConverter
        {
            /// <inheritdoc />
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var batch = (JsonBatch<T>) value;

                if (batch.IsSingleton)
                {
                    serializer.Serialize(writer, batch.Messages.Single());
                }
                else
                {
                    serializer.Serialize(writer, batch.Messages);
                }
            }

            /// <inheritdoc />
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                JsonSerializer serializer)
            {
                IReadOnlyList<T> messages;

                bool isSingleton = (reader.TokenType != JsonToken.StartArray);
                if (isSingleton)
                {
                    messages = new[] {serializer.Deserialize<T>(reader)};
                }
                else
                {
                    messages = serializer.Deserialize<List<T>>(reader);
                }

                return new JsonBatch<T>(messages, isSingleton);
            }

            /// <inheritdoc />
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(JsonBatch<T>);
            }
        }
    }
}
