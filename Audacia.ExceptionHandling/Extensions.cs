using System;
using System.Collections.Generic;
using System.Net;
using Audacia.ExceptionHandling.Builders;

namespace Audacia.ExceptionHandling
{
	/// <summary>Extension methods.</summary>
	public static class Extensions
	{
		/// <summary>Register the default handler for a <see cref="KeyNotFoundException"/>.</summary>
		public static ExceptionHandlerCollectionBuilder KeyNotFoundException(this ExceptionHandlerBuilder builder) => 
			builder.Handle((KeyNotFoundException e) => new ErrorResult(HttpStatusCode.NotFound, e.Message));
		
		/// <summary>Register the default handler for a <see cref="UnauthorizedAccessException"/>.</summary>
		public static ExceptionHandlerCollectionBuilder UnauthorizedAccessException(this ExceptionHandlerBuilder builder) => 
			builder.Handle((UnauthorizedAccessException e) => new ErrorResult(HttpStatusCode.Unauthorized));

		/// <summary>Register a handler for the specified exception, using the specified transformation function.</summary>
		public static ExceptionHandlerCollectionBuilder Handle<T>(
			this ExceptionHandlerCollectionBuilder builder, 
			Func<T, ErrorResult> handler) where T : Exception => 
			builder.Handle.Handle(handler);
	}
}