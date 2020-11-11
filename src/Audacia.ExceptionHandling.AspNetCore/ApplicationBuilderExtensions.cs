using System;
using Microsoft.AspNetCore.Builder;

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

            appBuilder.UseExceptionHandler(new Microsoft.AspNetCore.Builder.ExceptionHandlerOptions
            {
                ExceptionHandler = new ExceptionHandlingMiddleware(configBuilder.Build()).Invoke
            });

            return appBuilder;
        }
    }
}