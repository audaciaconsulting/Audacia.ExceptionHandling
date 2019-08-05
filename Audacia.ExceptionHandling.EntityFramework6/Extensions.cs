using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using Audacia.ExceptionHandling.Builders;

namespace Audacia.ExceptionHandling.EntityFramework6
{
	public static class Extensions
	{
		public static ExceptionConfigurationBuilder DbEntityValidationException(this ExceptionHandlerBuilder builder)
		{
			return builder.Handle((DbEntityValidationException e) => e.EntityValidationErrors
				.Select((entityValidation, index) => entityValidation.ValidationErrors
				.Select(propertyValidation =>
				{
					var entityType = entityValidation.Entry.Entity.GetType().Name;
					var propertyName = propertyValidation.PropertyName; // .CamelCase(); todo: let asp.net decide whether to camelcase things
					var message = propertyValidation.ErrorMessage;
					return new ErrorResult(HttpStatusCode.BadRequest, message, propertyName)
					{
						{"Type", entityType},
						{"Ordinal", index}
					};
				})).SelectMany(c => c));
		}
	}
}