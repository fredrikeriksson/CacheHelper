using System;
using System.Linq.Expressions;
using CacheHelper.Core;
using StackExchange.Redis;

namespace CacheHelper.Redis
{
    public class RedisCacheProvider : ICacheProvider
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public RedisCacheProvider(ConnectionMultiplexer connectionMultiplexer)
        {
            if(_connectionMultiplexer == null)
                throw new ArgumentException("connectionMultiplexer");
            _connectionMultiplexer = connectionMultiplexer;
        }

        public T Get<T>(string key, Expression<Func<T>> expression) where T : class
        {
            var database = _connectionMultiplexer.GetDatabase();
            if (database.KeyExists(key))
            {
                return database.Get<T>(key);
            }

            var func = expression.Compile();
            var result = func();
            if (result != null)
                database.Set(key, result);
            return result;
        }

        public void Bust(string key)
        {
            var database = _connectionMultiplexer.GetDatabase();
            database.KeyDelete(key);
        }
    }
}
