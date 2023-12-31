﻿using System;
using System.Collections.Generic;
using Audacia.ExceptionHandling.Extensions;
using Audacia.ExceptionHandling.Handlers;
using Microsoft.Extensions.Logging;

namespace Audacia.ExceptionHandling
{
    /// <summary>
    /// Provides an exception handler to deal with mapped exceptions.
    /// </summary>
    public sealed class ExceptionHandlerProvider
    {
        /// <summary>
        /// Gets a mapped collection of exception handlers.
        /// </summary>
        internal IDictionary<Type, IExceptionHandler> HandlerMap { get; } = new Dictionary<Type, IExceptionHandler>();

        /// <summary>
        /// Gets an action to log any exception.
        /// To be used when an individual handler doesn't have a logger.
        /// </summary>
        internal Action<ILogger, Exception>? DefaultLogAction { get; }

        /// <summary>
        /// Creates an instance of <see cref="ExceptionHandlerProvider"/>.
        /// </summary>
        /// <param name="exceptionHandlerMap">A mapped collection of exception handlers.</param>
        /// <param name="defaultLogAction">An action to log any exception.</param>
        public ExceptionHandlerProvider(
            IDictionary<Type, IExceptionHandler> exceptionHandlerMap,
            Action<ILogger, Exception> defaultLogAction)
        {
            HandlerMap = exceptionHandlerMap;
            DefaultLogAction = defaultLogAction;
        }

        /// <summary>
        /// Resolves an exception handler for the provided exception type.
        /// </summary>
        /// <param name="exceptionType">The type of exception to handle.</param>
        /// <returns>An exception handler if one has been setup.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="exceptionType"/> is <see langword="null"/>.</exception>
        public IExceptionHandler? ResolveExceptionHandler(Type exceptionType)
        {
            if (exceptionType == null)
            {
                throw new ArgumentNullException(nameof(exceptionType));
            }

            var types = new List<Type>
            {
                exceptionType
            };

            var exceptionTypeHierarchy = exceptionType.InheritanceHierarchy();

            types.AddRange(exceptionTypeHierarchy);

            foreach (var inheritedType in types)
            {
                if (HandlerMap.TryGetValue(inheritedType, out var handler))
                {
                    return handler;
                }
            }

            return null;
        }

        /// <summary>
        /// Resolves an exception handler for the provided exception type.
        /// </summary>
        /// <typeparam name="TException">The type of exception to handle.</typeparam>
        /// <returns>An exception handler if one has been setup.</returns>
        public IExceptionHandler? ResolveExceptionHandler<TException>()
        {
            return ResolveExceptionHandler(typeof(TException));
        }

        /// <summary>
        /// Attempts to log an exception using the provided handler.
        /// When the handler is unable to log the exception, then the default log action will be called.
        /// </summary>
        /// <param name="logger">An instance of <see cref="ILogger"/>.</param>
        /// <param name="handler">The handler to log from.</param>
        /// <param name="exception">The exception to try log.</param>
        public void Log(ILogger logger, IExceptionHandler? handler, Exception exception)
        {
            if (handler?.Log(logger, exception) ?? false)
            {
                return;
            }

            DefaultLogAction?.Invoke(logger, exception);
        }
    }
}