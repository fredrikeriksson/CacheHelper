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
            Assert.Equal("CacheHelper.Core.Tests.CacheEngineTests.OneParameterMethod_System.String:carrot", cache.BuildKey(() => OneParameterMethod("carrot")));
        }

        [Fact]
        public void BuildKeyWithOneEnumParameter()
        {
            var cache = new CacheEngine(new MemoryCacheProvider());
            Assert.Equal("CacheHelper.Core.Tests.CacheEngineTests.OneEnumParameterMethod_CacheHelper.Core.Tests.CacheEngineTests+EnumTest:Apa", cache.BuildKey(() => OneEnumParameterMethod(EnumTest.Apa)));
        }

        [Fact]
        public void BuildKeyWithMultipleParameter()
        {
            var cache = new CacheEngine(new MemoryCacheProvider());
            Assert.Equal("CacheHelper.Core.Tests.CacheEngineTests.MultipleParameterMethod_System.String:test1_System.Int32:1_System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]:2", cache.BuildKey(() => MultipleParameterMethod("test1", 1, 2)));
        }

        [Fact]
        public void BuildKeyWithClassParameter()
        {
            var cache = new CacheEngine(new MemoryCacheProvider());
            Assert.Equal("CacheHelper.Core.Tests.CacheEngineTests.ClassParameterMethod_System.Int32_1_System.String_Fredrik", cache.BuildKey(() => ClassParameterMethod(new Test { Id = 1, Name = "Fredrik" })));
        }

        [Fact]
        public void BuildKeyWithTwoClassParameter()
        {
            var cache = new CacheEngine(new MemoryCacheProvider());
            Assert.Equal("CacheHelper.Core.Tests.CacheEngineTests.TwoClassParameterMethod_System.Int32_1_System.String_Fredrik1_System.Int32_2_System.String_Fredrik2", cache.BuildKey(() => TwoClassParameterMethod(new Test { Id = 1, Name = "Fredrik1" }, new Test { Id = 2, Name = "Fredrik2" })));
        }

        [Fact]
        public void BuildKeyWithTwoParameter()
        {
            var cache = new CacheEngine(new MemoryCacheProvider());
            Assert.Equal("CacheHelper.Core.Tests.CacheEngineTests.TwoParameterMethod_System.Int32_1_System.String_Fredrik1_CacheHelper.Core.Tests.CacheEngineTests+EnumTest:Apa", cache.BuildKey(() => TwoParameterMethod(new Test { Id = 1, Name = "Fredrik1" }, EnumTest.Apa)));
        }

        [Fact]
        public void BuildKeyWithReferenceToNonMethod()
        {
            var cache = new CacheEngine(new MemoryCacheProvider());
            Assert.Throws<ExpressionBodyNotSupported>(() => cache.BuildKey(() => "asd"));
        }

        [Fact]
        public void BuildKeyForMethodsWithOverride()
        {
            var cache = new CacheEngine(new MemoryCacheProvider());
            var f = cache.BuildKey(() => OvverrideTest1(1));
            var s = cache.BuildKey(() => OvverrideTest1("1"));
            Assert.NotEqual(f, s);
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

        private string MultipleParameterMethod(string value, int value2, int? value3)
        {
            return string.Format("{0} & {1} & {2}", value.ToUpper(), value2, value3.GetValueOrDefault());
        }

        private string ClassParameterMethod(Test test)
        {
            return string.Format("{0} & {1}", test.Id, test.Name);
        }

        private string TwoClassParameterMethod(Test test, Test test2)
        {
            return string.Format("{0} & {1} || {2} & {3}", test.Id, test.Name, test2.Id, test2.Name);
        }

        private string TwoParameterMethod(Test test, EnumTest test2)
        {
            return string.Format("{0} & {1} || {2}", test.Id, test.Name, test2);
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

        private string OvverrideTest1(string test)
        {
            return "F";
        }

        private string OvverrideTest1(int test)
        {
            return "S";
        }

        private enum EnumTest
        {
            Apa = 1
        }

        private class Test
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}