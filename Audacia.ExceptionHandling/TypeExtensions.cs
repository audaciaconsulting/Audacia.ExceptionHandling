using System;
using System.Collections.Generic;

namespace Audacia.ExceptionHandling
{
	internal static class TypeExtensions
	{
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