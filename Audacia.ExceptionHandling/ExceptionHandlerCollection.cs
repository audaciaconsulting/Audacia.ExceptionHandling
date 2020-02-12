using System;
using System.Collections.Generic;
using System.Net;

namespace Audacia.ExceptionHandling
{
	/// <summary>A collection of <see cref="ExceptionHandler"/> instances which pertain to a particular application.</summary>
	public class ExceptionHandlerCollection : Dictionary<Type, ExceptionHandler>
	{
		private TypeMap TypeMap { get; } = new TypeMap();

		/// <summary>Add a handler to the collection.</summary>
		public void Add<TException>(Func<TException, ErrorResult> func, HttpStatusCode statusCode) where TException : Exception
		{
			Add(typeof(TException), new ExceptionHandler<TException>(func, statusCode));
			TypeMap.Invalidate();
		}

		/// <summary>Add a handler to the collection.</summary>
		public void Add<TException>(Func<TException, IEnumerable<ErrorResult>> func, HttpStatusCode statusCode) where TException : Exception
		{
			Add(typeof(TException), new ExceptionHandler<TException>(func, statusCode));
			TypeMap.Invalidate();
		}

		/// <summary>Finds the most appropriate handler for a given exception type, or null if none exist.</summary>
		/// <param name="exception"></param>
		public ExceptionHandler Match(Exception exception)
		{
			var cached = TypeMap.Find(exception.GetType());
			if (cached != null) return cached;

			// Try and return the exact match first.
			if (TryGetValue(exception.GetType(), out var handler))
			{
				TypeMap.Add(exception.GetType(), handler);
				return handler;
			}

			var hierarchy = exception.GetType().InheritanceHierarchy();

			foreach (var type in hierarchy)
			{
				if (!TryGetValue(type, out handler)) continue;
				TypeMap.Add(exception.GetType(), handler);
				return handler;
			}

			return null;
		}
	}
}