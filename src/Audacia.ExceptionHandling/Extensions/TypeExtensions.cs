using System;
using System.Collections.Generic;

namespace Audacia.ExceptionHandling.Extensions
{
    /// <summary>
    /// Extensions on the <see cref="Type"/> type.
    /// </summary>
    internal static class TypeExtensions
    {
        /// <summary>
        /// Get all the <see cref="Type"/>s that the given <see cref="Type"/> inherits from.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to get the hierarchy for.</param>
        /// <returns>All the <see cref="Type"/>s that the provided <see cref="Type"/>inherit from.</returns>
        public static IEnumerable<Type> InheritanceHierarchy(this Type type)
        {
            yield return type;

            while (type.BaseType != null)
            {
                yield return type.BaseType;
                type = type.BaseType;
            }
        }
    }
}