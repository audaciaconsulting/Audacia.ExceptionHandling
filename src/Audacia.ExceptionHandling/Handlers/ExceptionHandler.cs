using System;
using System.Collections.Generic;
using Audacia.ExceptionHandling.Results;
using Microsoft.Extensions.Logging;

namespace Audacia.ExceptionHandling.Handlers
{
    /// <summary>
    /// A wrapper around how to handle different exception types.
    /// </summary>
    /// <typeparam name="TException">The type of exception being handled.</typeparam>
    public class ExceptionHandler<TException> : IExceptionHandler
        where TException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ExceptionHandler{TException}"/>.
        /// </summary>
        /// <param name="action">The action to run on the given exception type to return the expected result.</param>
        /// <param name="log">The action to run on the exception to log it.</param>
        /// <param name="errorResponseType">The error type to be displayed on the error response.</param>
        public ExceptionHandler(
            Func<TException, IEnumerable<IHandledError>> action,
            Action<ILogger, TException>? log,
            string? errorResponseType)
        {
            Action = action;
            LogAction = log;
            ResponseType = errorResponseType ?? typeof(TException).Name;
        }

        /// <summary>
        /// Gets the function which transforms the exception into <see typeparamref="TResult"/>.
        /// </summary>
        public Func<TException, IEnumerable<IHandledError>> Action { get; }

        /// <summary>
        /// Gets the function which transforms the exception into <see typeparamref="TResult"/>.
        /// </summary>
        public Action<ILogger, TException>? LogAction { get; }

        /// <summary>
        /// Gets the response type to be displayed on the <see cref="ErrorResponse"/>.
        /// This is the configured exception type by default,
        /// which means if you configured a base exception but then throw an inherited exception,
        /// that the base exception type will show here.
        /// </summary>
        public string ResponseType { get; }

        /// <summary>
        /// Call the action. This is a wrapper such that you don't need to know the type of <see typeparamref="TException"/> or <see typeparamref="TResult"/> at the time of execution.
        /// </summary>
        /// <param name="exception">The exception to be processed.</param>
        /// <returns>The result that this exception products.</returns>
        /// <exception cref="ArgumentException">If the passed exception is not of the correct type an exception is thrown.</exception>
        public IEnumerable<IHandledError> Invoke(Exception exception)
        {
            if (exception is TException ex)
            {
                return Action.Invoke(ex);
            }

            throw new ArgumentException($"Exception is not of type {typeof(TException)}");
        }

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="logger">An instance of <see cref="ILogger"/>.</param>
        /// <param name="exception">The exception that has been encountered.</param>
        /// <returns>True if the exception was logged, false if not, an exception if the exception input is not of the correct type.</returns>
        /// <exception cref="ArgumentException">If the exception type passed in doesn't match the type for the handler an argument exception is thrown.</exception>
        public bool Log(ILogger logger, Exception exception)
        {
            if (exception is TException ex)
            {
                if (LogAction != null)
                {
                    LogAction.Invoke(logger, ex);
                    return true;
                }

                return false;
            }

            throw new ArgumentException($"Exception is not of type {typeof(TException)}");
        }
    }
}