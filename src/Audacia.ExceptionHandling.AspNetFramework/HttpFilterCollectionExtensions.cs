using System;
using System.Collections.Generic;
using System.Web.Http.Filters;

namespace Audacia.ExceptionHandling.AspNetFramework
{
    /// <summary>Extension methods.</summary>
    public static class HttpFilterCollectionExtensions
    {
        /// <summary>Configure an <see cref="ExceptionHandlerMap"/> for an application.</summary>
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