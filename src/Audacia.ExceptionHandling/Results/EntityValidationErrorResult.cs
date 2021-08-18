using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Audacia.ExceptionHandling.Results
{
    /// <summary>
    /// Represents an EF6 DbEntityValidationException.
    /// </summary>
    public class EntityValidationErrorResult : IHandledError
    {
        /// <summary>
        /// Gets the type of entity.
        /// </summary>
        public string EntityType { get; }

        /// <summary>
        /// Gets a collection of property errors.
        /// </summary>
        public IEnumerable<ValidationErrorResult> PropertyErrors { get; }

        /// <summary>
        /// Creates an instance of <see cref="EntityValidationErrorResult"/>.
        /// </summary>
        /// <param name="entityType">The type of entity.</param>
        /// <param name="propertyErrors">A collection of property errors.</param>
        public EntityValidationErrorResult(string entityType, IEnumerable<ValidationErrorResult> propertyErrors)
        {
            EntityType = entityType;
            PropertyErrors = propertyErrors.ToArray();
        }

        /// <summary>
        /// Returns a message containing the entity type and each validation error.
        /// </summary>
        /// <returns>A message describing the error.</returns>
        public string GetFullMessage()
        {
            var builder = new StringBuilder("EntityType: ");

            foreach (var fieldError in PropertyErrors)
            {
                builder.AppendLine();
                builder.Append("  Property: ");
                builder.Append(fieldError.Property);

                foreach (var message in fieldError.ValidationErrors)
                {
                    builder.AppendLine();
                    builder.Append("    - ");
                    builder.Append(message);
                } 
            }

            return builder.ToString();
        }
    }
}
