using System.Net;
using Audacia.ExceptionHandling.Builders;
using Newtonsoft.Json;

namespace Audacia.ExceptionHandling.Json
{
	/// <summary>Extension methods.</summary>
	public static class Extensions
	{
		/// <summary>Configure the default handler for <see cref="JsonReaderException"/>.</summary>
		public static ExceptionHandlerCollectionBuilder JsonReaderException(this ExceptionHandlerBuilder builder, int statusCode = 400)
		{
			return builder.Handle((JsonReaderException e) => new ErrorResult((HttpStatusCode)statusCode, e.Message, e.Path)
			{
				["LineNumber"] = e.LineNumber,
				["Position"] = e.LinePosition
			});
		}
	}
}