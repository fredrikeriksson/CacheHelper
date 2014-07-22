using CacheHelper.Core.Exceptions;
using Xunit;

namespace CacheHelper.Core.Tests
{
    public class CacheEngineTests
    {
        [Fact]
        public void BuildRegularKey()
        {
            var cache = new CacheEngine(new MemoryCacheProvider());
            Assert.Equal("test", cache.BuildKey("test"));
        }
        [Fact]
        public void BuildRegularKeyWithParameters()
        {
            var cache = new CacheEngine(new MemoryCacheProvider());
            Assert.Equal("test_carrot_potato", cache.BuildKey("test", "carrot", "potato"));
        }

        [Fact]
        public void BuildKeyWithNoParameter()
        {
            var cache = new CacheEngine(new MemoryCacheProvider());
            Assert.Equal("CacheHelper.Core.Tests.CacheEngineTests.NoParameterMethod", cache.BuildKey(() => NoParameterMethod()));
        }

        [Fact]
        public void BuildKeyWithOneParameter()
        {
            var cache = new CacheEngine(new MemoryCacheProvider());
            Assert.Equal("CacheHelper.Core.Tests.CacheEngineTests.OneParameterMethod_carrot", cache.BuildKey(() => OneParameterMethod("carrot")));
        }

        [Fact]
        public void BuildKeyWithOneEnumParameter()
        {
            var cache = new CacheEngine(new MemoryCacheProvider());
            Assert.Equal("CacheHelper.Core.Tests.CacheEngineTests.OneEnumParameterMethod_Apa", cache.BuildKey(() => OneEnumParameterMethod(EnumTest.Apa)));
        }

        [Fact]
        public void BuildKeyWithMultipleParameter()
        {
            var cache = new CacheEngine(new MemoryCacheProvider());
            Assert.Equal("CacheHelper.Core.Tests.CacheEngineTests.MultipleParameterMethod_test1_test2", cache.BuildKey(() => MultipleParameterMethod("test1", "test2")));
        }

        [Fact]
        public void BuildKeyWithReferenceToNonMethod()
        {
            var cache = new CacheEngine(new MemoryCacheProvider());
            Assert.Throws<ExpressionBodyNotSupported>(() => cache.BuildKey(() => "asd"));
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
    }
}