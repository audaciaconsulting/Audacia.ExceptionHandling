using System;
using System.Collections.Generic;

namespace Audacia.ExceptionHandling.Results
{
    /// <summary>
    /// Represents a single issue from an aggregate of errors.
    /// </summary>
    public class ErrorResult : IHandledError
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
        /// Creates an instance of <see cref="ErrorResult"/>.
        /// </summary>
        public ErrorResult() { }

        /// <summary>
        /// Creates an instance of <see cref="ErrorResult"/>.
        /// </summary>
        /// <param name="errorCode">The unique code to identify the error.</param>
        /// <param name="message">The message to describe the error.</param>
        public ErrorResult(string errorCode, string message)
        {
            Code = errorCode;
            Message = message;
        }

        /// <summary>
        /// Creates an instance of <see cref="ErrorResult"/>.
        /// </summary>
        /// <param name="errorCode">The unique code to identify the error.</param>
        /// <param name="message">The message to describe the error.</param>
        /// <param name="extraProperties">Any extra properties to be returned to the user.</param>
        public ErrorResult(string errorCode, string message, Dictionary<string, object> extraProperties)
        {
            Code = errorCode;
            Message = message;
            ExtraProperties = extraProperties;
        }

        /// <summary>
        /// Returns the <see cref="Code"/> and <see cref="Message"/> as a singlular string.
        /// </summary>
        /// <returns>A message describing the error.</returns>
        public string GetFullMessage()
        {
            return $"{Code}: {Message}";
        }

        /// <summary>
        /// Creates an instance of <see cref="ErrorResult"/> from any <see cref="Exception"/>.
        /// </summary>
        /// <param name="exception">Exception data.</param>
        /// <returns>An instance of <see cref="ErrorResult"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="exception"/> is <see langword="null"/>.</exception>
        public static ErrorResult FromException(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            return new ErrorResult(exception.GetType().Name, exception.Message);
        }
    }
}