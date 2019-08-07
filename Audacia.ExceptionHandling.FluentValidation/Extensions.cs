using System.Linq;
using System.Net;
using Audacia.ExceptionHandling.Builders;
using FluentValidation;
namespace Audacia.ExceptionHandling.FluentValidation
{
	/// <summary>Extension methods.</summary>
	public static class Extensions
	{
		/// <summary>Configure the default handler for FluentValidation's <see cref="ValidationException"/>.</summary>
		public static ExceptionHandlerCollectionBuilder FluentValidationException(this ExceptionHandlerBuilder builder) =>
			builder.Handle((ValidationException e) => e.Errors
				.Select(member => new ErrorResult(HttpStatusCode.BadRequest, member.ErrorMessage, member.PropertyName)));
	}
}