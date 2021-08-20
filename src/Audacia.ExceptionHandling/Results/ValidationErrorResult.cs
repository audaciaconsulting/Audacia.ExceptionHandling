using System.Collections.Generic;
using System.Linq;

namespace Audacia.ExceptionHandling.Results
{
    /// <summary>
    /// Represents a field validation error from a model state dictionary.
    /// </summary>
    public sealed class ValidationErrorResult : IHandledError
    {
        /// <summary>
        /// Gets the property name on the model.
        /// </summary>
        public string Property { get; } = default!;

        /// <summary>
        /// Gets an enumerable of one or more validation error messages.
        /// </summary>
        public IEnumerable<string> ValidationErrors { get; } = Enumerable.Empty<string>();

        /// <summary>
        /// Creates an instance of <see cref="ValidationErrorResult"/>.
        /// </summary>
        public ValidationErrorResult() { }

        /// <summary>
        /// Creates an instance of <see cref="ValidationErrorResult"/>.
        /// </summary>
        /// <param name="propertyName">The property name on the model.</param>
        /// <param name="validationErrors">A collection of one or more validation error messages.</param>
        public ValidationErrorResult(string propertyName, params string[] validationErrors)
        {
            Property = propertyName;
            ValidationErrors = validationErrors;
        }

        /// <summary>
        /// Creates an instance of <see cref="ValidationErrorResult"/>.
        /// </summary>
        /// <param name="propertyName">The property name on the model.</param>
        /// <param name="validationErrors">An enumerable of one or more validation error messages.</param>
        public ValidationErrorResult(string propertyName, IEnumerable<string> validationErrors)
        {
            Property = propertyName;
            ValidationErrors = validationErrors.ToArray();
        }

        /// <summary>
        /// Returns the <see cref="Property"/> and all <see cref="ValidationErrors"/> as a singlular string.
        /// </summary>
        /// <returns>A message describing the error.</returns>
        public string GetFullMessage()
        {
            return $"{Property}: [ {string.Join(", ", ValidationErrors)} ].";
        }
    }
}
