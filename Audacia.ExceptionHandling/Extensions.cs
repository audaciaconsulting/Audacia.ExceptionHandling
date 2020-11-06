using System;
using System.Collections.Generic;
using System.Net;
using Audacia.ExceptionHandling.Results;

namespace Audacia.ExceptionHandling
{
    /// <summary>Extension methods.</summary>
    public static class Extensions
    {
        /// <summary>Register the default handler for a <see cref="System.Collections.Generic.KeyNotFoundException"/> with the specified HTTP status code.</summary>
        public static ExceptionHandlerBuilder KeyNotFoundException(
            this ExceptionHandlerBuilder builder,
            HttpStatusCode statusCode) =>
            builder.Add((KeyNotFoundException e) => new ErrorResult(
                    e.Message,
                    string.Empty,
                    nameof(KeyNotFoundException)),
                statusCode);

        /// <summary>Register the default handler for a <see cref="System.Collections.Generic.KeyNotFoundException"/> with the specified HTTP status code.</summary>
        public static ExceptionHandlerBuilder
            KeyNotFoundException(this ExceptionHandlerBuilder builder, int statusCode) =>
            builder.KeyNotFoundException((HttpStatusCode) statusCode);

        /// <summary>Register the default handler for a <see cref="System.Collections.Generic.KeyNotFoundException"/>with an HTTP status code of 404: Not Found.</summary>
        public static ExceptionHandlerBuilder KeyNotFoundException(this ExceptionHandlerBuilder builder) =>
            builder.KeyNotFoundException(HttpStatusCode.NotFound);

        /// <summary>Register the default handler for a <see cref="System.UnauthorizedAccessException"/> with the specified HTTP status code.</summary>
        public static ExceptionHandlerBuilder UnauthorizedAccessException(
            this ExceptionHandlerBuilder builder,
            HttpStatusCode statusCode) =>
            builder.Add((UnauthorizedAccessException e) => new ErrorResult(), statusCode);

        /// <summary>Register the default handler for a <see cref="System.UnauthorizedAccessException"/> with the specified HTTP status code.</summary>
        public static ExceptionHandlerBuilder UnauthorizedAccessException(
            this ExceptionHandlerBuilder builder,
            int statusCode) =>
            builder.UnauthorizedAccessException((HttpStatusCode) statusCode);

        /// <summary>Register the default handler for a <see cref="System.UnauthorizedAccessException"/> with an HTTP status code of 401: Unauthorized.</summary>
        public static ExceptionHandlerBuilder UnauthorizedAccessException(this ExceptionHandlerBuilder builder) =>
            builder.UnauthorizedAccessException(HttpStatusCode.Unauthorized);
    }
}