using System.Collections.Generic;
using System.Net;
using Audacia.ExceptionHandling.Builders;

namespace Audacia.ExceptionHandling
{
	public static class Extensions
	{
		public static ExceptionConfigurationBuilder KeyNotFoundException(this ExceptionHandlerBuilder builder) => 
			builder.Handle((KeyNotFoundException e) => new ErrorResult(HttpStatusCode.NotFound, e.Message));
	}
}