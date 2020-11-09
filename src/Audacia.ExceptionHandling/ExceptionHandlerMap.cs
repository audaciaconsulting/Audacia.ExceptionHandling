using System;
using System.Collections.Generic;
using System.Net;
using Audacia.ExceptionHandling.Handlers;

namespace Audacia.ExceptionHandling
{
    /// <summary>Fluent API interface for configuring how exceptions are handled.</summary>
    public sealed class ExceptionHandlerMap
    {
        private readonly IDictionary<Type, IExceptionHandler> _exceptionToHandlerMap =
            new Dictionary<Type, IExceptionHandler>();

#pragma warning disable AV1551 // Don't need to call another overload.
        internal void Add<TException>(IExceptionHandler handler)
            where TException : Exception
        {
            _exceptionToHandlerMap.Add(typeof(TException), handler);
        }
#pragma warning restore AV1551

        internal ExceptionHandlerMap Add<TException, TResult>(
            Func<TException, TResult> action,
            Action<TException>? logAction = null)
            where TException : Exception
        {
            var handler = new ExceptionHandler<TException, TResult>(action, logAction);
            Add<TException>(handler);
            return this;
        }

        internal ExceptionHandlerMap Add<TException, TResult>(
            Func<TException, TResult> action,
            HttpStatusCode statusCode,
            Action<TException>? logAction = null)
            where TException : Exception
        {
            var handler = new HttpExceptionHandler<TException, TResult>(action, statusCode, logAction);
            Add<TException>(handler);
            return this;
        }

        /// <summary>
        /// Get the handler when you just have an exception, but don't know the type.
        /// You can call this method after getting the type using <see cref="Type.GetType()"/>.
        /// </summary>
        /// <param name="exceptionType">The type of exception to handle.</param>
        /// <returns>An exception handler instance if there is one.</returns>
        internal IExceptionHandler? Get(Type exceptionType)
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
        internal IExceptionHandler? Get<TException>()
            where TException : Exception
        {
            return Get(typeof(TException));
        }
    }
}