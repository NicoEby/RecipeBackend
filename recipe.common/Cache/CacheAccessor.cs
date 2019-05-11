using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ch.thommenmedia.common.Extensions;
using Microsoft.Extensions.Caching.Memory;

//using System.Runtime.Caching;

//using MemoryCache = System.Runtime.Caching.MemoryCache;

namespace ch.thommenmedia.common.Cache
{
    /// <summary>
    ///     Caching accessor used to abstract caching groups and technologies.
    /// </summary>
    public static class CacheAccessor
    {
        public static bool CacheEnabled = true;

        private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

       

        /// <summary>
        ///     Attempts to recover an object from the cache. the key must be unique within the class.
        /// </summary>
        /// <typeparam name="T">Type of instance to search</typeparam>
        /// <param name="key">Key</param>
        /// <param name="additional">The additional info</param>
        /// <returns></returns>
        public static T TryGet<T>(string key, string additional = null)
            where T : class
        {
            return (T) TryGet(typeof(T), key, additional);
        }

        /// <summary>
        ///     Attempts to recover an object from the cache. the key must be unique within the class.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="key">Key</param>
        /// <param name="additional">The additional info</param>
        /// <returns></returns>
        public static object TryGet(Type t, string key, string additional = null)
        {
            if (!CacheEnabled)
                return null;

            var obj = _cache.Get(GenerateFullKey(t, key, additional));
            return obj.Copy();
        }


        /// <param name="experation">the experiation of the cach entry</param>
        public static void AddOrUpdate(string key, object instance, string additional = "",
            DateTimeOffset? experation = null)
        {
            if (!CacheEnabled)
                return;

            if (experation == null)
                experation = DateTimeOffset.Now.AddHours(2);

            if (instance != null)
            {
                var copyValue = instance.Copy();
                _cache.Set(GenerateFullKey(instance.GetType(), key, additional), copyValue, experation.Value);
            }
        }


        /// <summary>
        ///     Removes the specified entry in MemCache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="additional">The additional info</param>
        public static void Remove<T>(string key, string additional = null)
            where T : class
        {
            Remove(typeof(T), key, additional);
        }

        /// <summary>
        ///     Removes the specified entry in MemCache
        /// </summary>
        /// <param name="t"></param>
        /// <param name="key">The key.</param>
        /// <param name="additional">The additional info</param>
        public static void Remove(Type t, string key, string additional = null)
        {
            if (!CacheEnabled)
                return;
            _cache.Remove(GenerateFullKey(t, key, additional));
        }

        /// <summary>
        ///     Generates the full key for the cache entry
        /// </summary>
        /// <param name="instanceType">Type of the instance.</param>
        /// <param name="key">The key.</param>
        /// <param name="additional">The additional info</param>
        /// <returns></returns>
        // ReSharper disable once SuggestBaseTypeForParameter
        private static string GenerateFullKey(Type instanceType, string key, string additional = null)
        {
            if (!string.IsNullOrEmpty(additional))
                return string.Format("{0}_{1}_{2}", instanceType.Name, key, additional);

            return string.Format("{0}_{1}", instanceType.Name, key);
        }

        /// <summary>
        /// resets the whole cache
        /// </summary>
        public static void ResetCache()
        {
            _cache.Dispose();
            _cache = new MemoryCache(new MemoryCacheOptions());
        }


        /// <summary>
        ///     Removes the specified entry in MemCache
        /// </summary>
        /// <param name="baseKey">The base key to remove entries</param>
        public static void RemoveAll(string baseKey)
        {
            foreach (var cacheKey in GetCacheKeys().Where(q => q.Contains(CleanKey(baseKey))))
            {
                _cache.Remove(cacheKey);
            }
        }

        private static string CleanKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;

            key = Regex.Replace(key, "{\\d}", "");
            key = Regex.Replace(key, "-$", "");
            key = Regex.Replace(key, "^-", "");
            return key;
        }


        /// <summary>
        /// returns a list of stored cache keys
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCacheKeys()
        {
            var items = new List<string>();
            var field = typeof(MemoryCache).GetProperty("EntriesCollection",
            BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                var collection = field.GetValue(_cache) as ICollection;
                
                if (collection != null)
                {
                    foreach (var item in collection)
                    {
                        var methodInfo = item.GetType().GetProperty("Key");
                        if(methodInfo != null) { 
                            var val = methodInfo.GetValue(item);
                            items.Add(val.ToString());
                        }
                    }
                }
            }
            return items;
        }
    }
}