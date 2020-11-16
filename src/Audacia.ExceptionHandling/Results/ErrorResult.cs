using System.Collections.Generic;

namespace Audacia.ExceptionHandling.Results
{
    /// <summary>Represents an API response which contains the details of an error.</summary>
    public class ErrorResult
    {
        /// <summary>
        /// Gets the message to describe the error.
        /// </summary>
        public string Message { get; } = default!;

        /// <summary>
        /// Gets the Error Code.
        /// </summary>
        public string ErrorCode { get; } = default!;

        /// <summary>
        /// Gets the Type of Error.
        /// </summary>
        public string ErrorType { get; } = default!;

        /// <summary>
        /// Gets any extra properties that the user wants to use for this error result without creating a new class.
        /// </summary>
        public Dictionary<string, object> ExtraProperties { get; } = new Dictionary<string, object>();

        /// <summary>Creates a new instance of <see cref="ErrorResult"/>.</summary>
        public ErrorResult() { }

        /// <summary>Create an <see cref="ErrorResult"/> with the specified message.</summary>
        /// <param name="message">The message to give this error.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="errorType">The type of error that happened.</param>
        public ErrorResult(string message, string errorCode, string errorType)
        {
            Message = message;
            ErrorCode = errorCode;
            ErrorType = errorType;
        }
    }
}