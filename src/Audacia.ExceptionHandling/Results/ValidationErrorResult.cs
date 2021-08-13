namespace Audacia.ExceptionHandling.Results
{
    /// <summary>
    /// A class that is used for describing validation errors.
    /// Adding a new property of <see cref="ValidationErrorResult.Property"/> to the existing
    /// <see cref="ErrorResult"/> class.
    /// </summary>
    public class ValidationErrorResult : ErrorResult
    {
        /// <summary>
        /// Gets the property that the error is for.
        /// </summary>
        public string? Property { get; }

        /// <summary>Create an error result with the specified message for the specified property.</summary>
        /// <param name="customerReference">An error code to be displayed to the customer that has been attached to related the Application Insights log.</param>
        /// <param name="message">The message to give this error.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="errorType">The type of error that happened.</param>
        /// <param name="property">The property the validation error is for.</param>
        public ValidationErrorResult(string customerReference, string message, string errorCode, string errorType, string property)
            : base(customerReference, message, errorCode, errorType)
        {
            Property = property;
        }
    }
}