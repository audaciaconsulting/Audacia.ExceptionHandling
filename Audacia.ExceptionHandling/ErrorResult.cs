using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace Audacia.ExceptionHandling
{
    /// <summary>Represents an API response which contains the details of an error.</summary>
    public class ErrorResult : Dictionary<string, object>
    {
        [IgnoreDataMember] private HttpStatusCode StatusCode { get; }
        
        /// <summary>The message which summarises the error.</summary>
        public string Message => this["Message"].ToString();

        /// <summary>The property on the model which caused the error, if applicable.</summary>
        public string Property => this["Property"].ToString();

        public ErrorResult() { }
        
        public ErrorResult(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
        
        public ErrorResult(string message)
        {
            this["Message"] = message;
        }
        
        public ErrorResult(string message, string property)
        {
            this["Message"] = message;
            this["Property"] = property;
        }
        
        public ErrorResult(HttpStatusCode statusCode, string message) : this(message)
        {
            StatusCode = statusCode;
        }
        
        public ErrorResult(HttpStatusCode statusCode, string message, string property) : this(message, property)
        {
            StatusCode = statusCode;
        }
    }
}