using System;
using System.Collections.Generic;
using System.Net;
using Audacia.ExceptionHandling.Extensions;
using Audacia.ExceptionHandling.Handlers;
using Audacia.ExceptionHandling.Results;
using Microsoft.Extensions.Logging;

namespace Audacia.ExceptionHandling
{
    /// <summary>
    /// A builder that is used to configure how exceptions are handled.
    /// </summary>
    public sealed class ExceptionHandlerProviderBuilder
    {
        private readonly IDictionary<Type, IExceptionHandler> _exceptionHandlerMap = new Dictionary<Type, IExceptionHandler>();

        private Action<ILogger, Exception> _defaultLogAction = new Action<ILogger, Exception>((logger, ex) => logger.LogError(ex, ex.Message));

        /// <summary>
        /// Add a handler to manage a given exception type.
        /// </summary>
        /// <param name="handlerAction">The action to run given the exception to return the result type.</param>
        /// <param name="logAction">(Optional) How to log this specific type of exception.</param>
        /// <typeparam name="TException">The type of exception to handle.</typeparam>
        /// <returns>The <see cref="ExceptionHandlerProviderBuilder"/> instance.</returns>
        public ExceptionHandlerProviderBuilder Handle<TException>(
            Func<TException, IHandledError> handlerAction,
            Action<ILogger, TException>? logAction = null)
            where TException : Exception
        {
            // Allow developers to return as single handler result, we still need it as an enumerable
            var handlerWrapper = new Func<TException, IEnumerable<IHandledError>>((ex) => 
            {
                var handledErrorModel = handlerAction.Invoke(ex);
                return new IHandledError[] { handledErrorModel };
            });

            return Handle(handlerWrapper, logAction);
        }

        /// <summary>
        /// Add a handler to manage a given exception type.
        /// </summary>
        /// <param name="handlerAction">The action to run given the exception to return the result type.</param>
        /// <param name="statusCode">The status code to return after handling the exception.</param>
        /// <param name="logAction">(Optional) How to log this specific type of exception.</param>
        /// <typeparam name="TException">The type of exception to handle.</typeparam>
        /// <returns>The <see cref="ExceptionHandlerProviderBuilder"/> instance.</returns>
        public ExceptionHandlerProviderBuilder Handle<TException>(
            Func<TException, IHandledError> handlerAction,
            HttpStatusCode statusCode,
            Action<ILogger, TException>? logAction = null)
            where TException : Exception
        {
            // Allow developers to return as single handler result, we still need it as an enumerable
            var handlerWrapper = new Func<TException, IEnumerable<IHandledError>>((ex) => 
            {
                var handledErrorModel = handlerAction.Invoke(ex);
                return new IHandledError[] { handledErrorModel };
            });

            return Handle(handlerWrapper, statusCode, logAction);
        }

#pragma warning disable AV1551
        /// <summary>
        /// Add a handler to manage a given exception type.
        /// </summary>
        /// <param name="handlerAction">The action to run given the exception to return the result type.</param>
        /// <param name="logAction">(Optional) How to log this specific type of exception.</param>
        /// <typeparam name="TException">The type of exception to handle.</typeparam>
        /// <returns>The <see cref="ExceptionHandlerProviderBuilder"/> instance.</returns>
        public ExceptionHandlerProviderBuilder Handle<TException>(
            Func<TException, IEnumerable<IHandledError>> handlerAction,
            Action<ILogger, TException>? logAction = null)
            where TException : Exception
        {
            var handler = new ExceptionHandler<TException>(handlerAction, logAction);
            _exceptionHandlerMap.AddExceptionHandler<TException>(handler);
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
        /// <returns>The <see cref="ExceptionHandlerProviderBuilder"/> instance.</returns>
        public ExceptionHandlerProviderBuilder Handle<TException>(
            Func<TException, IEnumerable<IHandledError>> handlerAction,
            HttpStatusCode statusCode,
            Action<ILogger, TException>? logAction = null)
            where TException : Exception
        {
            var handler = new HttpExceptionHandler<TException>(handlerAction, statusCode, logAction);
            _exceptionHandlerMap.AddExceptionHandler<TException>(handler);
            return this;
        }

        /// <summary>
        /// Add a handler to manage a given exception type.
        /// </summary>
        /// <param name="handler">The handler to add to the collection.</param>
        /// <typeparam name="TException">The type of exception to handle.</typeparam>
        /// <returns>The <see cref="ExceptionHandlerProviderBuilder"/> instance.</returns>
        public ExceptionHandlerProviderBuilder Handle<TException>(IExceptionHandler handler)
            where TException : Exception
        {
            _exceptionHandlerMap.AddExceptionHandler<TException>(handler);
            return this;
        }

        /// <summary>
        /// Setup the logging for how to deal with all exceptions, unless overridden.
        /// </summary>
        /// <param name="loggingAction">The action to run on logging the exception.</param>
        /// <returns>The <see cref="ExceptionHandlerProviderBuilder"/> instance.</returns>
        public ExceptionHandlerProviderBuilder WithDefaultLogging(Action<ILogger, Exception> loggingAction)
        {
            _defaultLogAction = loggingAction;
            return this;
        }

        /// <summary>
        /// Return the options that will be used to handle exceptions.
        /// </summary>
        /// <returns>An instance of <see cref="ExceptionHandlerProvider"/>.</returns>
        public ExceptionHandlerProvider Build()
        {
            return new ExceptionHandlerProvider(_exceptionHandlerMap, _defaultLogAction);
        }
    }
}