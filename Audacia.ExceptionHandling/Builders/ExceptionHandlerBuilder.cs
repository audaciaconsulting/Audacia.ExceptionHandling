using System;
using System.Collections.Generic;

namespace Audacia.ExceptionHandling.Builders
{
	public class ExceptionHandlerBuilder
	{
		private readonly ExceptionHandlerCollectionBuilder _builder;

		public ExceptionHandlerBuilder(ExceptionHandlerCollectionBuilder builder) => _builder = builder;

		public ExceptionHandlerCollectionBuilder Handle<TException>(Func<TException, ErrorResult> func)
			where TException : Exception => _builder.Add(func);

		public ExceptionHandlerCollectionBuilder Handle<TException>(Func<TException, IEnumerable<ErrorResult>> func)
			where TException : Exception => _builder.Add(func);
	}
}