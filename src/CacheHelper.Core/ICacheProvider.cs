using System;
using System.Linq.Expressions;

namespace CacheHelper.Core
{
    public interface ICacheProvider
    {
        T Get<T>(string key, Expression<Func<T>> expression ) where T : class;
        void Bust(string key);
    }
}