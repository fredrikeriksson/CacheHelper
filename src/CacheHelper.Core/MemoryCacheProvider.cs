using System;
using System.Runtime.Caching;

namespace CacheHelper.Core
{
    public class MemoryCacheProvider : ICacheProvider
    {
        public T Get<T>(string key, Func<T> func) where T : class
        {
            if (MemoryCache.Default.Contains(key))
                return MemoryCache.Default.Get(key) as T;
            else
            {
                return SetAndReturn(key, func);
            }
        }

        public void Evict(string key)
        {
            MemoryCache.Default.Remove(key);
        }

        private static T SetAndReturn<T>(string key, Func<T> func) where T : class
        {
            var response = func();
            if (response == null) return null;

            MemoryCache.Default.Add(key, response, CreateCacheItemPolicy());
            return response;
        }

        private static CacheItemPolicy CreateCacheItemPolicy(int seconds = 600)
        {
            return new CacheItemPolicy { SlidingExpiration = TimeSpan.FromSeconds(seconds) };
        }
    }
}