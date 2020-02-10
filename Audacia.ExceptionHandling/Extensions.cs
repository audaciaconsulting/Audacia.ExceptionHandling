using System;
using System.Collections.Generic;
using System.Net;
using Audacia.ExceptionHandling.Builders;

namespace Audacia.ExceptionHandling
{
	/// <summary>Extension methods.</summary>
	public static class Extensions
	{
		/// <summary>Register the default handler for a <see cref="System.Collections.Generic.KeyNotFoundException"/> with the specified HTTP status code.</summary>
		public static ExceptionHandlerCollectionBuilder KeyNotFoundException(this ExceptionHandlerBuilder builder, HttpStatusCode statusCode) =>
			builder.Handle(statusCode, (KeyNotFoundException e) => new ErrorResult(e.Message));

		/// <summary>Register the default handler for a <see cref="System.Collections.Generic.KeyNotFoundException"/> with the specified HTTP status code.</summary>
		public static ExceptionHandlerCollectionBuilder KeyNotFoundException(this ExceptionHandlerBuilder builder, int statusCode) => builder.KeyNotFoundException((HttpStatusCode)statusCode);

		/// <summary>Register the default handler for a <see cref="System.Collections.Generic.KeyNotFoundException"/>with an HTTP status code of 404: Not Found.</summary>
		public static ExceptionHandlerCollectionBuilder KeyNotFoundException(this ExceptionHandlerBuilder builder) => builder.KeyNotFoundException(HttpStatusCode.NotFound);

		/// <summary>Register the default handler for a <see cref="System.UnauthorizedAccessException"/> with the specified HTTP status code.</summary>
		public static ExceptionHandlerCollectionBuilder UnauthorizedAccessException(this ExceptionHandlerBuilder builder, HttpStatusCode statusCode) =>
			builder.Handle(statusCode, (UnauthorizedAccessException e) => new ErrorResult());

		/// <summary>Register the default handler for a <see cref="System.UnauthorizedAccessException"/> with the specified HTTP status code.</summary>
		public static ExceptionHandlerCollectionBuilder UnauthorizedAccessException(this ExceptionHandlerBuilder builder, int statusCode) =>
			builder.UnauthorizedAccessException((HttpStatusCode)statusCode);

		/// <summary>Register the default handler for a <see cref="System.UnauthorizedAccessException"/> with an HTTP status code of 401: Unauthorized.</summary>
		public static ExceptionHandlerCollectionBuilder UnauthorizedAccessException(this ExceptionHandlerBuilder builder) =>
			builder.UnauthorizedAccessException(HttpStatusCode.Unauthorized);

		/// <summary>Register a handler for the specified exception, using the specified transformation function.</summary>
		public static ExceptionHandlerCollectionBuilder Handle<T>(
			this ExceptionHandlerCollectionBuilder builder, HttpStatusCode statusCode, Func<T, ErrorResult> handler) where T : Exception =>
			builder.Handle.Handle(statusCode, handler);

		/// <summary>Register a handler for the specified exception, using the specified transformation function.</summary>
		public static ExceptionHandlerCollectionBuilder Handle<T>(
			this ExceptionHandlerCollectionBuilder builder,
			HttpStatusCode statusCode,
			Func<T, IEnumerable<ErrorResult>> handler) where T : Exception =>
			builder.Handle.Handle(statusCode, handler);
	}
}