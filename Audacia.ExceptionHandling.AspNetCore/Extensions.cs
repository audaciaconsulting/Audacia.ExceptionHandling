using System;
using Audacia.ExceptionHandling.Builders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;

namespace Audacia.ExceptionHandling.AspNetCore
{
	public static class Extensions
	{
		public static IApplicationBuilder ConfigureExceptions(this IApplicationBuilder appBuilder, Action<ExceptionFilter> action)
		{
			var configBuilder = new ExceptionFilter();
			action(configBuilder);

			appBuilder.UseExceptionHandler(builder =>
			{
				builder.Run(async context =>
				{
					var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
					configBuilder.OnException(exceptionHandlerPathFeature.Error, context);
				});
			});
			
			return appBuilder;
		}
	}
}