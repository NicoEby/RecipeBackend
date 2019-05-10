using System;
using ch.thommenmedia.common.Interfaces;
using ch.thommenmedia.common.Model;
using ch.thommenmedia.common.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ch.thommenmedia.common.Extensions
{
    /// <summary>
    ///     converts the input guid elements to a valid guid (null, "" => Guid.Empty)
    /// </summary>
    public class SelectItemConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            if (reader.TokenType == JsonToken.String)
            {
                if (ShortGuid.TryParse((string) reader.Value, out var newGuid))
                {
                    //take only the id
                    return new SimpleSelectItem()
                    {
                        Id = newGuid
                    };
                }
                else if (reader.Value is string str && !string.IsNullOrEmpty(str))
                {
                    //take only the id
                    return new SimpleSelectItem()
                    {
                        Name = str
                    };
                }
                else
                {
                    //guid could not be determined it is set to null
                    return null;
                }
            }
            else
            {
                var jObject = JObject.Load(reader);
                var si = JsonHelper.Deserialize<SimpleSelectItem>(jObject.ToString());
                si.Data = null;
                return si;
            }
            
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SimpleSelectItem) || objectType == typeof(ISelectItem);
        }
    }
}