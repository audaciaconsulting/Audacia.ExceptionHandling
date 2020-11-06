using System.Net;
using Audacia.ExceptionHandling.Handlers;
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
            HandlerBuilder = new ExceptionHandlerBuilder();
        }

        protected ExceptionHandlerBuilder HandlerBuilder { get; }

        [Fact]
        public void Registers_Correctly()
        {
            HandlerBuilder.JsonReaderException();
            var handler = HandlerBuilder.Get<JsonReaderException>();
            handler.Should().NotBeNull();
        }

        [Fact]
        public void Registers_Correctly_With_Integer_Status_Code()
        {
            HandlerBuilder.JsonReaderException(418);
            var handler = HandlerBuilder.Get<JsonReaderException>();
            handler.Should().NotBeNull();
            handler.Should().BeAssignableTo<IHttpExceptionHandler>();
            (handler as IHttpExceptionHandler)?.StatusCode.Should().Be(418);
        }

        [Fact]
        public void Registers_Correctly_With_Enum_Status_Code()
        {
            HandlerBuilder.JsonReaderException(HttpStatusCode.UnavailableForLegalReasons);
            var handler = HandlerBuilder.Get<JsonReaderException>();
            handler.Should().NotBeNull();
            handler.Should().BeAssignableTo<IHttpExceptionHandler>();
            (handler as IHttpExceptionHandler)?.StatusCode.Should().Be(HttpStatusCode.UnavailableForLegalReasons);
        }
    }
}