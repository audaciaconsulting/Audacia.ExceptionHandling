using System;
using System.Collections.Generic;

namespace Audacia.ExceptionHandling.Builders
{
	/// <summary>Fluent API interface for configuring a handler for a specific exception.</summary>
	public class ExceptionHandlerBuilder
	{
		private readonly ExceptionHandlerCollectionBuilder _builder;

		/// <summary>Create a new instance of <see cref="ExceptionHandlerBuilder"/>.</summary>
		public ExceptionHandlerBuilder(ExceptionHandlerCollectionBuilder builder) => _builder = builder;

		/// <summary>Handle the specified exception type using the specified handler.</summary>
		public ExceptionHandlerCollectionBuilder Handle<TException>(Func<TException, ErrorResult> func)
			where TException : Exception => _builder.Add(func);

		/// <summary>Handle the specified exception type using the specified handler.</summary>
		public ExceptionHandlerCollectionBuilder Handle<TException>(Func<TException, IEnumerable<ErrorResult>> func)
			where TException : Exception => _builder.Add(func);
	}
}