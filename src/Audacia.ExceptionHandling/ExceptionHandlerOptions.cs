using System;
using Audacia.ExceptionHandling.Handlers;

namespace Audacia.ExceptionHandling
{
    /// <summary>
    /// The options that describe how to deal with exceptions.
    /// </summary>
    public class ExceptionHandlerOptions
    {
        /// <summary>
        /// Gets a collection of handlers.
        /// </summary>
        public ExceptionHandlerMap HandlerMap { get; } = new ExceptionHandlerMap();

        /// <summary>
        /// Gets an action to log any exception if an individual handler doesn't have a logger.
        /// </summary>
        public Action<Exception>? Logging { get; internal set; }

        /// <summary>
        /// Get the handler when you just have an exception, but don't know the type.
        /// You can call this method after getting the type using <see cref="Type.GetType()"/>.
        /// </summary>
        /// <param name="exceptionType">The type of exception to handle.</param>
        /// <returns>An exception handler if one has been setup.</returns>
        public virtual IExceptionHandler? GetHandler(Type exceptionType)
        {
            return HandlerMap.Get(exceptionType);
        }

        /// <summary>
        /// Get the handler when you know the exception type.
        /// </summary>
        /// <typeparam name="TException">The type of exception to return a handler for.</typeparam>
        /// <returns>An exception handler if one has been setup</returns>
        public IExceptionHandler? GetHandler<TException>()
            where TException : Exception
        {
            return GetHandler(typeof(TException));
        }

        /// <summary>
        /// Try log an exception with the handler. If it isn't logged, try log it with the overall logger.
        /// </summary>
        /// <param name="handler">The handler to log from.</param>
        /// <param name="exception">The exception to try log.</param>
        public void Log(IExceptionHandler? handler, Exception exception)
        {
            if (handler?.Log(exception) ?? false)
            {
                return;
            }

            Logging?.Invoke(exception);
        }
    }
}