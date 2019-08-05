using System;
using System.Collections.Generic;

namespace Audacia.ExceptionHandling
{
	internal class ExceptionHandler<TException> : ExceptionHandler where TException : Exception
	{
		public override Func<Exception, IEnumerable<ErrorResult>> Action { get; }
		
		public override Type ExceptionType => typeof(TException);
		
		public ExceptionHandler(Func<TException, IEnumerable<ErrorResult>> action) => Action = x => action((TException) x);

		public ExceptionHandler(Func<TException,ErrorResult> action) => Action = x => new[] {action((TException) x) };
	}

	public abstract class ExceptionHandler
	{
		public abstract Type ExceptionType { get; }
		
		public abstract Func<Exception, IEnumerable<ErrorResult>> Action { get; }
	}
}