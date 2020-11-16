using System;
using System.Collections.Generic;
using System.Web.Http.Filters;

namespace Audacia.ExceptionHandling.AspNetFramework
{
    /// <summary>Extension methods.</summary>
    public static class HttpFilterCollectionExtensions
    {
        /// <summary>Configure an <see cref="ExceptionHandlerMap"/> for an application.</summary>
        /// <param name="filters">The collection of HttpFilters.</param>
        /// <param name="action">The action that is used to configure the exception handling.</param>
        /// <returns>The list of filters after you have added the exception handling.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
        public static IEnumerable<FilterInfo> ConfigureExceptions(
            this HttpFilterCollection filters,
            Action<ExceptionHandlerOptionsBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var config = new ExceptionHandlerOptionsBuilder();
            action(config);
            var filter = new ExceptionFilter(config.Build());

            filters.Add(filter);

            return filters;
        }
    }
}