using System;
using System.Net;
using Audacia.ExceptionHandling.Handlers;

namespace Audacia.ExceptionHandling
{
    /// <summary>
    /// A builder that is used to configure how exceptions are handled.
    /// </summary>
    public class ExceptionHandlerOptionsBuilder
    {
        private readonly ExceptionHandlerOptions _options = new ExceptionHandlerOptions();

#pragma warning disable AV1551
        /// <summary>
        /// Add a handler to manage a given exception type.
        /// </summary>
        /// <param name="handlerAction">The action to run given the exception to return the result type.</param>
        /// <param name="logAction">(Optional) How to log this specific type of exception.</param>
        /// <typeparam name="TException">The type of exception to handle.</typeparam>
        /// <typeparam name="TResult">The type of the result to return from handling the exception.</typeparam>
        /// <returns>The <see cref="ExceptionHandlerOptionsBuilder"/> instance.</returns>
        public ExceptionHandlerOptionsBuilder Handle<TException, TResult>(
            Func<TException, TResult> handlerAction,
            Action<TException>? logAction = null)
            where TException : Exception
        {
            _options.HandlerMap.Add(handlerAction, logAction);
            return this;
        }
#pragma warning restore AV1551

        /// <summary>
        /// Add a handler to manage a given exception type.
        /// </summary>
        /// <param name="handlerAction">The action to run given the exception to return the result type.</param>
        /// <param name="statusCode">The status code to return after handling the exception.</param>
        /// <param name="logAction">(Optional) How to log this specific type of exception.</param>
        /// <typeparam name="TException">The type of exception to handle.</typeparam>
        /// <typeparam name="TResult">The type of the result to return from handling the exception.</typeparam>
        /// <returns>The <see cref="ExceptionHandlerOptionsBuilder"/> instance.</returns>
        public virtual ExceptionHandlerOptionsBuilder Handle<TException, TResult>(
            Func<TException, TResult> handlerAction,
            HttpStatusCode statusCode,
            Action<TException>? logAction = null)
            where TException : Exception
        {
            _options.HandlerMap.Add(handlerAction, statusCode, logAction);
            return this;
        }

        /// <summary>
        /// Add a handler to manage a given exception type.
        /// </summary>
        /// <param name="handler">The handler to add to the collection.</param>
        /// <typeparam name="TException">The type of exception to handle.</typeparam>
        /// <returns>The <see cref="ExceptionHandlerOptionsBuilder"/> instance.</returns>
        public ExceptionHandlerOptionsBuilder AddHandler<TException>(
            IExceptionHandler handler)
            where TException : Exception
        {
            _options.HandlerMap.Add<TException>(handler);
            return this;
        }

        /// <summary>
        /// Setup the logging for how to deal with all exceptions, unless overridden.
        /// </summary>
        /// <param name="loggingAction">The action to run on logging the exception.</param>
        /// <returns>The <see cref="ExceptionHandlerOptionsBuilder"/> instance.</returns>
        public ExceptionHandlerOptionsBuilder WithDefaultLogging(Action<Exception> loggingAction)
        {
            _options.Logging = loggingAction;
            return this;
        }

        /// <summary>
        /// Return the options that will be used to handle exceptions.
        /// </summary>
        /// <returns>An instance of <see cref="ExceptionHandlerOptions"/>.</returns>
        public ExceptionHandlerOptions Build()
        {
            return _options;
        }
    }
}