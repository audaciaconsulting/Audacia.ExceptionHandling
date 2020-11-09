using System.Net;

namespace Audacia.ExceptionHandling.Handlers
{
    /// <summary>
    /// A generic-less interface for <see cref="IHttpExceptionHandler"/>.
    /// </summary>
    public interface IHttpExceptionHandler
    {
        /// <summary>
        /// The status code to return for the given exception type.
        /// </summary>
        public HttpStatusCode StatusCode { get; }
    }
}