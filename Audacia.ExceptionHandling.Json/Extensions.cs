using System.Net;
using Audacia.ExceptionHandling.Results;
using Newtonsoft.Json;

namespace Audacia.ExceptionHandling.Json
{
    /// <summary>Extension methods.</summary>
    public static class Extensions
    {
        /// <summary>Configure the default handler for <see cref="Newtonsoft.Json.JsonReaderException"/> with the specified HTTP status code.</summary>
        public static ExceptionHandlerBuilder JsonReaderException(
            this ExceptionHandlerBuilder builder,
            HttpStatusCode statusCode)
        {
            return builder.Add((JsonReaderException e) =>
                new ValidationErrorResult(e.Message, string.Empty, nameof(JsonReaderException), e.Path)
                {
                    ExtraProperties =
                    {
                        ["LineNumber"] = e.LineNumber,
                        ["Position"] = e.LinePosition
                    }
                }, statusCode);
        }

        /// <summary>Configure the default handler for <see cref="Newtonsoft.Json.JsonReaderException"/> with the specified HTTP status code.</summary>
        public static ExceptionHandlerBuilder
            JsonReaderException(this ExceptionHandlerBuilder builder, int statusCode) =>
            builder.JsonReaderException((HttpStatusCode) statusCode);

        /// <summary>Configure the default handler for <see cref="Newtonsoft.Json.JsonReaderException"/> with an HTTP status code of 400: Bad Request.</summary>
        public static ExceptionHandlerBuilder JsonReaderException(this ExceptionHandlerBuilder builder) =>
            builder.JsonReaderException((HttpStatusCode) 400);
    }
}