using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ch.thommenmedia.common.Extensions;
using ch.thommenmedia.common.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ch.thommenmedia.common.Helper
{
    public static class JsonHelper
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="converters"></param>
        /// <param name="ignoreErrors"></param>
        /// <param name="maxDepth">if 0 ignored; otherwise loops are ignored and maxdepth is set to the number provieded</param>
        /// <returns></returns>
        public static JsonSerializerSettings GetSettings(List<JsonConverter> converters = null, bool ignoreErrors = false, int maxDepth = 0 )
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.Converters.Add(new GuidIdConverter());
            serializerSettings.Converters.Add(new ShortGuidIdConverter());
            serializerSettings.Converters.Add(new SelectItemInterfaceConverter());
            if (converters != null)
            {
                foreach (var jsonConverter in converters)
                {
                    serializerSettings.Converters.Add(jsonConverter);
                }
            }   
            serializerSettings.NullValueHandling = NullValueHandling.Ignore;
            // en general we ignore serialization errors
            serializerSettings.Error = delegate(object sender, ErrorEventArgs args)
            {
                args.ErrorContext.Handled = ignoreErrors;
            };
            if (maxDepth > 0)
            {
                serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                serializerSettings.MaxDepth = maxDepth;
            }
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return serializerSettings;
        }

        public static TOutputObject Deserialize<TOutputObject>(string input, List<JsonConverter> converters = null, bool ignoreErrors = false)
            where TOutputObject: class
        {
            if (string.IsNullOrEmpty(input)) return (TOutputObject)null;
            if (input == "null") input = "{}";
            return JsonConvert.DeserializeObject<TOutputObject>(input, GetSettings(converters, ignoreErrors));
        }

        public static Object Deserialize(Type type, string input, List<JsonConverter> converters = null, bool ignoreErrors = false)
        {
            if (string.IsNullOrEmpty(input)) return null;
            if (input == "null") input = "{}";
            if (type == typeof(ISelectItem) && !input.Contains("{"))
                input = "{name:'" + input + "'}";
            if (type.ImplementsInterface(typeof(IEnumerable)) && !input.Contains("["))
                input = "[" + input + "]";
            return JsonConvert.DeserializeObject(input, type, GetSettings(converters, ignoreErrors));
        }

        public static string Serialize(object ob, List<JsonConverter> converters = null, bool ignoreErrors = false, int maxDepth = 0)
        {
            return JsonConvert.SerializeObject(ob,GetSettings(null, ignoreErrors, maxDepth));
        }

        public static Dictionary<string, object> DumpObject(object input)
        {
            return input?.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(q =>
                    // do not dump the context
                        !q.PropertyType.ImplementsInterface(typeof(IDbContext))
                        // do not log plain entity types
                        && !q.PropertyType.ImplementsInterface(typeof(IEntityBase))
                ).ToDictionary(propertyInfo => propertyInfo.Name, propertyInfo => propertyInfo.GetValue(input));
        }
    }
}
