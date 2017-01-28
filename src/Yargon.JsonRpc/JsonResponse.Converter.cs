﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                #region Contract
                Debug.Assert(writer != null);
                Debug.Assert(value != null);
                Debug.Assert(serializer != null);
                #endregion

                if (value is JsonResult)
                    serializer.Serialize(writer, value, typeof(JsonResult));
                if (value is JsonError)
                    serializer.Serialize(writer, value, typeof(JsonError));

                throw new InvalidOperationException("Json response is neither a JsonResult nor a JsonError.");
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

                var jsonObject = JObject.Load(reader);
                if (jsonObject["result"] != null)
                    return serializer.Deserialize<JsonResult>(jsonObject.CreateReader());
                if (jsonObject["error"] != null)
                    return serializer.Deserialize<JsonError>(jsonObject.CreateReader());
                
                throw new InvalidOperationException("Json response has neither 'result' nor 'error' field.");
            }

            /// <inheritdoc />
            public override bool CanConvert(Type objectType)
            {
                #region Contract
                Debug.Assert(objectType != null);
                #endregion

                return objectType == typeof(JsonResponse);
            }
        }
    }
}
