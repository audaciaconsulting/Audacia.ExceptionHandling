using System;
using System.Collections.Generic;
using Audacia.ExceptionHandling.Handlers;

namespace Audacia.ExceptionHandling.Extensions
{
    /// <summary>
    /// A set of extension methods for an exception handler mapping dictionary.
    /// </summary>
    public static class ExceptionHandlerMapExtensions
    {
        /// <summary>
        /// Add an <see cref="ExceptionHandler{TException}"/> to the map.
        /// </summary>
        /// <param name="map">An exception handler mapping dictionary.</param>
        /// <param name="exceptionHandler">The action to create the <see cref="ExceptionHandler{TException}"/> with.</param>
        /// <typeparam name="TException">The type of <see cref="Exception"/>.</typeparam>
        /// <exception cref="ArgumentNullException"><paramref name="map"/> is <see langword="null"/>.</exception>
        public static void AddExceptionHandler<TException>(
            this IDictionary<Type, IExceptionHandler> map,
            IExceptionHandler exceptionHandler)
            where TException : Exception
        {
            if (map == null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            map.Add(typeof(TException), exceptionHandler);
        }

        /// <summary>
        /// Get the handler when you just have an exception, but don't know the type.
        /// You can call this method after getting the type using <see cref="Type.GetType()"/>.
        /// </summary>
        /// <param name="map">An exception handler mapping dictionary.</param>
        /// <param name="exceptionType">The type of exception to handle.</param>
        /// <returns>An exception handler instance if there is one.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="map"/> or <paramref name="exceptionType"/> is <see langword="null"/>.</exception>
        public static IExceptionHandler? GetExceptionHandler(this IDictionary<Type, IExceptionHandler> map, Type exceptionType)
        {
            if (map == null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (exceptionType == null)
            {
                throw new ArgumentNullException(nameof(exceptionType));
            }

            var types = new List<Type>
            {
                exceptionType
            };

            types.AddRange(exceptionType.InheritanceHierarchy());

            foreach (var inheritedType in types)
            {
                if (map.TryGetValue(inheritedType, out var handler))
                {
                    return handler;
                }
            }

            return null;
        }

        /// <summary>
        /// Get the handler when you know the exception type.
        /// </summary>
        /// <param name="map">An exception handler mapping dictionary.</param>
        /// <typeparam name="TException">The type of exception to return a handler for.</typeparam>
        /// <returns>The handler for the given exception.</returns>
        public static IExceptionHandler? GetExceptionHandler<TException>(this IDictionary<Type, IExceptionHandler> map)
            where TException : Exception
        {
            return GetExceptionHandler(map, typeof(TException));
        }
    }
}
