using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CacheHelper.Core;
using StackExchange.Redis;

namespace CacheHelper.Redis
{
    public class RedisCacheProvider : ICacheProvider
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private IDatabase _database;

        public RedisCacheProvider(ConnectionMultiplexer connectionMultiplexer)
        {
            if (connectionMultiplexer == null)
                throw new ArgumentException("connectionMultiplexer");
            _connectionMultiplexer = connectionMultiplexer;
            _database = _connectionMultiplexer.GetDatabase();
        }

        public T Get<T>(string key, Expression<Func<T>> expression, TimeSpan? expiry = null)
        {
            var cacheItem = _database.Get<T>(key);
            if (EqualityComparer<T>.Default.Equals(cacheItem, default(T)))
                cacheItem = SetAndGet(key, expression, expiry);
            return cacheItem;
        }

        private T SetAndGet<T>(string key, Expression<Func<T>> expression, TimeSpan? expiry = null)
        {
            var func = expression.Compile();
            var result = func();
            if (!ReferenceEquals(result, null))
                _database.Set(key, result, expiry ?? TimeSpan.FromMinutes(5));
            return result;
        }

        public void Bust(string key)
        {
            var database = _connectionMultiplexer.GetDatabase();
            database.KeyDelete(key);
        }
    }
}
