using System;
using System.Threading.Tasks;
using Audacia.ExceptionHandling.Builders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;

namespace Audacia.ExceptionHandling.AspNetCore
{
	public static class Extensions
	{
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
					filter.OnException(exceptionHandlerPathFeature.Error, context);
					return Task.CompletedTask;
				});
			});
			
			return appBuilder;
		}
	}
}