using System;
using System.Collections.Generic;

namespace Audacia.ExceptionHandling.Builders
{
	/// <summary>Fluent API interface for configuring a set of handlers for an application.</summary>
	public class ExceptionHandlerCollectionBuilder
	{
		internal ExceptionHandlerCollection ExceptionHandlerCollection { get; } = new ExceptionHandlerCollection();
		
		/// <summary>Add the specified handler to the collection.</summary>
		public ExceptionHandlerCollectionBuilder Add<TException>(Func<TException, ErrorResult> func) where TException : Exception
		{
			ExceptionHandlerCollection.Add(func);
			return this;
		}
		
		/// <summary>Add the specified handler to the collection.</summary>
		public ExceptionHandlerCollectionBuilder Add<TException>(Func<TException, IEnumerable<ErrorResult>> func) where TException : Exception
		{
			ExceptionHandlerCollection.Add(func);
			return this;
		}
		
		/// <summary>Provides an <see cref="ExceptionHandlerBuilder"/> for configuring the handler for a single exception type.</summary>
		public ExceptionHandlerBuilder Handle => new ExceptionHandlerBuilder(this);
	}
}