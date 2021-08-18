using System;
using Audacia.ExceptionHandling.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace Audacia.ExceptionHandling.AspNetCore
{
    /// <summary>Extension methods.</summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configures an <see cref="IExceptionHandler"/> for each domain exception type in the application.
        /// </summary>
        /// <param name="appBuilder">The app builder that you are configuring.</param>
        /// <param name="configureAction">The action to configure tha exception handlers.</param>
        /// <param name="loggerFactory">Logger factory, required for attaching customer references to error logs.</param>
        /// <returns>The same instance of <see cref="IApplicationBuilder"/> as was passed in but with exception handling configured.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="configureAction"/> is <see langword="null"/>.</exception>
        public static IApplicationBuilder ConfigureExceptions(
            this IApplicationBuilder appBuilder,
            Action<ExceptionHandlerOptionsBuilder> configureAction,
            ILoggerFactory loggerFactory)
        {
            if (configureAction == null)
            {
                throw new ArgumentNullException(nameof(configureAction));
            }

            var builder = new ExceptionHandlerOptionsBuilder();
            configureAction(builder);

            appBuilder.UseMiddleware<ExceptionHandlingMiddleware>(loggerFactory, builder.Build());

            return appBuilder;
        }
    }
}