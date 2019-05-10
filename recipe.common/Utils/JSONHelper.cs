using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ch.thommenmedia.common.Utils
{
    public class JsonHelper
    {
        /// <summary>
        ///     Serialisiert ein Object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <typeparam name="T"></typeparam>
        /// <remarks></remarks>
        public static string Serialize<T>(T obj)
        {
            if (obj == null)
                return null;
            var serializer = new DataContractJsonSerializer(obj.GetType());
            var ms = new MemoryStream();
            serializer.WriteObject(ms, obj);
            var retVal = Encoding.Default.GetString(ms.ToArray());
            ms.Dispose();
            return retVal;
        }

        /// <summary>
        ///     Macht aus einem JSON String wieder ein Objekt des jeweiligen Typen
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        /// <typeparam name="T"></typeparam>
        /// <remarks></remarks>
        public static T Deserialize<T>(string json)
        {
            var obj = Activator.CreateInstance<T>();
            var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            var serializer = new DataContractJsonSerializer(obj.GetType());
            obj = (T) serializer.ReadObject(ms);
            ms.Close();
            ms.Dispose();
            return obj;
        }
    }
}