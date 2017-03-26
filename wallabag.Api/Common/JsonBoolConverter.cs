using Newtonsoft.Json;
using System;

namespace wallabag.Api.Common
{
    class JsonBoolConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(bool);
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.ReadAsInt32();
            if (value != null)
            {
                if (value == 1)
                    return true;
                else
                    return false;
            }

            return false;
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => writer.WriteValue((bool)value ? 1 : 0);
    }
}
