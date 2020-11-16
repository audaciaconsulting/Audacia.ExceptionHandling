using System;

namespace Audacia.ExceptionHandling.Handlers
{
    /// <summary>
    /// A wrapper around how to handle different exception types.
    /// </summary>
    /// <typeparam name="TException">The type of exception being handled.</typeparam>
    /// <typeparam name="TResult">The result type that is returned.</typeparam>
    public class ExceptionHandler<TException, TResult> : IExceptionHandler
        where TException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ExceptionHandler{TException, TResult}"/>.
        /// </summary>
        /// <param name="action">The action to run on the given exception type to return the expected result.</param>
        /// <param name="log">The (optional) action to run on the exception to log it.</param>
        public ExceptionHandler(Func<TException, TResult> action, Action<TException>? log = null)
        {
            Action = action;
            LogAction = log;
        }

        /// <summary>Gets the type of Exception this handler handles.</summary>
        public Type ExceptionType => typeof(TException);

        /// <summary>Gets the type of Result this handler outputs.</summary>
        public Type ResultType => typeof(TResult);

        /// <summary>Gets the function which transforms the exception into <see typeparamref="TResult"/>.</summary>
        public Func<TException, TResult> Action { get; }

        /// <summary>Gets the function which transforms the exception into <see typeparamref="TResult"/>.</summary>
        public Action<TException>? LogAction { get; }

        /// <summary>
        /// Call the action. This is a wrapper such that you don't need to know the type of <see typeparamref="TException"/> or <see typeparamref="TResult"/> at the time of execution.
        /// </summary>
        /// <param name="exception">The exception to be processed.</param>
        /// <returns>The result that this exception products.</returns>
        /// <exception cref="ArgumentException">If the passed exception is not of the correct type an exception is thrown.</exception>
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
        /// <exception cref="ArgumentException">If the exception type passed in doesn't match the type for the handler an argument exception is thrown.</exception>
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
}