using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;

namespace Audacia.ExceptionHandling.AspNetCore
{
    /// <summary>Extension methods.</summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>Configure an <see cref="ExceptionHandlerMap"/> for an application.</summary>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
        public static IApplicationBuilder ConfigureExceptions(
            this IApplicationBuilder appBuilder,
            Action<ExceptionHandlerOptionsBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

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