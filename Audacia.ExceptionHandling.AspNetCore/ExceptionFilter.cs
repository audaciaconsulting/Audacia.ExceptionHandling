using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Audacia.ExceptionHandling.AspNetCore
{
	/// <summary>Handles exceptions thrown in an ASP.NET Core application based on the configured <see cref="ExceptionHandlerCollection"/>.</summary>
	public class ExceptionFilter
	{
		private readonly ExceptionHandlerCollection _exceptions;

		/// <summary>Create a new instance of <see cref="ExceptionFilter"/>.</summary>
		public ExceptionFilter(ExceptionHandlerCollection exceptions)
		{
			_exceptions = exceptions;
		}

		/// <summary>Handles the specified exception based on the configured <see cref="ExceptionHandlerCollection"/>.</summary>
		public void OnException(Exception exception, HttpContext context)
		{
			if (exception is AggregateException aggregateException)
			{
				exception = aggregateException.Flatten();

				if (exception.InnerException != null)
					exception = exception.InnerException;
			}

			if (!_exceptions.TryGetValue(exception.GetType(), out var handler)) return;
			if (handler == null) return;

			var result = handler.Action.Invoke(exception);

			// todo: make this respect the accept header from the client (if possible)
			var json = JsonConvert.SerializeObject(result, new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});
			context.Response.OnStarting(() => context.Response.WriteAsync(json));
		}
	}
}