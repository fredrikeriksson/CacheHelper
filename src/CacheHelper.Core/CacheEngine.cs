using System;
using System.Linq;

namespace CacheHelper.Core
{
    public class CacheEngine
    {
        private readonly ICacheProvider _provider;

        public CacheEngine(ICacheProvider provider)
        {
            _provider = provider;
        }

        public T Get<T>(string key, Func<T> func) where T : class
        {
            return _provider.Get(key, func);
        }

        public string BuildKey(string main, params string[] parameters)
        {
            return string.Join("_", new[] {main}.Union(parameters).ToArray());
        }

        public string BuildKey<T>(params string[] parameters)
        {
            var name = typeof (T).FullName;
            return BuildKey(name, parameters);
        }
    }
}