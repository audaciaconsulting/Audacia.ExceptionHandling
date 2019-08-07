using System;
using System.Web.Http.Filters;
using Audacia.ExceptionHandling.Builders;

namespace Audacia.ExceptionHandling.AspNetFramework
{
	/// <summary>Extension methods.</summary>
	public static class Extensions
	{
		/// <summary>Configure an <see cref="ExceptionHandlerCollection"/> for an application.</summary>
		public static HttpFilterCollection ConfigureExceptions(this HttpFilterCollection filters,
			Action<ExceptionHandlerCollectionBuilder> action)
		{
			var configBuilder = new ExceptionHandlerCollectionBuilder();
			action(configBuilder);
			var config = configBuilder.ExceptionHandlerCollection;
			var filter = new ExceptionFilter(config);

			filters.Add(filter);

			return filters;
		}
	}
}