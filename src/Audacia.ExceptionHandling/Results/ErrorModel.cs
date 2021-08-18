using System.Collections.Generic;

namespace Audacia.ExceptionHandling.Results
{
    /// <summary>
    /// Represents a single issue from an aggregate of errors.
    /// </summary>
    public class ErrorModel : IHandledError
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
        public IDictionary<string, object> ExtraProperties { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Creates an instance of <see cref="ErrorModel"/>.
        /// </summary>
        public ErrorModel() { }

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
}
