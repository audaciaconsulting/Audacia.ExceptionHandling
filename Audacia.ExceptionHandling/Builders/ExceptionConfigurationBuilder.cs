using System;
using System.Collections.Generic;

namespace Audacia.ExceptionHandling.Builders
{
	public class ExceptionConfigurationBuilder
	{
		private ExceptionHandlerCollection Exceptions { get; } = new ExceptionHandlerCollection();
		
		public ExceptionConfigurationBuilder Handle<TException>(Func<TException, ErrorResult> func) where TException : Exception
		{
			Exceptions.Add(func);
			return this;
		}
		
		public ExceptionConfigurationBuilder Handle<TException>(Func<TException, IEnumerable<ErrorResult>> func) where TException : Exception
		{
			Exceptions.Add(func);
			return this;
		}
		
		public ExceptionHandlerBuilder Handle() => new ExceptionHandlerBuilder(this);
	}
}