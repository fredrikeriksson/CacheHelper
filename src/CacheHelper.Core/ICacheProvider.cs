using System;
using System.Linq.Expressions;

namespace CacheHelper.Core
{
    public interface ICacheProvider
    {
        T Get<T>(string key, Expression<Func<T>> expression, TimeSpan? expiry = null);
        void Bust(string key);
    }
}