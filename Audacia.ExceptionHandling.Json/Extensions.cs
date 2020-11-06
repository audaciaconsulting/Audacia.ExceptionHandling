using System.Net;
using Audacia.ExceptionHandling.Builders;
using Newtonsoft.Json;

namespace Audacia.ExceptionHandling.Json
{
	/// <summary>Extension methods.</summary>
	public static class Extensions
	{
		/// <summary>Configure the default handler for <see cref="Newtonsoft.Json.JsonReaderException"/> with the specified HTTP status code.</summary>
		public static ExceptionHandlerBuilder JsonReaderException(this ExceptionHandlerBuilder builder, HttpStatusCode statusCode)
		{
			return builder.Add(statusCode, (JsonReaderException e) => new ErrorResult(e.Message, e.Path)
			{
				ExtraProperties =
				{
					["LineNumber"] = e.LineNumber,
					["Position"] = e.LinePosition
				}
			});
		}

		/// <summary>Configure the default handler for <see cref="Newtonsoft.Json.JsonReaderException"/> with the specified HTTP status code.</summary>
		public static ExceptionHandlerBuilder
			JsonReaderException(this ExceptionHandlerBuilder builder, int statusCode) =>
			builder.JsonReaderException((HttpStatusCode) statusCode);

		/// <summary>Configure the default handler for <see cref="Newtonsoft.Json.JsonReaderException"/> with an HTTP status code of 400: Bad Request.</summary>
		public static ExceptionHandlerBuilder JsonReaderException(this ExceptionHandlerBuilder builder) =>
			builder.JsonReaderException((HttpStatusCode) 400);
	}
}