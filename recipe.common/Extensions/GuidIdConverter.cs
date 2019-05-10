using System;
using ch.thommenmedia.common.Utils;
using Newtonsoft.Json;

namespace ch.thommenmedia.common.Extensions
{
    /// <summary>
    ///     converts the input guid elements to a valid guid (null, "" => Guid.Empty)
    /// </summary>
    public class GuidIdConverter : JsonConverter
    {
        public override bool CanWrite => true;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Guid?)
            {
                writer.WriteValue(ShortGuid.Encode((Guid) value));
            }       
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            // if the value is null or empty and  we cann save a null value we will transform it to null
            if (objectType == typeof(Guid?) && (reader.Value == null || string.IsNullOrEmpty(reader.Value.ToString())))
                return null;
            
            if (reader.Value == null || !ShortGuid.TryParse(reader.Value.ToString(), out var result))
                result = Guid.Empty;

            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Guid?) || objectType == typeof(Guid);
        }
    }
}