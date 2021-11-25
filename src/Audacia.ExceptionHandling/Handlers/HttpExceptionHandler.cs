using System;
using System.Collections.Generic;
using System.Net;
using Audacia.ExceptionHandling.Results;
using Microsoft.Extensions.Logging;

namespace Audacia.ExceptionHandling.Handlers
{
    /// <summary>
    /// An exception handler specific to applications that use HTTP.
    /// </summary>
    /// <typeparam name="TException">The type of exception being handled.</typeparam>
    public class HttpExceptionHandler<TException> : ExceptionHandler<TException>, IHttpExceptionHandler
        where TException : Exception
    {
        /// <summary>Gets the HTTP Status code to set on the response.</summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Create a new instance of <see cref="HttpExceptionHandler{TException}"/>.
        /// </summary>
        /// <param name="action">The action to run to get the result type.</param>
        /// <param name="statusCode">The Http Status code to return on this error type.</param>
        /// <param name="log">The action to log the exception.</param>
        /// <param name="errorResponseType">The error type to be displayed on the error response.</param>
        public HttpExceptionHandler(
            Func<TException, IEnumerable<ErrorResult>> action,
            HttpStatusCode statusCode,
            Action<ILogger, TException>? log,
            string? errorResponseType)
            : base(action, log, errorResponseType)
        {
            StatusCode = statusCode;
        }
    }
}