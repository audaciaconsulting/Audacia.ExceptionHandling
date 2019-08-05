using System.Net;
using Audacia.ExceptionHandling.Builders;
using Newtonsoft.Json;

namespace Audacia.ExceptionHandling.Json
{
	public static class Extensions
	{
		public static ExceptionConfigurationBuilder JsonReaderException(this ExceptionHandlerBuilder builder, int statusCode = 400)
		{
			return builder.Handle((JsonReaderException e) => new ErrorResult((HttpStatusCode)statusCode, e.Message, e.Path)
			{
				["LineNumber"] = e.LineNumber,
				["Position"] = e.LinePosition
			});
		}
	}
}