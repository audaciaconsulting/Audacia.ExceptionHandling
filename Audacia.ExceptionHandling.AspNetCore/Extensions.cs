using System;
using System.Threading.Tasks;
using Audacia.ExceptionHandling.Builders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;

namespace Audacia.ExceptionHandling.AspNetCore
{
	/// <summary>Extension methods.</summary>
	public static class Extensions
	{
		/// <summary>Configure an <see cref="ExceptionHandlerCollection"/> for an application.</summary>
		public static IApplicationBuilder ConfigureExceptions(this IApplicationBuilder appBuilder, Action<ExceptionHandlerCollectionBuilder> action)
		{
			var configBuilder = new ExceptionHandlerCollectionBuilder();
			action(configBuilder);
			var config = configBuilder.ExceptionHandlerCollection;
			var filter = new ExceptionFilter(config);

			appBuilder.UseExceptionHandler(builder =>
			{
				builder.Run(context =>
				{
					var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
					return filter.OnException(exceptionHandlerPathFeature.Error, context);
				});
			});

			return appBuilder;
		}
	}
}