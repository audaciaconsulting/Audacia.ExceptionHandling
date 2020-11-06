using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Audacia.ExceptionHandling.Builders;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Audacia.ExceptionHandling.AspNetCore
{
    /// <summary>Handles exceptions thrown in an ASP.NET Core application based on the configured <see cref="ExceptionHandlerBuilder"/>.</summary>
    public class ExceptionFilter
    {
        private readonly ExceptionHandlerBuilder _exceptions;

        /// <summary>Create a new instance of <see cref="ExceptionFilter"/>.</summary>
        public ExceptionFilter(ExceptionHandlerBuilder exceptions)
        {
            _exceptions = exceptions;
        }

        /// <summary>Handles the specified exception based on the configured <see cref="ExceptionHandlerBuilder"/>.</summary>
        public Task OnExceptionAsync(Exception exception, HttpContext context)
        {
            if (exception is AggregateException aggregateException)
            {
                exception = aggregateException.Flatten();

                if (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
            }

            var handler = _exceptions.Get(exception);

            if (handler == null)
            {
                return Task.CompletedTask;
            }

            var result = handler.Action.Invoke(exception);

            var statusCode = HttpStatusCode.BadRequest;

            if (handler is HttpExceptionHandler<Exception, object> httpExceptionHandler)
            {
                statusCode = httpExceptionHandler.StatusCode;
            }

            // todo: make this respect the accept header from the client (if possible)
            var json = JsonConvert.SerializeObject(result,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            context.Response.Clear();
            context.Response.Headers.Add("Content-Type", "application/json");
            context.Response.Headers.Add("Content-Length", Encoding.UTF8.GetByteCount(json).ToString());
            context.Response.StatusCode = (int) statusCode;
            context.Response.WriteAsync(json, Encoding.UTF8);
            return Task.CompletedTask;
        }
    }
}