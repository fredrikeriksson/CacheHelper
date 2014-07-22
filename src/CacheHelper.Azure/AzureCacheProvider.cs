using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CacheHelper.Core;
using Microsoft.ApplicationServer.Caching;

namespace CacheHelper.Azure
{
    public class AzureCacheProvider : ICacheProvider
    {
        public T Get<T>(string key, Expression<Func<T>> expression) where T : class
        {
            var cache = new DataCache();
            var cacheItem = cache.Get(key) as T;
            if (cacheItem == null)
            {
                var func = expression.Compile();
                cacheItem = func();
                if (cacheItem != null)
                    cache.Put(key, cacheItem, TimeSpan.FromSeconds(600));
            }
            return cacheItem;
        }

        public void Bust(string key)
        {
            var cache = new DataCache();
            cache.Remove(key);
        }
    }
}
