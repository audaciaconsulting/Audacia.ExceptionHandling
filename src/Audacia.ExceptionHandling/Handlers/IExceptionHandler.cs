﻿using System;
using Microsoft.Extensions.Logging;

namespace Audacia.ExceptionHandling.Handlers
{
    /// <summary>
    /// Interface describing the exception handler that can be used with a generic argument.
    /// </summary>
    public interface IExceptionHandler
    {
        /// <summary>
        /// Given an exception return a result.
        /// </summary>
        /// <param name="customerReference">The customer reference to be attached to the error result.</param>
        /// <param name="exception">The exception that has been encountered.</param>
        /// <returns>The error result that has been setup.</returns>
        public object? Invoke(string customerReference, Exception exception);

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="logger">An instance of <see cref="ILogger"/>.</param>
        /// <param name="exception">The exception that has been encountered.</param>
        /// <returns>True if the exception was logged, false if not, an exception if the exception input is not of the correct type.</returns>
        public bool Log(ILogger logger, Exception exception);
    }
}