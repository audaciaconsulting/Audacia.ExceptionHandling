using System.Net;
using Audacia.ExceptionHandling.Builders;
using Audacia.ExceptionHandling.Json;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Audacia.ExceptionHandling.Tests
{
	public class JsonReaderExceptionTests
	{
		public JsonReaderExceptionTests()
		{
			CollectionBuilder = new ExceptionHandlerCollectionBuilder();
			HandlerBuilder = new ExceptionHandlerBuilder(CollectionBuilder);
		}

		protected ExceptionHandlerCollectionBuilder CollectionBuilder { get; }

		protected ExceptionHandlerBuilder HandlerBuilder { get; }


		[Fact]
		public void Registers_correctly()
		{
			HandlerBuilder.JsonReaderException();
			CollectionBuilder.ExceptionHandlerCollection.Should()
				.ContainKey(typeof(JsonReaderException));
		}

		[Fact]
		public void Registers_correctly_with_integer_status_code()
		{
			HandlerBuilder.JsonReaderException(418);
			CollectionBuilder.ExceptionHandlerCollection.Should()
				.ContainKey(typeof(JsonReaderException));
		}

		[Fact]
		public void Registers_correctly_with_enum_status_code()
		{
			HandlerBuilder.JsonReaderException(HttpStatusCode.UnavailableForLegalReasons);
			CollectionBuilder.ExceptionHandlerCollection.Should()
				.ContainKey(typeof(JsonReaderException));
		}
	}
}