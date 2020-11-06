using System.Collections.Generic;

namespace Audacia.ExceptionHandling
{
    /// <summary>Represents an API response which contains the details of an error.</summary>
    public class ErrorResult
    {
        public string Message { get; }

        public string? Property { get; }

        public Dictionary<string, object> ExtraProperties { get; }

        /// <summary>Creates a new instance of <see cref="ErrorResult"/>.</summary>
        public ErrorResult() { }

        /// <summary>Create an <see cref="ErrorResult"/> with the specified message.</summary>
        public ErrorResult(string message)
        {
            Message = message;
        }

        /// <summary>Create an error result with the specified message for the specified property.</summary>
        public ErrorResult(string message, string property) : this(message)
        {
            Property = property;
        }
    }
}