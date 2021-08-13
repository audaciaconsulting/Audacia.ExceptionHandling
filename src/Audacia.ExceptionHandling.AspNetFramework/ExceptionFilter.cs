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

namespace Audacia.ExceptionHandling.AspNetFramework
{
    /// <summary>Handles exceptions and produces standardised error responses from them.</summary>
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ExceptionHandlerOptions _options;

        /// <summary>Create a new <see cref="ExceptionFilter"/> instance.</summary>
        /// <param name="loggerFactory">Logger factory, required for attaching customer references to error logs.</param>
        /// <param name="options">The options that will be used to handle exceptions.</param>
        public ExceptionFilter(ILoggerFactory loggerFactory, ExceptionHandlerOptions options)
        {
            _loggerFactory = loggerFactory;
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

            // Find the related exception handler
            var handler = _options.GetHandler(exception.GetType());
            
            // Generate a customer reference for the current exception
            var customerReference = StringExtensions.GetCustomerReference();
            
            // Create a logger scope to attach the customer reference to log messages
            var logger = _loggerFactory.CreateLogger("ExceptionHandler");
            using (logger.BeginScope(nameof(ErrorResult.CustomerReference), customerReference))
            {
                // Run the log action on the exception handler
                _options.Log(logger, handler, exception);
            }
            
            if (handler == null)
            {
                return Task.CompletedTask;
            }

            // Handle the exception and generate an API response
            var result = handler.Invoke(customerReference, actionExecutedContext.Exception);
            var statusCode = GetStatusCode(handler);

            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(statusCode, result);

            return Task.CompletedTask;
        }
    }
}