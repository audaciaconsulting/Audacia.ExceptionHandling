using System;
using System.Collections.Generic;

namespace Audacia.ExceptionHandling.Builders
{
	public class ExceptionHandlerBuilder
	{
		private readonly ExceptionConfigurationBuilder _builder;

		public ExceptionHandlerBuilder(ExceptionConfigurationBuilder builder) => _builder = builder;

		public ExceptionConfigurationBuilder Handle<TException>(Func<TException, ErrorResult> func)
			where TException : Exception => _builder.Handle(func);

		public ExceptionConfigurationBuilder Handle<TException>(Func<TException, IEnumerable<ErrorResult>> func)
			where TException : Exception => _builder.Handle(func);
	}
}