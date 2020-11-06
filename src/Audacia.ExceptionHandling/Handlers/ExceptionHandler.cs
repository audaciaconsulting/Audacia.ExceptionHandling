using System;

namespace Audacia.ExceptionHandling.Handlers
{
    /// <summary>
    /// A wrapper around how to handle different exception types
    /// </summary>
    /// <typeparam name="TException">The type of exception being handled.</typeparam>
    /// <typeparam name="TResult">The result type that is returned.</typeparam>
    public class ExceptionHandler<TException, TResult> : IExceptionHandler
        where TException : Exception
    {
        /// <summary>Initializes a new instance of <see cref="ExceptionHandler{TException, TResult}"/></summary>
        public ExceptionHandler(Func<TException, TResult> action, Action<TException>? log = null)
        {
            Action = action;
            LogAction = log;
        }

        /// <summary>The type of Exception this handler handles.</summary>
        public Type ExceptionType => typeof(TException);

        /// <summary>The type of Result this handler outputs.</summary>
        public Type ResultType => typeof(TResult);

        /// <summary>The function which transforms the exception into <see cref="TResult"/>.</summary>
        public Func<TException, TResult> Action { get; }

        /// <summary>The function which transforms the exception into <see cref="TResult"/>.</summary>
        public Action<TException>? LogAction { get; }

        /// <summary>
        /// Call the action. This is a wrapper such that
        /// </summary>
        /// <param name="exception">The exception to be processed</param>
        /// <returns>The result that this exception products.</returns>
        /// <exception cref="ArgumentException">If the passed exception is not of the correct type an exception is thrown</exception>
        public object? Invoke(Exception exception)
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
        /// <param name="exception">The exception that has been encountered.</param>
        /// <returns>True if the exception was logged, false if not, an exception if the exception input is not of the correct type.</returns>
        public bool Log(Exception exception)
        {
            if (exception is TException ex)
            {
                if (LogAction != null)
                {
                    LogAction.Invoke(ex);
                    return true;
                }

                return false;
            }

            throw new ArgumentException($"Exception is not of type {typeof(TException)}");
        }
    }

    /// <summary>
    /// Interface describing the exception handler that can be used with a generic argument.
    /// </summary>
    public interface IExceptionHandler
    {
        /// <summary>
        /// Given an exception return a result.
        /// </summary>
        /// <param name="exception">The exception that has been encountered.</param>
        /// <returns>The error result that has been setup.</returns>
        public object? Invoke(Exception exception);

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="exception">The exception that has been encountered.</param>
        /// <returns>True if the exception was logged, false if not, an exception if the exception input is not of the correct type.</returns>
        public bool Log(Exception exception);
    }
}