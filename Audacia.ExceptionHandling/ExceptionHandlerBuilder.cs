using System;
using System.Collections.Generic;
using System.Net;
using Audacia.ExceptionHandling.Handlers;

namespace Audacia.ExceptionHandling
{
    /// <summary>Fluent API interface for configuring how exceptions are handled.</summary>
    public class ExceptionHandlerBuilder
    {
        private readonly IDictionary<Type, IExceptionHandler> _exceptionToHandlerMap =
            new Dictionary<Type, IExceptionHandler>();

        private void Add<TException>(IExceptionHandler handler)
            where TException : Exception
        {
            _exceptionToHandlerMap.Add(typeof(TException), handler);
        }

        public ExceptionHandlerBuilder Add<TException, TResult>(Func<TException, TResult> action)
            where TException : Exception
        {
            var handler = new ExceptionHandler<TException, TResult>(action);
            Add<TException>(handler);
            return this;
        }

        public ExceptionHandlerBuilder Add<TException, TResult>(
            Func<TException, TResult> action,
            HttpStatusCode statusCode)
            where TException : Exception
        {
            var handler = new HttpExceptionHandler<TException, TResult>(action, statusCode);
            Add<TException>(handler);
            return this;
        }

        /// <summary>
        /// Get the handler when you just have an exception, but don't know the type.
        /// You can call this method after getting the type using <see cref="Type.GetType()"/>.
        /// </summary>
        /// <param name="exceptionType">The type of exception to handle.</param>
        /// <returns></returns>
        public IExceptionHandler? Get(Type exceptionType)
        {
            if (_exceptionToHandlerMap.TryGetValue(exceptionType, out var handler))
            {
                return handler;
            }

            return null;
        }

        /// <summary>
        /// Get the handler when you know the exception type.
        /// </summary>
        /// <typeparam name="TException">The type of exception to return a handler for.</typeparam>
        /// <returns>The handler for the given exception.</returns>
        public IExceptionHandler? Get<TException>()
            where TException : Exception
        {
            return Get(typeof(TException));
        }
    }
}