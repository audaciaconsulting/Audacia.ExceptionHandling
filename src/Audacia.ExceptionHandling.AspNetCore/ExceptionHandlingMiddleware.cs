using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Audacia.ExceptionHandling.Extensions;
using Audacia.ExceptionHandling.Handlers;
using Audacia.ExceptionHandling.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Audacia.ExceptionHandling.AspNetCore
{
    /// <summary>
    /// A custom middleware for handling exceptions.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ExceptionHandlerProvider _provider;
        private readonly IResponseSerializer _responseSerializer;

        /// <summary>
        /// Create a new instance of <see cref="ExceptionHandlingMiddleware" />.
        /// </summary>
        /// <param name="next">The next method to call in the middleware pipeline.</param>
        /// <param name="loggerFactory">Logger factory, required for attaching customer references to error logs.</param>
        /// <param name="provider">Provides exception hanlders to gracefully handle failures.</param>
        /// <param name="responseSerializer">A <see cref="IResponseSerializer"/> instance that can serialize exception responses.</param>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, ExceptionHandlerProvider provider, IResponseSerializer responseSerializer)
        {
            _next = next;
            _loggerFactory = loggerFactory;
            _provider = provider;
            _responseSerializer = responseSerializer;
        }

        /// <summary>
        /// Run this step of the HTTP Request pipeline.
        /// </summary>
        /// <param name="context">The current HttpContext.</param>
        /// <returns>If there is an exception will set the response on the context, if there isn't one then nothing will happen.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is <see langword="null"/>.</exception>
        public async Task InvokeAsync(HttpContext context)
        {
#pragma warning disable CA1031 // Capture a more specific exception type
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                await OnExceptionAsync(exception, context).ConfigureAwait(false);
            }
#pragma warning restore CA1031
        }

        /// <summary>Handles the specified exception based on the configured <see cref="ExceptionHandlerProvider"/>.</summary>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is <see langword="null"/>.</exception>
#pragma warning disable ACL1002 // Member or local function contains too many statements
        private Task OnExceptionAsync(Exception exception, HttpContext context)
#pragma warning restore ACL1002 // Member or local function contains too many statements
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            exception = Flatten(exception);

            // Get exception type
            var type = exception.GetType();

            // Find the related exception handler
            var handler = _provider.ResolveExceptionHandler(type);

            // Generate a customer reference for the current exception
            var reference = StringExtensions.GetCustomerReference();

            // Create a logger scope to attach the customer reference to log messages
            var logger = _loggerFactory.CreateLogger("ExceptionHandler");

            // PLEASE NOTE: IncludeScopes MUST be enabled on the logging provider to see this value
            using (logger.BeginScope("{CustomerReference}", reference))
            using (logger.BeginScope("{ExceptionData}", _responseSerializer.Serialize(exception.Data)))
            {
                // Run the log action on the exception handler
                _provider.Log(logger, handler, exception);
            }

            if (handler == null)
            {
                // When no exception handler is found return a default error response with the customer reference
                var unhandledExceptionResponse = new ErrorResponse(reference);

                return SetResponseAsync(context, unhandledExceptionResponse, HttpStatusCode.InternalServerError);
            }

            // Handle the exception and generate an API response
            var handledErrorMessages = handler.Invoke(exception);

            var errorResponse = new ErrorResponse(reference, handler.ResponseType, handledErrorMessages);

            var statusCode = GetStatusCode(handler);

            return SetResponseAsync(context, errorResponse, statusCode);
        }

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

        private Task SetResponseAsync(HttpContext context, object? result, HttpStatusCode statusCode)
        {
            context.Response.Clear();
            context.Response.StatusCode = (int)statusCode;

            if (result != null)
            {
                return SetResponseBodyAsync(result, context.Response);
            }

            return Task.CompletedTask;
        }

        private Task SetResponseBodyAsync(object result, HttpResponse response)
        {
            var json = _responseSerializer.Serialize(result);
            
            response.Headers.Add("Content-Type", "application/json");

            var value = Encoding.UTF8.GetByteCount(json).ToString(CultureInfo.InvariantCulture);

            response.Headers.Add("Content-Length", value);
            
            return response.WriteAsync(json, Encoding.UTF8);
        }
    }
}