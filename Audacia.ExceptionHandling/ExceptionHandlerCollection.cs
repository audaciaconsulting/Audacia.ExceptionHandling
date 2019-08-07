using System;
using System.Collections.Generic;

namespace Audacia.ExceptionHandling
{
	/// <summary>A collection of <see cref="ExceptionHandler"/> instances which pertain to a particular application.</summary>
	public class ExceptionHandlerCollection : Dictionary<Type, ExceptionHandler>
	{
		/// <summary>Add a handler to the collection.</summary>
		public void Add<TException>(Func<TException, ErrorResult> func) where TException : Exception =>
			Add(typeof(TException), new ExceptionHandler<TException>(func));

		/// <summary>Add a handler to the collection.</summary>
		public void Add<TException>(Func<TException, IEnumerable<ErrorResult>> func) where TException : Exception =>
			Add(typeof(TException), new ExceptionHandler<TException>(func));
	}
}