using System;
using Newtonsoft.Json;

namespace Vinyl.Utils
{
    public class BoxedTypeJsonConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return TypeConverterInstance.Current.ConvertTo(objectType, reader.Value);
        }

        public override bool CanConvert(Type objectType)
        {
            return TypeConverterInstance.Current.CanConvert(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(TypeConverterInstance.Current.ConvertFrom(value.GetType(), value));
        }
    }
}