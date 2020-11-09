using System;
using System.Net;

namespace Audacia.ExceptionHandling.Handlers
{
    /// <summary>
    /// An exception handler specific to applications that use HTTP.
    /// </summary>
    /// <typeparam name="TException">The type of exception being handled.</typeparam>
    /// <typeparam name="TResult">The result type that is returned.</typeparam>
    public class HttpExceptionHandler<TException, TResult> : ExceptionHandler<TException, TResult>,
        IHttpExceptionHandler
        where TException : Exception
    {
        /// <summary>Gets the HTTP Status code to set on the response.</summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Create a new instance of <see cref="HttpExceptionHandler{TException,TResult}"/>
        /// </summary>
        /// <param name="action">The action to run to get the result type.</param>
        /// <param name="statusCode">The Http Status code to return on this error type.</param>
        /// <param name="log">The action to log the exception (optional).</param>
        public HttpExceptionHandler(
            Func<TException, TResult> action,
            HttpStatusCode statusCode,
            Action<TException>? log = null) : base(action, log)
        {
            StatusCode = statusCode;
        }
    }
}