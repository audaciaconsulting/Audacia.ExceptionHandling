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
            HandlerCollection = new ExceptionHandlerCollection();
        }

        protected ExceptionHandlerCollection HandlerCollection { get; }

        [Fact]
        public void Registers_Correctly()
        {
            HandlerCollection.JsonReaderException();
            var handler = HandlerCollection.Get<JsonReaderException>();
            handler.Should().NotBeNull();
        }

        [Fact]
        public void Registers_Correctly_With_Integer_Status_Code()
        {
            HandlerCollection.JsonReaderException(418);
            var handler = HandlerCollection.Get<JsonReaderException>();
            handler.Should().NotBeNull();
            handler.Should().BeAssignableTo<IHttpExceptionHandler>();
            (handler as IHttpExceptionHandler)?.StatusCode.Should().Be(418);
        }

        [Fact]
        public void Registers_Correctly_With_Enum_Status_Code()
        {
            HandlerCollection.JsonReaderException(HttpStatusCode.UnavailableForLegalReasons);
            var handler = HandlerCollection.Get<JsonReaderException>();
            handler.Should().NotBeNull();
            handler.Should().BeAssignableTo<IHttpExceptionHandler>();
            (handler as IHttpExceptionHandler)?.StatusCode.Should().Be(HttpStatusCode.UnavailableForLegalReasons);
        }
    }
}