using System;
using System.Collections.Generic;
using System.Reflection;

namespace Audacia.ExceptionHandling.Builders
{
	public class ExceptionHandlerCollectionBuilder
	{
		internal ExceptionHandlerCollection ExceptionHandlerCollection { get; } = new ExceptionHandlerCollection();
		
		public ExceptionHandlerCollectionBuilder Add<TException>(Func<TException, ErrorResult> func) where TException : Exception
		{
			ExceptionHandlerCollection.Add(func);
			return this;
		}
		
		public ExceptionHandlerCollectionBuilder Add<TException>(Func<TException, IEnumerable<ErrorResult>> func) where TException : Exception
		{
			ExceptionHandlerCollection.Add(func);
			return this;
		}
		
		public ExceptionHandlerBuilder Handle => new ExceptionHandlerBuilder(this);
	}
}