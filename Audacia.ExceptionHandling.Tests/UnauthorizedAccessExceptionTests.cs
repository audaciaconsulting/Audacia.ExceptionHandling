using System;
using System.Net;
using Audacia.ExceptionHandling.Builders;
using Audacia.ExceptionHandling.Json;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Audacia.ExceptionHandling.Tests
{
	public class UnauthorizedAccessExceptionTests
	{
		public UnauthorizedAccessExceptionTests()
		{
			CollectionBuilder = new ExceptionHandlerCollectionBuilder();
			HandlerBuilder = new ExceptionHandlerBuilder(CollectionBuilder);
		}

		protected ExceptionHandlerCollectionBuilder CollectionBuilder { get; }

		protected ExceptionHandlerBuilder HandlerBuilder { get; }


		[Fact]
		public void Registers_correctly()
		{
			HandlerBuilder.UnauthorizedAccessException();
			CollectionBuilder.ExceptionHandlerCollection.Should()
				.ContainKey(typeof(UnauthorizedAccessException));
		}

		[Fact]
		public void Registers_correctly_with_integer_status_code()
		{
			HandlerBuilder.UnauthorizedAccessException(418);
			CollectionBuilder.ExceptionHandlerCollection.Should()
				.ContainKey(typeof(UnauthorizedAccessException));
		}

		[Fact]
		public void Registers_correctly_with_enum_status_code()
		{
			HandlerBuilder.UnauthorizedAccessException(HttpStatusCode.UnavailableForLegalReasons);
			CollectionBuilder.ExceptionHandlerCollection.Should()
				.ContainKey(typeof(UnauthorizedAccessException));
		}
	}
}