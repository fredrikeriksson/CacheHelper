using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CacheHelper.Core.Tests
{
    public class MemoryCacheProviderTests : IDisposable
    {
        [Fact]
        public void AddCache()
        {
            var cache = new CacheEngine(new MemoryCacheProvider());
            Assert.Equal("Morot", cache.Get(() => GetLol("morot")));
            Assert.Equal("Morot", cache.Get(() => GetLol("morot")));
            Assert.Equal("Banan", cache.Get(() => GetLol("banan")));
            Assert.Equal("Banan", cache.Get(() => GetLol("banan")));
            Assert.Equal("Banan", cache.Get(() => GetLol("banan")));
        }

        [Fact]
        public void BuildKey()
        {
            var cache = new CacheEngine(new MemoryCacheProvider());
            Assert.Equal("CacheHelper.Core.Tests.MemoryCacheProviderTests.GetLol_morot", cache.BuildKey(() => GetLol("morot")));
        }

        private string GetLol(string value)
        {
            if (value == "morot")
                return "Morot";
            if (value == "banan")
                return "Banan";
            return null;
        }

        public void Dispose()
        {
            var allKeys = MemoryCache.Default.Select(o => o.Key);
            Parallel.ForEach(allKeys, key => MemoryCache.Default.Remove(key));
        }
    }
}
