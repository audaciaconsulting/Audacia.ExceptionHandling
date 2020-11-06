using System.Collections.Generic;
using System.Net;
using Audacia.ExceptionHandling.Handlers;
using FluentAssertions;
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

        [Fact]
        public void Registers_Correctly()
        {
            HandlerBuilder.KeyNotFoundException();
            var handler = HandlerBuilder.Get<KeyNotFoundException>();
            handler.Should().NotBeNull();
        }

        [Fact]
        public void Registers_Correctly_With_Integer_Status_Code()
        {
            HandlerBuilder.KeyNotFoundException(418);
            var handler = HandlerBuilder.Get<KeyNotFoundException>();
            handler.Should().NotBeNull();
            handler.Should().BeAssignableTo<IHttpExceptionHandler>();
            (handler as IHttpExceptionHandler)?.StatusCode.Should().Be(418);
        }

        [Fact]
        public void Registers_Correctly_With_Enum_Status_Code()
        {
            HandlerBuilder.KeyNotFoundException(HttpStatusCode.UnavailableForLegalReasons);
            var handler = HandlerBuilder.Get<KeyNotFoundException>();
            handler.Should().NotBeNull();
            handler.Should().BeAssignableTo<IHttpExceptionHandler>();
            (handler as IHttpExceptionHandler)?.StatusCode.Should().Be(HttpStatusCode.UnavailableForLegalReasons);
        }
    }
}