using System;
using System.Linq;

namespace Audacia.ExceptionHandling.Extensions
{
    /// <summary>
    /// Extensions that return a <see cref="string"/> type.
    /// </summary>
    internal static class StringExtensions
    {
        private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        private static readonly Random Random = new Random();

        /// <summary>
        /// Generates a customer reference number in the format "XXXX-XXXX-XXXX" where X can be A-Z,0-9.
        /// </summary>
        /// <returns>Customer reference as a string.</returns>
        public static string GetCustomerReference()
        {
            return $"{GetRandomString(4)}-{GetRandomString(4)}-{GetRandomString(4)}";
        }

        private static string GetRandomString(int length)
        {
            var randomCharArray = Enumerable
                .Repeat(Characters, length)
                .Select(text =>
                {
                    var next = Random.Next(text.Length);
                    return text[next];
                })
                .ToArray();

            return new string(randomCharArray);
        }
    }
}
