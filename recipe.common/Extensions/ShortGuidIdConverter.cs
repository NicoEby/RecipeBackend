using System;
using ch.thommenmedia.common.Utils;
using Newtonsoft.Json;

namespace ch.thommenmedia.common.Extensions
{
    /// <summary>
    ///     converts the input guid elements to a valid guid (null, "" => Guid.Empty)
    /// </summary>
    public class ShortGuidIdConverter : JsonConverter
    {
        public override bool CanWrite => true;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is ShortGuid guid)
                writer.WriteValue(guid.Value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            if (reader.Value == null || !ShortGuid.TryParse((string) reader.Value, out var result))
                result = Guid.Empty;
            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ShortGuid);
        }
    }
}