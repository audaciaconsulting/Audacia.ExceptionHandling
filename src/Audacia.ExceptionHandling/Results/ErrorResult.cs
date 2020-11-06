using System.Collections.Generic;

namespace Audacia.ExceptionHandling.Results
{
    /// <summary>Represents an API response which contains the details of an error.</summary>
    public class ErrorResult
    {
        /// <summary>
        /// The message to describe the error.
        /// </summary>
        public string Message { get; } = default!;

        /// <summary>
        /// The Error Code.
        /// </summary>
        public string ErrorCode { get; } = default!;

        /// <summary>
        /// The Type of Error.
        /// </summary>
        public string ErrorType { get; } = default!;

        /// <summary>
        /// Any extra properties that the user wants to use for this error result without creating a new class.
        /// </summary>
        public Dictionary<string, object> ExtraProperties { get; } = new Dictionary<string, object>();

        /// <summary>Creates a new instance of <see cref="ErrorResult"/>.</summary>
        public ErrorResult() { }

        /// <summary>Create an <see cref="ErrorResult"/> with the specified message.</summary>
        public ErrorResult(string message, string errorCode, string errorType)
        {
            Message = message;
            ErrorCode = errorCode;
            ErrorType = errorType;
        }
    }
}