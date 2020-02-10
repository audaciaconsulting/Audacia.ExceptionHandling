using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using Audacia.ExceptionHandling.Builders;

namespace Audacia.ExceptionHandling.Annotations
{
	/// <summary>Extension methods.</summary>
	public static class Extensions
	{
		/// <summary>Configure the default handler for <see cref="System.ComponentModel.DataAnnotations.ValidationException"/> with the specified http status code.</summary>
		public static ExceptionHandlerCollectionBuilder ValidationException(this ExceptionHandlerBuilder builder, HttpStatusCode statusCode) =>
			builder.Handle(statusCode, (ValidationException e) => e.ValidationResult.MemberNames
				.Select(member => new ErrorResult(e.ValidationResult.ErrorMessage, member)));

		/// <summary>Configure the default handler for <see cref="System.ComponentModel.DataAnnotations.ValidationException"/> with the specified http status code.</summary>
		public static ExceptionHandlerCollectionBuilder
			ValidationException(this ExceptionHandlerBuilder builder, int statusCode) =>
			builder.ValidationException((HttpStatusCode) statusCode);

		/// <summary>Configure the default handler for <see cref="System.ComponentModel.DataAnnotations.ValidationException"/> with an HTTP status code of 422: Unprocessable Entity.</summary>
		public static ExceptionHandlerCollectionBuilder ValidationException(this ExceptionHandlerBuilder builder) =>
			builder.ValidationException((HttpStatusCode) 422);
	}
}