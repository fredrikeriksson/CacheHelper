using System;
using System.Linq;
using System.Linq.Expressions;

namespace CacheHelper.Core
{
    public class CacheEngine
    {
        private readonly ICacheProvider _provider;

        public CacheEngine(ICacheProvider provider)
        {
            _provider = provider;
        }

        public T Get<T>(string key, Expression<Func<T>> expression) where T : class
        {
            return _provider.Get(key, expression);
        }
        public T Get<T>(Expression<Func<T>> expression) where T : class
        {
            var key = BuildKey(expression);
            return _provider.Get(key, expression);
        }

        private static string GetArgumentValue(Expression element)
        {
            var lambda = Expression.Lambda(Expression.Convert(element, element.Type));
            return lambda.Compile().DynamicInvoke().ToString();
        }

        public void Bust(string key)
        {
            _provider.Bust(key);
        }
        public void Bust<T>(Expression<Func<T>> expression)
        {
            _provider.Bust(BuildKey(expression));
        }

        public string BuildKey<T>(Expression<Func<T>> expression)
        {
            var method = expression.Body as MethodCallExpression;

            if (method == null)
            {
                return null;
            }
            if (method.Method.ReflectedType != null)
                return BuildKey(string.Format("{0}.{1}", method.Method.ReflectedType.FullName, method.Method.Name), method.Arguments.Select(GetArgumentValue).ToArray());
            throw new Exception("ReflectedType");
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