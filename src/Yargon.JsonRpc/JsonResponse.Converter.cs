using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Yargon.JsonRpc
{
    partial class JsonResponse
    {
        /// <summary>
        /// Reads a JSON response as either a <see cref="JsonResult"/> or a <see cref="JsonError"/>.
        /// </summary>
        public sealed class Converter : JsonConverter
        {
            /// <inheritdoc />
            public override bool CanWrite => false;

            /// <inheritdoc />
            public override bool CanConvert(Type objectType)
            {
                #region Contract
                Debug.Assert(objectType != null);
                #endregion

                return objectType == typeof(JsonResponse);
            }

            /// <inheritdoc />
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                JsonSerializer serializer)
            {
                #region Contract
                Debug.Assert(reader != null);
                Debug.Assert(objectType != null);
                Debug.Assert(serializer != null);
                #endregion

                // NOTE: This would recursively invoke this converter on the object to be read.
                // To prevent this, we attach a dummy converter to JsonResult and JsonError.
                var jsonObject = JObject.Load(reader);
                if (jsonObject["result"] != null)
                    return serializer.Deserialize<JsonResult>(jsonObject.CreateReader());
                if (jsonObject["error"] != null)
                    return serializer.Deserialize<JsonError>(jsonObject.CreateReader());
                
                throw new JsonSerializationException("Json response has neither 'result' nor 'error' field.");
            }

            /// <inheritdoc />
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                // Writing either a JsonResult or a JsonError can just use the default implementation.
                throw new NotSupportedException();
            }
        }
    }
}
