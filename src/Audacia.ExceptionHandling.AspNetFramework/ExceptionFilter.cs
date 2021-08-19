using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Audacia.ExceptionHandling.Extensions;
using Audacia.ExceptionHandling.Handlers;
using Audacia.ExceptionHandling.Results;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Audacia.ExceptionHandling.AspNetFramework
{
    /// <summary>Handles exceptions and produces standardised error responses from them.</summary>
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ExceptionHandlerProvider _provider;

        /// <summary>Create a new <see cref="ExceptionFilter"/> instance.</summary>
        /// <param name="loggerFactory">Logger factory, required for attaching customer references to error logs.</param>
        /// <param name="provider">Provides exception hanlders to gracefully handle failures.</param>
        public ExceptionFilter(ILoggerFactory loggerFactory, ExceptionHandlerProvider provider)
        {
            _loggerFactory = loggerFactory;
            _provider = provider;
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

#pragma warning disable AV1710 // Member name includes the name of its including type.
#pragma warning disable ACL1002 // Member or local function contains too many statements
        /// <inheritdoc />
        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
#pragma warning restore ACL1002 // Member or local function contains too many statements
#pragma warning restore AV1710 // Member name includes the name of its including type.
        {
            if (actionExecutedContext == null)
            {
                throw new ArgumentNullException(nameof(actionExecutedContext));
            }

            var exception = Flatten(actionExecutedContext.Exception);
            var exceptionType = exception.GetType();

            // Find the related exception handler
            var handler = _provider.ResolveExceptionHandler(exceptionType);
            
            // Generate a customer reference for the current exception
            var reference = StringExtensions.GetCustomerReference();
            
            // Create a logger scope to attach the customer reference to log messages
            var logger = _loggerFactory.CreateLogger("ExceptionHandler");

            // PLEASE NOTE: IncludeScopes MUST be enabled on the logging provider to see this value
            using (logger.BeginScope("{CustomerReference}", reference))
            using (logger.BeginScope("{ExceptionData}", JsonConvert.SerializeObject(exception.Data)))
            {
                // Run the log action on the exception handler
                _provider.Log(logger, handler, exception);
            }
            
            if (handler == null)
            {
                // When no exception handler is found return a default error response with the customer reference
                var unhandledExceptionResponse = new ErrorResponse(reference, exceptionType);

                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, unhandledExceptionResponse);

                return Task.CompletedTask;
            }

            // Handle the exception and generate an API response
            var handledErrorMessages = handler.Invoke(actionExecutedContext.Exception);

            var errorResponse = new ErrorResponse(reference, handler.ResponseType, handledErrorMessages);

            var statusCode = GetStatusCode(handler);

            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(statusCode, errorResponse);

            return Task.CompletedTask;
        }
    }
}