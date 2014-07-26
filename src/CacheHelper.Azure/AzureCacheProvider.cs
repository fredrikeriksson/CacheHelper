using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CacheHelper.Core;
using Microsoft.ApplicationServer.Caching;

namespace CacheHelper.Azure
{
    public class AzureCacheProvider : ICacheProvider
    {
        private readonly DataCache _cache;

        public AzureCacheProvider()
        {
            _cache = new DataCache();
        }
        public T Get<T>(string key, Expression<Func<T>> expression, TimeSpan? expiry = null)
        {
            var cacheItem = (T)_cache.Get(key);
            if (EqualityComparer<T>.Default.Equals(cacheItem, default(T)))
                cacheItem = SetAndGet(key, expression, expiry);
            return cacheItem;
        }

        private T SetAndGet<T>(string key, Expression<Func<T>> expression, TimeSpan? expiry = null)
        {

            var func = expression.Compile();
            var result = func();
            if (!ReferenceEquals(result, null))
                _cache.Put(key, result, expiry ?? TimeSpan.FromMinutes(5));
            return result;
        }

        public void Bust(string key)
        {
            var cache = new DataCache();
            cache.Remove(key);
        }
    }
}
