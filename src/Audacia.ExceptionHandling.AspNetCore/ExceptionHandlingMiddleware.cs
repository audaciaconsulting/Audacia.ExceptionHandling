using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Audacia.ExceptionHandling.Handlers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Audacia.ExceptionHandling.AspNetCore
{
    /// <summary>
    /// A custom middleware for handling exceptions.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ExceptionHandlerOptions _options;

        /// <summary>
        /// Create a new instance of <see cref="ExceptionHandlingMiddleware" />.
        /// </summary>
        /// <param name="next">The next method to call in the middleware pipeline.</param>
        /// <param name="options">The options for how to handle exceptions.</param>
        public ExceptionHandlingMiddleware(RequestDelegate next, ExceptionHandlerOptions options)
        {
            _next = next;
            _options = options;
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

        /// <summary>Handles the specified exception based on the configured <see cref="ExceptionHandlerMap"/>.</summary>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is <see langword="null"/>.</exception>
        private Task OnExceptionAsync(Exception exception, HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            exception = Flatten(exception);

            var handler = _options.GetHandler(exception.GetType());

            _options.Log(handler, exception);

            if (handler == null)
            {
                return Task.CompletedTask;
            }

            var result = handler.Invoke(exception);
            var statusCode = GetStatusCode(handler);

            SetResponse(context, result, statusCode);
            return Task.CompletedTask;
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

        private static void SetResponse(HttpContext context, object? result, HttpStatusCode statusCode)
        {
#pragma warning disable AV2318
            // todo: make this respect the accept header from the client (if possible)
#pragma warning restore AV2318
            var json = JsonConvert.SerializeObject(
                result,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            context.Response.Clear();
            context.Response.Headers.Add("Content-Type", "application/json");
#pragma warning disable CA1305 // specify IFormatProvider
            context.Response.Headers.Add("Content-Length", Encoding.UTF8.GetByteCount(json).ToString());
#pragma warning restore CA1305
            context.Response.StatusCode = (int)statusCode;
            context.Response.WriteAsync(json, Encoding.UTF8);
        }
    }
}