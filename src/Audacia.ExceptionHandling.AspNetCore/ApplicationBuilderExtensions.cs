using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace Audacia.ExceptionHandling.AspNetCore
{
    /// <summary>Extension methods.</summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configure an <see cref="ExceptionHandlerMap"/> for an application.
        /// </summary>
        /// <param name="appBuilder">The app builder that you are configuring.</param>
        /// <param name="loggerFactory">Logger factory, required for attaching customer references to error logs.</param>
        /// <param name="action">The action to configure tha exception handlers.</param>
        /// <returns>The same instance of <see cref="IApplicationBuilder"/> as was passed in but with exception handling configured.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
        public static IApplicationBuilder ConfigureExceptions(
            this IApplicationBuilder appBuilder,
            ILoggerFactory loggerFactory,
            Action<ExceptionHandlerOptionsBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var configBuilder = new ExceptionHandlerOptionsBuilder();
            action(configBuilder);

            appBuilder.UseMiddleware<ExceptionHandlingMiddleware>(loggerFactory, configBuilder.Build());

            return appBuilder;
        }
    }
}