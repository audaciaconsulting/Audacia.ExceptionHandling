using System;
using System.Net;
using Audacia.ExceptionHandling.Handlers;
using Audacia.ExceptionHandling.Results;
using FluentAssertions;
using Xunit;

namespace Audacia.ExceptionHandling.Tests
{
    public class UnauthorizedAccessExceptionTests
    {
        private ExceptionHandlerCollection HandlerCollection { get; }

        public UnauthorizedAccessExceptionTests()
        {
            HandlerCollection = new ExceptionHandlerCollection();
        }

        [Fact]
        public void Registers_Correctly()
        {
            HandlerCollection.UnauthorizedAccessException();
            var handler = HandlerCollection.Get<UnauthorizedAccessException>();
            handler.Should().NotBeNull();
        }

        [Fact]
        public void Registers_Correctly_With_Integer_Status_Code()
        {
            HandlerCollection.UnauthorizedAccessException(418);
            var handler = HandlerCollection.Get<UnauthorizedAccessException>();
            handler.Should().NotBeNull();
            handler.Should().BeOfType<HttpExceptionHandler<UnauthorizedAccessException, ErrorResult>>();
            if (handler is HttpExceptionHandler<Exception, object> unauthorizedHandler)
            {
                unauthorizedHandler.StatusCode.Should().Be(418);
            }
        }

        [Fact]
        public void Registers_Correctly_With_Enum_Status_Code()
        {
            HandlerCollection.UnauthorizedAccessException(HttpStatusCode.UnavailableForLegalReasons);
            var handler = HandlerCollection.Get<UnauthorizedAccessException>();
            handler.Should().NotBeNull();
            handler.Should().BeOfType<HttpExceptionHandler<UnauthorizedAccessException, ErrorResult>>();
            if (handler is HttpExceptionHandler<Exception, object> unauthorizedHandler)
            {
                unauthorizedHandler.StatusCode.Should().Be(HttpStatusCode.UnavailableForLegalReasons);
            }
        }
    }
}