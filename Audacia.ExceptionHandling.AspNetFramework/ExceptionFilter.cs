using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;

namespace Audacia.ExceptionHandling.AspNetFramework
{
	/// <summary>Handles exceptions and produces standardised error responses from them.</summary>
	public class ExceptionFilter : IExceptionFilter
	{
		private readonly ExceptionHandlerCollection _handlers;

		public ExceptionFilter(ExceptionHandlerCollection handlers)
		{
			_handlers = handlers;
		}

		public bool AllowMultiple { get; } = false;
		public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext context, CancellationToken cancellationToken)
		{
			var exception = context.Exception;
			if (exception is AggregateException aggregateException)
			{
				exception = aggregateException.Flatten();

				if (exception.InnerException != null)
					exception = exception.InnerException;
			}

			if (!_handlers.TryGetValue(exception.GetType(), out var handler)) return Task.CompletedTask;
			if (handler == null) return Task.CompletedTask;

			var result = handler.Action.Invoke(context.Exception);
			context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, result);
			
			return Task.CompletedTask;
		}
	}
}