using System;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Xunit;

namespace CacheHelper.Core.Tests
{
    public class MemoryCacheProviderTests : IDisposable
    {
        [Fact]
        public void AddCacheWithNoParameter()
        {
            var cache = new CacheEngine(new MemoryCacheProvider());
            Assert.Equal("This is a test!", cache.Get(() => NoParameterMethod()));
            Assert.Equal("This is a test!", MemoryCache.Default[cache.BuildKey(() => NoParameterMethod())]);
            Assert.Equal("This is a test!", cache.Get(() => NoParameterMethod()));
        }

        [Fact]
        public void AddCacheWithOneParameter()
        {
            var cache = new CacheEngine(new MemoryCacheProvider());
            Assert.Equal("Banana", cache.Get(() => OneParameterMethod("banana")));
            Assert.Equal("Banana", MemoryCache.Default[cache.BuildKey(() => OneParameterMethod("banana"))]);
            Assert.Equal("Banana", cache.Get(() => OneParameterMethod("banana")));
        }

        [Fact]
        public void AddCacheWithMultipleParameter()
        {
            var cache = new CacheEngine(new MemoryCacheProvider());
            Assert.Equal("CARROT & POTATO", cache.Get(() => MultipleParameterMethod("carrot", "potato")));
            Assert.Equal("CARROT & POTATO", MemoryCache.Default[cache.BuildKey(() => MultipleParameterMethod("carrot", "potato"))]);
            Assert.Equal("CARROT & POTATO", cache.Get(() => MultipleParameterMethod("carrot", "potato")));
        }

        [Fact]
        public void AddCacheWithEnumParameter()
        {
            var cache = new CacheEngine(new MemoryCacheProvider());
            Assert.Equal("apa", cache.Get(() => OneEnumParameterMethod(EnumTest.Apa)));
            Assert.Equal("apa", MemoryCache.Default[cache.BuildKey(() => OneEnumParameterMethod(EnumTest.Apa))]);
            Assert.Equal("apa", cache.Get(() => OneEnumParameterMethod(EnumTest.Apa)));
        }

        private string NoParameterMethod()
        {
            return "This is a test!";
        }

        private string OneParameterMethod(string value)
        {
            if (value == "carrot")
                return "Carrot";
            if (value == "banana")
                return "Banana";
            return null;
        }

        private string MultipleParameterMethod(string value, string value2)
        {
            return string.Format("{0} & {1}", value.ToUpper(), value2.ToUpper());
        }

        private string OneEnumParameterMethod(EnumTest enumTest)
        {
            switch (enumTest)
            {
                case EnumTest.Apa:
                    return "apa";
            }
            return null;
        }

        private enum EnumTest
        {
            Apa = 1
        }

        public void Dispose()
        {
            var allKeys = MemoryCache.Default.Select(o => o.Key);
            Parallel.ForEach(allKeys, key => MemoryCache.Default.Remove(key));
        }
    }
}
