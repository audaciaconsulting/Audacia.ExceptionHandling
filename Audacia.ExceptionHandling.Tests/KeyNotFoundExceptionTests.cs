using System;
using System.Collections.Generic;
using System.Net;
using Audacia.ExceptionHandling.Builders;
using Audacia.ExceptionHandling.Json;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Audacia.ExceptionHandling.Tests
{
	public class KeyNotFoundExceptionTests
	{
		public KeyNotFoundExceptionTests()
		{
			HandlerBuilder = new ExceptionHandlerBuilder();
		}

		protected ExceptionHandlerBuilder HandlerBuilder { get; }


		// [Fact]
		// public void Registers_correctly()
		// {
		// 	HandlerBuilder.KeyNotFoundException();
		// 	CollectionBuilder.ExceptionHandlerCollection.Should()
		// 		.ContainKey(typeof(KeyNotFoundException));
		// }
		//
		// [Fact]
		// public void Registers_correctly_with_integer_status_code()
		// {
		// 	HandlerBuilder.KeyNotFoundException(418);
		// 	CollectionBuilder.ExceptionHandlerCollection.Should()
		// 		.ContainKey(typeof(KeyNotFoundException));
		// }
		//
		// [Fact]
		// public void Registers_correctly_with_enum_status_code()
		// {
		// 	HandlerBuilder.KeyNotFoundException(HttpStatusCode.UnavailableForLegalReasons);
		// 	CollectionBuilder.ExceptionHandlerCollection.Should()
		// 		.ContainKey(typeof(KeyNotFoundException));
		// }
	}
}