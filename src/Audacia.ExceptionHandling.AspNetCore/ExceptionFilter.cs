using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Audacia.ExceptionHandling.Handlers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Audacia.ExceptionHandling.AspNetCore
{
    /// <summary>Handles exceptions thrown in an ASP.NET Core application based on the configured <see cref="ExceptionHandlerMap"/>.</summary>
    public class ExceptionFilter
    {
        private readonly ExceptionHandlerOptions _options;

        /// <summary>Create a new instance of <see cref="ExceptionFilter"/>.</summary>
        public ExceptionFilter(ExceptionHandlerOptions options)
        {
            _options = options;
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
            context.Response.Headers.Add("Content-Length", Encoding.UTF8.GetByteCount(json).ToString());
            context.Response.StatusCode = (int)statusCode;
            context.Response.WriteAsync(json, Encoding.UTF8);
        }

        /// <summary>Handles the specified exception based on the configured <see cref="ExceptionHandlerMap"/>.</summary>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is <see langword="null"/>.</exception>
        public Task OnExceptionAsync(Exception exception, HttpContext context)
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
    }
}