using System;
using System.Collections.Generic;
using System.Linq;
using Audacia.ExceptionHandling.Results;
using Microsoft.Extensions.Logging;

namespace Audacia.ExceptionHandling.Extensions
{
    /// <summary>
    /// A set of extension methods on <see cref="IHandledError"/>.
    /// </summary>
    public static class HandledErrorExtensions
    {
        /// <summary>
        /// Returns all error messages for the provided error results as a singular string.
        /// </summary>
        /// <param name="handledErrors">An enumerable of handled error results.</param>
        /// <returns>Returns a message describing all errors.</returns>
        public static string GetFullMessage(this IEnumerable<IHandledError> handledErrors)
        {
            return string.Join("\r\n", handledErrors.Select(e => e.GetFullMessage()));
        }

        /// <summary>
        /// Creates an instance of an <see cref="ErrorResponse"/>, the <see cref="ErrorResponse.CustomerReference"/> is automatically generated and logged to application insights, alongside the collection of error messages.
        /// </summary>
        /// <param name="handledErrors">An enumerable of handled error results.</param>
        /// <param name="loggerFactory">Logger factory, required for attaching customer references to error logs.</param>
        /// <param name="responseType">The error type to be displayed on the error response.</param>
        /// <returns>The response returned to the user.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null"/>.</exception>
        public static ErrorResponse CreateErrorResponse(this IEnumerable<IHandledError> handledErrors, ILoggerFactory loggerFactory, string responseType)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            // Create a customer reference to show to the user
            var customerReference = StringExtensions.GetCustomerReference();

            // Create a logger scope to attach the customer reference to log messages
            var logger = loggerFactory.CreateLogger("CreateErrorResponse");
            using (logger.BeginScope(nameof(ErrorResponse.CustomerReference), customerReference))
            using (logger.BeginScope(nameof(ErrorResponse.Errors), GetFullMessage(handledErrors)))
            {
                // Generally we don't need to attach the response data as the stack trace would over this,
                // but if they're asking for help on an MVC validation response it would be goood to know what they were shown.
                logger.LogInformation($"A {responseType} error response has been sent to the customer, please see the custom properties for more details.");
            }

            return new ErrorResponse(customerReference, responseType, handledErrors);
        }
    }
}
