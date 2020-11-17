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
        /// <summary>
        /// Add an <see cref="IExceptionHandler"/> to the map.
        /// </summary>
        /// <param name="handler">The handler to the add to the Map.</param>
        /// <typeparam name="TException">The type of <see cref="Exception"/> that is being handled.</typeparam>
        /// <returns>The instance of the <see cref="ExceptionHandlerMap"/> that is being acted upon.</returns>
        internal ExceptionHandlerMap Add<TException>(IExceptionHandler handler)
            where TException : Exception
        {
            _exceptionToHandlerMap.Add(typeof(TException), handler);
            return this;
        }
#pragma warning restore AV1551

        /// <summary>
        /// Add an <see cref="ExceptionHandler{TException,TResult}"/> to the map.
        /// </summary>
        /// <param name="action">The action to create the <see cref="ExceptionHandler{TException,TResult}"/> with.</param>
        /// <param name="logAction">The action to log the exception with.</param>
        /// <typeparam name="TException">The type of <see cref="Exception"/>.</typeparam>
        /// <typeparam name="TResult">The type of result that the action returns.</typeparam>
        /// <returns>The instance of the <see cref="ExceptionHandlerMap"/> that is being acted upon.</returns>
        internal ExceptionHandlerMap Add<TException, TResult>(
            Func<TException, TResult> action,
            Action<TException>? logAction = null)
            where TException : Exception
        {
            var handler = new ExceptionHandler<TException, TResult>(action, logAction);
            Add<TException>(handler);
            return this;
        }

        /// <summary>
        /// Add an <see cref="HttpExceptionHandler{TException,TResult}"/> to the map.
        /// </summary>
        /// <param name="action">The action to create the <see cref="HttpExceptionHandler{TException,TResult}"/> with.</param>
        /// <param name="statusCode">The <see cref="HttpStatusCode"/> to create the handler with.</param>
        /// <param name="logAction">The action to log the exception with.</param>
        /// <typeparam name="TException">The type of <see cref="Exception"/>.</typeparam>
        /// <typeparam name="TResult">The type of result that the action returns.</typeparam>
        /// <returns>The instance of the <see cref="ExceptionHandlerMap"/> that is being acted upon.</returns>
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
            var types = new List<Type>
            {
                exceptionType
            };
            types.AddRange(exceptionType.InheritanceHierarchy());

            foreach (var inheritedType in types)
            {
                if (_exceptionToHandlerMap.TryGetValue(inheritedType, out var handler))
                {
                    return handler;
                }
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