using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Audacia.ExceptionHandling.AspNetCore
{
	public class ExceptionFilter
	{
		private readonly ExceptionHandlerCollection _exceptions;

		public ExceptionFilter(ExceptionHandlerCollection exceptions)
		{
			_exceptions = exceptions;
		}
		
		public void OnException(Exception exception, HttpContext context)
		{
			if (exception is AggregateException aggregateException)
			{
				exception = aggregateException.Flatten();

				if (exception.InnerException != null)
					exception = exception.InnerException;
			}

			var assembly = exception.TargetSite.Module.Assembly;
			
			var trace = new StackTrace(exception, true);

			
			if (!_exceptions.TryGetValue(exception.GetType(), out var handler)) return;
			if (handler == null) return;

			var result = handler.Action.Invoke(exception);
			
			// todo: make this respect the accept header from the client (if possible)
			var json = JsonConvert.SerializeObject(result, new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});
			context.Response.WriteAsync(json);
		}
	}
}