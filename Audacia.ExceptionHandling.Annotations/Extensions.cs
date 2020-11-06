using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using Audacia.ExceptionHandling.Results;

namespace Audacia.ExceptionHandling.Annotations
{
    /// <summary>Extension methods.</summary>
    public static class Extensions
    {
        /// <summary>Configure the default handler for <see cref="System.ComponentModel.DataAnnotations.ValidationException"/> with the specified http status code.</summary>
        public static ExceptionHandlerBuilder ValidationException(
            this ExceptionHandlerBuilder builder,
            HttpStatusCode statusCode) =>
            builder.Add((ValidationException e) => e.ValidationResult.MemberNames
                    .Select(member => new ValidationErrorResult(e.ValidationResult.ErrorMessage,
                        string.Empty,
                        nameof(ValidationException),
                        member)),
                statusCode);

        /// <summary>Configure the default handler for <see cref="System.ComponentModel.DataAnnotations.ValidationException"/> with the specified http status code.</summary>
        public static ExceptionHandlerBuilder
            ValidationException(this ExceptionHandlerBuilder builder, int statusCode) =>
            builder.ValidationException((HttpStatusCode) statusCode);

        /// <summary>Configure the default handler for <see cref="System.ComponentModel.DataAnnotations.ValidationException"/> with an HTTP status code of 422: Unprocessable Entity.</summary>
        public static ExceptionHandlerBuilder ValidationException(this ExceptionHandlerBuilder builder) =>
            builder.ValidationException((HttpStatusCode) 422);
    }
}