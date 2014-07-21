using System;

namespace CacheHelper.Core
{
    public interface ICacheProvider
    {
        T Get<T>(string key, Func<T> func) where T : class;
        void Evict(string key);
    }
}