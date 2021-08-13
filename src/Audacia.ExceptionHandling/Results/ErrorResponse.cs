using System.Collections.Generic;

namespace Audacia.ExceptionHandling.Results
{
    /// <summary>
    /// Represents an error that can be returned from the API.
    /// </summary>
    public interface IError { }

    /// <summary>
    /// The response returned to the user after an exception has been handled.
    /// </summary>
    public sealed class ErrorResponse
    {
        /// <summary>
        /// Gets the reference displayed to the user, this is also logged to Application Insights.
        /// </summary>
        public string CustomerReference { get; }

        /// <summary>
        /// Gets the type of error that was handled.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets a collection of one or more errors that have occurred.
        /// </summary>
        public IEnumerable<IError> Errors { get; }

        /// <summary>
        /// Creates an instance of <see cref="ErrorResponse"/>.
        /// </summary>
        /// <param name="customerReference">The reference displayed to the user.</param>
        /// <param name="errorType">The type of error that was handled.</param>
        /// <param name="errors">A collection of one or more errors that have occurred.</param>
        public ErrorResponse(string customerReference, string errorType, IEnumerable<IError> errors)
        {
            CustomerReference = customerReference;
            Type = errorType;
            Errors = errors;
        }
    }

    /// <summary>
    /// Represents a single issue from an aggregate exception.
    /// </summary>
    public class ErrorModel : IError
    {
        /// <summary>
        /// Gets the unique code to identify the error.
        /// </summary>
        public string Code { get; } = default!;

        /// <summary>
        /// Gets the message to describe the error.
        /// </summary>
        public string Message { get; } = default!;

        /// <summary>
        /// Gets any extra properties that the user wants to use for this error result without creating a new class.
        /// </summary>
        public Dictionary<string, object> ExtraProperties { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Creates an instance of <see cref="ErrorModel"/>.
        /// </summary>
        /// <param name="errorCode">The unique code to identify the error.</param>
        /// <param name="message">The message to describe the error.</param>
        public ErrorModel(string errorCode, string message)
        {
            Code = errorCode;
            Message = message;
        }

        /// <summary>
        /// Creates an instance of <see cref="ErrorModel"/>.
        /// </summary>
        /// <param name="errorCode">The unique code to identify the error.</param>
        /// <param name="message">The message to describe the error.</param>
        /// <param name="extraProperties">Any extra properties to be returned to the user.</param>
        public ErrorModel(string errorCode, string message, Dictionary<string, object> extraProperties)
        {
            Code = errorCode;
            Message = message;
            ExtraProperties = extraProperties;
        }
    }

    /// <summary>
    /// Represents a field validation error from a model state dictionary.
    /// </summary>
    public sealed class FieldValidationErrorModel : IError
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
