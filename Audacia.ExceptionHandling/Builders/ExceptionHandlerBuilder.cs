using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;

namespace Audacia.ExceptionHandling.Builders
{
    /// <summary>Fluent API interface for configuring how exceptions are handled.</summary>
    public class ExceptionHandlerBuilder
    {
        private readonly IDictionary<TypeKey, ExceptionHandler<Exception, object>> _exceptionToHandlerMap =
            new Dictionary<TypeKey, ExceptionHandler<Exception, object>>();

        private static IEnumerable<TypeKey> GetKeys<TException>()
        {
            var exceptionType = typeof(TException);

            var keys = new List<TypeKey>
            {
                new TypeKey(exceptionType)
            };

            var hierarchy = typeof(TException).InheritanceHierarchy();

            keys.AddRange(hierarchy.Select(h => new TypeKey(exceptionType, h)));

            return keys.ToArray();
        }

        private void Add<TException, TResult>(TypeKey key, ExceptionHandler<TException, TResult> action)
            where TException : Exception
        {
            var castHandler = (action as ExceptionHandler<Exception, object>);
            _exceptionToHandlerMap.Add(key, castHandler!);
        }

        private void Add<TException, TResult>(ExceptionHandler<TException, TResult> handler)
            where TException : Exception
        {
            var keys = GetKeys<TException>();

            foreach (var key in keys)
            {
                if (!_exceptionToHandlerMap.ContainsKey(key))
                {
                    Add(key, handler);
                }
            }
        }

        public ExceptionHandlerBuilder Add<TException, TResult>(Func<TException, TResult> action)
            where TException : Exception
        {
            var handler = new ExceptionHandler<TException, TResult>(action);
            Add(handler);
            return this;
        }

        public ExceptionHandlerBuilder Add<TException, TResult>(Func<TException, TResult> action,
            HttpStatusCode statusCode)
            where TException : Exception
        {
            var handler = new HttpExceptionHandler<TException, TResult>(action, statusCode);
            Add(handler);
            return this;
        }

        public ExceptionHandler<Exception, object>? Get<TException>()
            where TException : Exception
        {
            var keys = GetKeys<TException>();

            foreach (var key in keys)
            {
                if (_exceptionToHandlerMap.ContainsKey(key))
                {
                    return _exceptionToHandlerMap[key];
                }
            }

            return null;
        }

        public ExceptionHandler<Exception, object>? Get<TException>(TException exception)
            where TException : Exception
        {
            return Get<TException>();
        }
    }
}