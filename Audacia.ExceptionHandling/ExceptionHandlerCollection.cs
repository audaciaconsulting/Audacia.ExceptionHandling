using System;
using System.Collections.Generic;

namespace Audacia.ExceptionHandling
{
	public class ExceptionHandlerCollection : Dictionary<Type, ExceptionHandler>
	{
		public void Add<TException>(Func<TException, ErrorResult> func) where TException : Exception =>
			Add(typeof(TException), new ExceptionHandler<TException>(func));

		public void Add<TException>(Func<TException, IEnumerable<ErrorResult>> func) where TException : Exception =>
			Add(typeof(TException), new ExceptionHandler<TException>(func));
	}
}