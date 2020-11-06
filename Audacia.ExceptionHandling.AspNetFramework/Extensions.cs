using System;
using System.Web.Http.Filters;

namespace Audacia.ExceptionHandling.AspNetFramework
{
	/// <summary>Extension methods.</summary>
	public static class Extensions
	{
		/// <summary>Configure an <see cref="ExceptionHandlerBuilder"/> for an application.</summary>
		public static HttpFilterCollection ConfigureExceptions(this HttpFilterCollection filters,
			Action<ExceptionHandlerBuilder> action)
		{
			var config = new ExceptionHandlerBuilder();
			action(config);
			var filter = new ExceptionFilter(config);

			filters.Add(filter);

			return filters;
		}
	}
}