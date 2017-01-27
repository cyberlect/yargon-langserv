using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// Reads a JSON response as either a <see cref="JsonResult"/> or a <see cref="JsonError"/>.
    /// </summary>
    internal sealed class JsonResponseConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            #region Contract
            Debug.Assert(reader != null);
            Debug.Assert(objectType != null);
            Debug.Assert(serializer != null);
            #endregion
            var jsonObject = JObject.Load(reader);
            if (jsonObject["result"] != null)
                return serializer.Deserialize<JsonResult>(jsonObject.CreateReader());
            else if (jsonObject["error"] != null)
                return serializer.Deserialize<JsonError>(jsonObject.CreateReader());
            else
                throw new InvalidOperationException("Json response has neither 'result' nor 'error' field.");
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}
