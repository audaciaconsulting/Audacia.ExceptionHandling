using System;
using System.Collections.Generic;
using Audacia.ExceptionHandling.Handlers;

namespace Audacia.ExceptionHandling.Extensions
{
    /// <summary>
    /// A set of extension methods for an exception handler mapping dictionary.
    /// </summary>
    internal static class ExceptionHandlerMapExtensions
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
    }
}
