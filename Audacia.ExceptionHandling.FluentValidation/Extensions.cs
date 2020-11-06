using System.Linq;
using System.Net;
using Audacia.ExceptionHandling.Builders;
using FluentValidation;
namespace Audacia.ExceptionHandling.FluentValidation
{
	/// <summary>Extension methods.</summary>
	public static class Extensions
	{
		/// <summary>Configure the default handler for FluentValidation's <see cref="ValidationException"/> with the specified HTTP status code.</summary>
		public static ExceptionHandlerBuilder FluentValidationException(this ExceptionHandlerBuilder builder, HttpStatusCode statusCode) =>
			builder.Add(statusCode, (ValidationException e) => e.Errors
				.Select(member => new ErrorResult(member.ErrorMessage, member.PropertyName)));

		/// <summary>Configure the default handler for <see cref="System.ComponentModel.DataAnnotations.ValidationException"/> with the specified HTTP status code.</summary>
		public static ExceptionHandlerBuilder
			FluentValidationException(this ExceptionHandlerBuilder builder, int statusCode) =>
			builder.FluentValidationException((HttpStatusCode) statusCode);

		/// <summary>Configure the default handler for <see cref="System.ComponentModel.DataAnnotations.ValidationException"/> with an HTTP status code of 422: Unprocessable Entity.</summary>
		public static ExceptionHandlerBuilder FluentValidationException(this ExceptionHandlerBuilder builder) =>
			builder.FluentValidationException((HttpStatusCode) 422);
	}
}