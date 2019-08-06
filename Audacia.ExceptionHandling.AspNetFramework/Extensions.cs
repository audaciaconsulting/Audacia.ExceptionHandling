using System;
using System.Web.Http.Filters;
using System.Web.Mvc;
using Audacia.ExceptionHandling.Builders;

namespace Audacia.ExceptionHandling.AspNetFramework
{
	public static class Extensions
	{
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