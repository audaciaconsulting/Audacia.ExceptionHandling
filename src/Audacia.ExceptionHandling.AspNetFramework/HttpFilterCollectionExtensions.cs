using System;
using System.Web.Http.Filters;

namespace Audacia.ExceptionHandling.AspNetFramework
{
    /// <summary>Extension methods.</summary>
    public static class HttpFilterCollectionExtensions
    {
        /// <summary>Configure an <see cref="ExceptionHandlerCollection"/> for an application.</summary>
        public static HttpFilterCollection ConfigureExceptions(
            this HttpFilterCollection filters,
            Action<ExceptionHandlerOptionsBuilder> action)
        {
            var config = new ExceptionHandlerOptionsBuilder();
            action(config);
            var filter = new ExceptionFilter(config.Build());

            filters.Add(filter);

            return filters;
        }
    }
}