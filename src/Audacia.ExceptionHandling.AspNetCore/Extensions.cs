using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;

namespace Audacia.ExceptionHandling.AspNetCore
{
    /// <summary>Extension methods.</summary>
    public static class Extensions
    {
        /// <summary>Configure an <see cref="ExceptionHandlerCollection"/> for an application.</summary>
        public static IApplicationBuilder ConfigureExceptions(this IApplicationBuilder appBuilder,
            Action<ExceptionHandlerOptionsBuilder> action)
        {
            var configBuilder = new ExceptionHandlerOptionsBuilder();
            action(configBuilder);
            var filter = new ExceptionFilter(configBuilder.Build());

            appBuilder.UseExceptionHandler(builder =>
            {
                builder.Run(context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    return filter.OnExceptionAsync(exceptionHandlerPathFeature.Error, context);
                });
            });

            return appBuilder;
        }
    }
}