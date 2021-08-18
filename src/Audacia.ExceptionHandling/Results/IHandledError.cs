namespace Audacia.ExceptionHandling.Results
{
    /// <summary>
    /// Represents an exception that has been handled and is to be returned from the API.
    /// </summary>
    public interface IHandledError
    {
        /// <summary>
        /// Returns a message describing the error.
        /// </summary>
        /// <returns>A message describing the error.</returns>
        string GetFullMessage();
    }
}
