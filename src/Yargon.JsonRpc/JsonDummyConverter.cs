using System;
using Newtonsoft.Json;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// Dummy converter.
    /// </summary>
    /// <remarks>
    /// Using this converter on <see cref="JsonError"/> and <see cref="JsonResult"/>
    /// prevents the converter of the base class <see cref="JsonResponse"/> from
    /// invoking itself recursively (causing a stack overflow or a cycle detection error).
    /// </remarks>
    internal sealed class JsonDummyConverter : JsonConverter
    {
        /// <inheritdoc />
        public override bool CanWrite => false;

        /// <inheritdoc />
        public override bool CanRead => false;

        /// <inheritdoc />
        public override bool CanConvert(Type objectType) => false;

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}
