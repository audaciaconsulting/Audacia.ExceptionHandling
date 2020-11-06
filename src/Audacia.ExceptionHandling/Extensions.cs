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
        public static ExceptionHandlerCollection KeyNotFoundException(
            this ExceptionHandlerCollection collection,
            HttpStatusCode statusCode) =>
            collection.Add((KeyNotFoundException e) => new ErrorResult(
                    e.Message,
                    string.Empty,
                    nameof(KeyNotFoundException)),
                statusCode);

        /// <summary>Register the default handler for a <see cref="System.Collections.Generic.KeyNotFoundException"/> with the specified HTTP status code.</summary>
        public static ExceptionHandlerCollection
            KeyNotFoundException(this ExceptionHandlerCollection collection, int statusCode) =>
            collection.KeyNotFoundException((HttpStatusCode) statusCode);

        /// <summary>Register the default handler for a <see cref="System.Collections.Generic.KeyNotFoundException"/>with an HTTP status code of 404: Not Found.</summary>
        public static ExceptionHandlerCollection KeyNotFoundException(this ExceptionHandlerCollection collection) =>
            collection.KeyNotFoundException(HttpStatusCode.NotFound);

        /// <summary>Register the default handler for a <see cref="System.UnauthorizedAccessException"/> with the specified HTTP status code.</summary>
        public static ExceptionHandlerCollection UnauthorizedAccessException(
            this ExceptionHandlerCollection collection,
            HttpStatusCode statusCode) =>
            collection.Add((UnauthorizedAccessException e) => new ErrorResult(), statusCode);

        /// <summary>Register the default handler for a <see cref="System.UnauthorizedAccessException"/> with the specified HTTP status code.</summary>
        public static ExceptionHandlerCollection UnauthorizedAccessException(
            this ExceptionHandlerCollection collection,
            int statusCode) =>
            collection.UnauthorizedAccessException((HttpStatusCode) statusCode);

        /// <summary>Register the default handler for a <see cref="System.UnauthorizedAccessException"/> with an HTTP status code of 401: Unauthorized.</summary>
        public static ExceptionHandlerCollection UnauthorizedAccessException(this ExceptionHandlerCollection collection) =>
            collection.UnauthorizedAccessException(HttpStatusCode.Unauthorized);
    }
}