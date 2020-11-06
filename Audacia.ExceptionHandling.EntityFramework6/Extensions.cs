using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using Audacia.ExceptionHandling.Builders;

namespace Audacia.ExceptionHandling.EntityFramework6
{
    /// <summary>Extension methods.</summary>
    public static class Extensions
    {
        /// <summary>Configure the default handler for <see cref="System.Data.Entity.Validation.DbEntityValidationException"/> with an HTTP status code of 400: Bad Request.</summary>
        public static ExceptionHandlerBuilder DbEntityValidationException(this ExceptionHandlerBuilder builder) =>
            builder.DbEntityValidationException(HttpStatusCode.BadRequest);

        /// <summary>Configure the default handler for <see cref="System.Data.Entity.Validation.DbEntityValidationException"/> with the specified HTTP status code.</summary>
        public static ExceptionHandlerBuilder DbEntityValidationException(this ExceptionHandlerBuilder builder,
            int statusCode) =>
            builder.DbEntityValidationException((HttpStatusCode) statusCode);

        /// <summary>Configure the default handler for <see cref="System.Data.Entity.Validation.DbEntityValidationException"/> with the specified HTTP status code.</summary>
        public static ExceptionHandlerBuilder DbEntityValidationException(this ExceptionHandlerBuilder builder,
            HttpStatusCode statusCode)
        {
            return builder.Add(statusCode, (DbEntityValidationException e) => e.EntityValidationErrors
                .Select((entityValidation, index) => entityValidation.ValidationErrors
                    .Select(propertyValidation =>
                    {
                        var entityType = entityValidation.Entry.Entity.GetType().Name;
                        var propertyName =
                            propertyValidation
                                .PropertyName; // .CamelCase(); todo: let asp.net decide whether to camelcase things
                        var message = propertyValidation.ErrorMessage;
                        return new ErrorResult(message, propertyName)
                        {
                            ExtraProperties =
                            {
                                { "Type", entityType },
                                { "Ordinal", index }
                            }
                        };
                    })).SelectMany(c => c));
        }
    }
}