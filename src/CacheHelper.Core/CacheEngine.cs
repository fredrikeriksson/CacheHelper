using System;
using System.Linq;
using System.Linq.Expressions;
using CacheHelper.Core.Exceptions;

namespace CacheHelper.Core
{
    public class CacheEngine
    {
        private readonly ICacheProvider _provider;

        public CacheEngine(ICacheProvider provider)
        {
            _provider = provider;
        }

        public T Get<T>(string key, Expression<Func<T>> expression, TimeSpan? expires = null)
        {
            return _provider.Get(key, expression, expires);
        }
        public T Get<T>(Expression<Func<T>> expression, TimeSpan? expires = null)
        {
            var key = BuildKey(expression);
            return _provider.Get(key, expression, expires);
        }

        private static string GetArgumentValue(Expression element)
        {
            var lambda = Expression.Lambda(Expression.Convert(element, element.Type));

            var invoked = lambda.Compile().DynamicInvoke();
            var properties = invoked.GetType().GetProperties();
            if (!(invoked is string) && properties.Any())
                return string.Join("_", properties.Select(prop => prop.GetValue(invoked, null)));
            return invoked.ToString();
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
                throw new ExpressionBodyNotSupported();
            }
            if (method.Method.ReflectedType != null)
            {
                return BuildKey(string.Format("{0}.{1}", method.Method.ReflectedType.FullName, method.Method.Name), method.Arguments.Select(GetArgumentValue).ToArray());
            }
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