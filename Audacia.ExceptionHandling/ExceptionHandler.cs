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

	/// <summary>Handles an exception, transforming it into a standardized <see cref="ErrorResult"/>.</summary>
	public abstract class ExceptionHandler
	{
		/// <summary>The type of Exception this handler handles.</summary>
		public abstract Type ExceptionType { get; }
		
		/// <summary>The function which transforms the exception into an <see cref="ErrorResult"/>.</summary>
		public abstract Func<Exception, IEnumerable<ErrorResult>> Action { get; }
	}
}