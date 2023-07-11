using System;
using System.Collections.Generic;
using System.Web.Http.Filters;
using Audacia.ExceptionHandling.Handlers;
using Microsoft.Extensions.Logging;

namespace Audacia.ExceptionHandling.AspNetFramework
{
    /// <summary>Extension methods.</summary>
    public static class HttpFilterCollectionExtensions
    {
        /// <summary>Configures an <see cref="IExceptionHandler"/> for each domain exception type in the application.</summary>
        /// <param name="filters">The collection of HttpFilters.</param>
        /// <param name="loggerFactory">Logger factory, required for attaching customer references to error logs.</param>
        /// <param name="configureAction">The action that is used to configure the exception handling.</param>
        /// <returns>The list of filters after you have added the exception handling.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="configureAction"/> is <see langword="null"/>.</exception>
        public static IEnumerable<FilterInfo> ConfigureExceptions(
            this HttpFilterCollection filters,
            ILoggerFactory loggerFactory,
            Action<ExceptionHandlerOptionsBuilder> configureAction)
        {
            if (filters == null)
            {
                throw new ArgumentNullException(nameof(filters));
            }

            if (configureAction == null)
            {
                throw new ArgumentNullException(nameof(configureAction));
            }

            var builder = new ExceptionHandlerOptionsBuilder();
            configureAction(builder);

            var exceptionHandler = builder.Build();

            var filter = new ExceptionFilter(loggerFactory, exceptionHandler);
            filters.Add(filter);

            return filters;
        }
    }
}