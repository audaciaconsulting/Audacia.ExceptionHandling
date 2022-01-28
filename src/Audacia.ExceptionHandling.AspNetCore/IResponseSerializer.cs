namespace Audacia.ExceptionHandling.AspNetCore
{
    /// <summary>
    /// Represents a type that can serialize a given <see cref="object"/> to a <see cref="string"/>.
    /// </summary>
    public interface IResponseSerializer
    {
        /// <summary>
        /// Serializes the given <paramref name="response"/>.
        /// </summary>
        /// <param name="response">The <see cref="object"/> to serialize.</param>
        /// <returns>A <see cref="string"/> containing the serialized version of the given <paramref name="response"/>.</returns>
        string Serialize(object response);
    }
}
