using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;

namespace Audacia.ExceptionHandling.AspNetFramework
{
	/// <summary>Handles exceptions and produces standardised error responses from them.</summary>
	public class ExceptionFilter : ExceptionHandlerCollection, IFilter
	{
		public bool AllowMultiple { get; } = false;

		public void OnException(ExceptionContext context)
		{
			var exception = context.Exception;
			if (exception is AggregateException aggregateException)
			{
				exception = aggregateException.Flatten();

				if (exception.InnerException != null)
					exception = exception.InnerException;
			}

			if (!TryGetValue(exception.GetType(), out var handler)) return;
			if (handler == null) return;

			var result = handler.Action.Invoke(context.Exception);
			context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, result);
		}
	}
}