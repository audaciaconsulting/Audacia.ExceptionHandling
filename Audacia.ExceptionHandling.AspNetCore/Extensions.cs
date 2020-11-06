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
        /// <summary>Configure an <see cref="ExceptionHandlerBuilder"/> for an application.</summary>
        public static IApplicationBuilder ConfigureExceptions(this IApplicationBuilder appBuilder,
            Action<ExceptionHandlerBuilder> action)
        {
            var configBuilder = new ExceptionHandlerBuilder();
            action(configBuilder);
            var filter = new ExceptionFilter(configBuilder);

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