using System.Collections.Generic;
using System.Linq;

namespace Audacia.ExceptionHandling.Results
{
    /// <summary>
    /// Represents a field validation error from a model state dictionary.
    /// </summary>
    public sealed class FieldValidationErrorModel : IHandledError
    {
        /// <summary>
        /// Gets the field name on the model.
        /// </summary>
        public string FieldName { get; } = default!;

        /// <summary>
        /// Gets an enumerable of one or more validation error messages.
        /// </summary>
        public IEnumerable<string> ValidationErrors { get; } = Enumerable.Empty<string>();

        /// <summary>
        /// Creates an instance of <see cref="FieldValidationErrorModel"/>.
        /// </summary>
        public FieldValidationErrorModel() { }

        /// <summary>
        /// Creates an instance of <see cref="FieldValidationErrorModel"/>.
        /// </summary>
        /// <param name="fieldName">The field name on the model.</param>
        /// <param name="validationErrors">A collection of one or more validation error messages.</param>
        public FieldValidationErrorModel(string fieldName, params string[] validationErrors)
        {
            FieldName = fieldName;
            ValidationErrors = validationErrors;
        }

        /// <summary>
        /// Creates an instance of <see cref="FieldValidationErrorModel"/>.
        /// </summary>
        /// <param name="fieldName">The field name on the model.</param>
        /// <param name="validationErrors">An enumerable of one or more validation error messages.</param>
        public FieldValidationErrorModel(string fieldName, IEnumerable<string> validationErrors)
        {
            FieldName = fieldName;
            ValidationErrors = validationErrors;
        }
    }
}
