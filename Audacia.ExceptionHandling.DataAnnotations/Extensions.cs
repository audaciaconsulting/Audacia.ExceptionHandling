using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using Audacia.ExceptionHandling.Builders;

namespace Audacia.ExceptionHandling.DataAnnotations
{
	public static class Extensions
	{
		public static ExceptionConfigurationBuilder ValidationException(this ExceptionHandlerBuilder builder) =>
			builder.Handle((ValidationException e) => e.ValidationResult.MemberNames
				.Select(member => new ErrorResult(HttpStatusCode.BadRequest, e.ValidationResult.ErrorMessage, member)));
	}
}