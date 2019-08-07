using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using Audacia.ExceptionHandling.Builders;

namespace Audacia.ExceptionHandling.Annotations
{
	/// <summary>Extension methods.</summary>
	public static class Extensions
	{
		/// <summary>Configure the default handler for <see cref="ValidationException"/>.</summary>
		public static ExceptionHandlerCollectionBuilder ValidationException(this ExceptionHandlerBuilder builder) =>
			builder.Handle((ValidationException e) => e.ValidationResult.MemberNames
				.Select(member => new ErrorResult(HttpStatusCode.BadRequest, e.ValidationResult.ErrorMessage, member)));
	}
}