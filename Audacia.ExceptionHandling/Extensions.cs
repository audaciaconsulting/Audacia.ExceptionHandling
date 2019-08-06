using System;
using System.Collections.Generic;
using System.Net;
using Audacia.ExceptionHandling.Builders;

namespace Audacia.ExceptionHandling
{
	public static class Extensions
	{
		public static ExceptionHandlerCollectionBuilder KeyNotFoundException(this ExceptionHandlerBuilder builder) => 
			builder.Handle((KeyNotFoundException e) => new ErrorResult(HttpStatusCode.NotFound, e.Message));
		
		public static ExceptionHandlerCollectionBuilder UnauthorizedAccessException(this ExceptionHandlerBuilder builder) => 
			builder.Handle((UnauthorizedAccessException e) => new ErrorResult(HttpStatusCode.Unauthorized));

		public static ExceptionHandlerCollectionBuilder Handle<T>(
			this ExceptionHandlerCollectionBuilder builder, 
			Func<T, ErrorResult> handler) where T : Exception => 
			builder.Handle.Handle(handler);
	}
}