using System.Linq;
using System.Net;
using Audacia.ExceptionHandling.Builders;
using FluentValidation;
namespace Audacia.ExceptionHandling.FluentValidation
{
	public static class Extensions
	{
		public static ExceptionHandlerCollectionBuilder FluentValidationException(this ExceptionHandlerBuilder builder) =>
			builder.Handle((ValidationException e) => e.Errors
				.Select(member => new ErrorResult(HttpStatusCode.BadRequest, member.ErrorMessage, member.PropertyName)));
	}
}