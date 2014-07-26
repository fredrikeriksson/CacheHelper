using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CacheHelper.Core;
using StackExchange.Redis;
using Xunit;

namespace CacheHelper.Redis.Tests
{
    public class RedisTests : IDisposable
    {
        private readonly ConnectionMultiplexer _multiplexer;
        private readonly CacheEngine _cacheEngine;

        public RedisTests()
        {
            _multiplexer = ConnectionMultiplexer.Connect("bafver.redis.cache.windows.net,ssl=true,password=een9I4OZKFi1sju6OEm9dJefqokLtLxNuVDc0CDzrY8=,allowAdmin=true");
            _multiplexer.GetServer(_multiplexer.GetEndPoints()[0]).FlushDatabase();
            _cacheEngine = new CacheEngine(new RedisCacheProvider(_multiplexer));
        }

        [Fact]
        public void TestRedis()
        {
            Assert.False(_multiplexer.GetDatabase().KeyExists(_cacheEngine.BuildKey(() => TestMethod())));
            var result = _cacheEngine.Get(() => TestMethod());
            var result2 = _multiplexer.GetDatabase().Get<Morot>(_cacheEngine.BuildKey(() => TestMethod()));

            Assert.Equal(result.Name, result2.Name);
            Assert.Equal(result.Size, result2.Size);
        }

        [Fact]
        public void TestRedisExpires()
        {
            Assert.False(_multiplexer.GetDatabase().KeyExists(_cacheEngine.BuildKey(() => TestMethod())));
            var result = _cacheEngine.Get(() => TestMethod(), TimeSpan.FromSeconds(1));
            Thread.Sleep(2000);
            var result2 = _multiplexer.GetDatabase().Get<Morot>(_cacheEngine.BuildKey(() => TestMethod()));

            Assert.NotNull(result);
            Assert.Null(result2);
        }

        [Fact]
        public void TestRedisKeyNotFoundInt()
        {
            var result = _cacheEngine.Get(() => KeyNotFoundIntMethod(), TimeSpan.FromSeconds(1));
            Assert.NotNull(result);
        }

        public Morot TestMethod()
        {
            return new Morot{Name = "Göran", Size = "Liten"};
        }

        private int KeyNotFoundIntMethod()
        {
            return 10;
        }

        public void Dispose()
        {
            _multiplexer.GetServer(_multiplexer.GetEndPoints()[0]).FlushDatabase();
        }
    }

    [Serializable]
    public class Morot
    {
        public string Name { get; set; }
        public string Size { get; set; }
    }
}
