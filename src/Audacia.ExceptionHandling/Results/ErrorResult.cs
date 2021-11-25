using System;
using System.Collections.Generic;
using System.Linq;

namespace Audacia.ExceptionHandling.Results
{
    /// <summary>
    /// Represents a single issue from an aggregate of errors.
    /// </summary>
    public sealed class ErrorResult
    {
        /// <summary>
        /// Gets the unique code to identify the error.
        /// </summary>
        public string Code { get; } = default!;

        /// <summary>
        /// Gets the messages to describe the error.
        /// </summary>
        public IEnumerable<string> Messages { get; } = Enumerable.Empty<string>();

        /// <summary>
        /// Creates an instance of <see cref="ErrorResult"/>.
        /// </summary>
        public ErrorResult() { }

        /// <summary>
        /// Creates an instance of <see cref="ErrorResult"/>.
        /// </summary>
        /// <param name="errorCode">The unique code to identify the error.</param>
        /// <param name="messages">One or more messages to describe the error.</param>
        public ErrorResult(string errorCode, params string[] messages)
        {
            Code = errorCode;
            Messages = messages;
        }

        /// <summary>
        /// Creates an instance of <see cref="ErrorResult"/>.
        /// </summary>
        /// <param name="errorCode">The unique code to identify the error.</param>
        /// <param name="messages">A collection of messages to describe the error.</param>
        public ErrorResult(string errorCode, IEnumerable<string> messages)
        {
            Code = errorCode;
            Messages = messages.ToArray();
        }

        /// <summary>
        /// Returns the <see cref="Code"/> and <see cref="Messages"/> as a singlular string.
        /// </summary>
        /// <returns>A message describing the error.</returns>
        public string GetFullMessage()
        {
            if (Messages.Count() == 1) 
            {
                return $"{Code}: {Messages.First()}.";
            }

            return $"{Code}: [ {string.Join(", ", Messages)} ].";
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