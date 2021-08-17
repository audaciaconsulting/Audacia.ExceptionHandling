using System.Collections.Generic;

namespace Audacia.ExceptionHandling.Results
{
    /// <summary>
    /// Represents a field validation error from a model state dictionary.
    /// </summary>
    public class FieldValidationErrorModel : IHandledError
    {
        /// <summary>
        /// Gets the field name on the model.
        /// </summary>
        public string FieldName { get; }

        /// <summary>
        /// Gets a collection of one or more validation error messages.
        /// </summary>
        public IEnumerable<string> ValidationErrors { get; }

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
    }
}
