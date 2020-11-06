namespace Audacia.ExceptionHandling
{
    /// <summary>
    /// A class that is used for describing validation errors.
    /// Adding a new property of <see cref="ValidationErrorResult.Property"/> to the existing
    /// <see cref="ErrorResult"/> class.
    /// </summary>
    public class ValidationErrorResult : ErrorResult
    {
        /// <summary>
        /// The property that the error is for.
        /// </summary>
        public string? Property { get; }

        /// <summary>Create an error result with the specified message for the specified property.</summary>
        public ValidationErrorResult(string message, string property) : base(message)
        {
            Property = property;
        }
    }
}