using System;
using System.Collections.Generic;
using System.Linq;

namespace Audacia.ExceptionHandling.Results
{
    /// <summary>
    /// The response returned to the user after an exception has been handled.
    /// </summary>
    public sealed class ErrorResponse
    {
        private const string UnexpectedErrorType = "Unexpected Server Error";
        private const string UnexpectedErrorMessage = "An unexpected error has occurred, please contact a system administrator.";

        /// <summary>
        /// Gets the reference displayed to the user, this is also logged to Application Insights.
        /// </summary>
        public string Reference { get; } = default!;

        /// <summary>
        /// Gets the type of error that was handled.
        /// </summary>
        public string Type { get; } = default!;

        /// <summary>
        /// Gets a collection of one or more errors that have occurred.
        /// </summary>
        public IEnumerable<IHandledError> Errors { get; } = Enumerable.Empty<IHandledError>();

        /// <summary>
        /// Creates an instance of <see cref="ErrorResponse"/>.
        /// </summary>
        public ErrorResponse() { }

        /// <summary>
        /// Creates an instance of <see cref="ErrorResponse"/> with an unhandled error message.
        /// </summary>
        /// <param name="customerReference">The reference displayed to the user.</param>
        public ErrorResponse(string customerReference)
        {
            Reference = customerReference;
            Type = UnexpectedErrorType;
            Errors = new IHandledError[]
            {
                new ErrorResult(UnexpectedErrorType, UnexpectedErrorMessage)
            };
        }

        /// <summary>
        /// Creates an instance of <see cref="ErrorResponse"/>.
        /// </summary>
        /// <param name="customerReference">The reference displayed to the user.</param>
        /// <param name="errorType">The type of error that was handled.</param>
        /// <param name="errors">A collection of one or more errors that have occurred.</param>
        public ErrorResponse(string customerReference, string errorType, IEnumerable<IHandledError> errors)
        {
            if (string.IsNullOrWhiteSpace(errorType)) 
            {
                throw new ArgumentNullException(nameof(errorType));
            }

            Reference = customerReference;
            Type = errorType;
            Errors = errors.ToArray();
        }
    }
}
