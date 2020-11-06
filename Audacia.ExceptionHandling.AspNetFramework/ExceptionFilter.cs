using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Audacia.ExceptionHandling.Builders;

namespace Audacia.ExceptionHandling.AspNetFramework
{
    /// <summary>Handles exceptions and produces standardised error responses from them.</summary>
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ExceptionHandlerBuilder _builder;

        /// <summary>Create a new <see cref="ExceptionFilter"/> instance.</summary>
        public ExceptionFilter(ExceptionHandlerBuilder builder)
        {
            _builder = builder;
        }

        /// <inheritdoc />
        public bool AllowMultiple { get; } = false;

        /// <inheritdoc />
        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext context, CancellationToken cancellationToken)
        {
            var exception = context.Exception;
            if (exception is AggregateException aggregateException)
            {
                exception = aggregateException.Flatten();

                if (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
            }

            var handler = _builder.Get(exception);

            if (handler == null)
            {
                return Task.CompletedTask;
            }

            var result = handler.Invoke(context.Exception);

            var statusCode = HttpStatusCode.BadRequest;

            if (handler is IHttpExceptionHandler httpExceptionHandler)
            {
                statusCode = httpExceptionHandler.StatusCode;
            }

            context.Response = context.Request.CreateResponse(statusCode, result);

            return Task.CompletedTask;
        }
    }
}