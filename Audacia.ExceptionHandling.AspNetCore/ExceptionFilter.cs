using System;
using System.Net;
using Audacia.ExceptionHandling.Builders;
using Microsoft.AspNetCore.Http;

namespace Audacia.ExceptionHandling.AspNetCore
{
	public class ExceptionFilter : ExceptionHandlerCollection
	{
		public void OnException(Exception exception, HttpContext context)
		{
			if (exception is AggregateException aggregateException)
			{
				exception = aggregateException.Flatten();

				if (exception.InnerException != null)
					exception = exception.InnerException;
			}

			if (!TryGetValue(exception.GetType(), out var handler)) return;
			if (handler == null) return;

			var result = handler.Action.Invoke(exception);
			
			context.Request.CreateResponse(HttpStatusCode.BadRequest, result);
		}
		
		public ExceptionHandlerBuilder Handle() => new ExceptionHandlerBuilder(this);
	}
}