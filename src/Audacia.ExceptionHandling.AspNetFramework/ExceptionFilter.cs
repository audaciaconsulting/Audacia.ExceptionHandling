using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Audacia.ExceptionHandling.Handlers;

namespace Audacia.ExceptionHandling.AspNetFramework
{
    /// <summary>Handles exceptions and produces standardised error responses from them.</summary>
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ExceptionHandlerOptions _options;

        /// <summary>Create a new <see cref="ExceptionFilter"/> instance.</summary>
        public ExceptionFilter(ExceptionHandlerOptions options)
        {
            _options = options;
        }

        /// <inheritdoc />
        public bool AllowMultiple { get; } = false;

        private static Exception Flatten(Exception exception)
        {
            if (exception is AggregateException aggregateException)
            {
                exception = aggregateException.Flatten();

                if (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
            }

            return exception;
        }

        private static HttpStatusCode GetStatusCode(IExceptionHandler? handler)
        {
            var statusCode = HttpStatusCode.BadRequest;

            if (handler is IHttpExceptionHandler httpExceptionHandler)
            {
                statusCode = httpExceptionHandler.StatusCode;
            }

            return statusCode;
        }

        /// <inheritdoc />
        public Task ExecuteExceptionFilterAsync(
            HttpActionExecutedContext actionExecutedContext,
            CancellationToken cancellationToken)
        {
            if (actionExecutedContext == null)
            {
                throw new ArgumentNullException(nameof(actionExecutedContext));
            }

            var exception = Flatten(actionExecutedContext.Exception);

            var handler = _options.GetHandler(exception.GetType());

            _options.Log(handler, exception);

            if (handler == null)
            {
                return Task.CompletedTask;
            }

            var result = handler.Invoke(actionExecutedContext.Exception);
            var statusCode = GetStatusCode(handler);

            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(statusCode, result);

            return Task.CompletedTask;
        }
    }
}