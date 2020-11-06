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
    public class UnauthorizedAccessExceptionTests
    {
        private ExceptionHandlerBuilder HandlerBuilder { get; }

        public UnauthorizedAccessExceptionTests()
        {
            HandlerBuilder = new ExceptionHandlerBuilder();
        }

        [Fact]
        public void Registers_correctly()
        {
            HandlerBuilder.UnauthorizedAccessException();
            var handler = HandlerBuilder.Get<UnauthorizedAccessException>();
            handler.Should().NotBeNull();
        }

        [Fact]
        public void Registers_Correctly_With_Integer_Status_Code()
        {
            HandlerBuilder.UnauthorizedAccessException(418);
            var handler = HandlerBuilder.Get<UnauthorizedAccessException>();
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
            HandlerBuilder.UnauthorizedAccessException(HttpStatusCode.UnavailableForLegalReasons);
            var handler = HandlerBuilder.Get<UnauthorizedAccessException>();
            handler.Should().NotBeNull();
            handler.Should().BeOfType<HttpExceptionHandler<UnauthorizedAccessException, ErrorResult>>();
            if (handler is HttpExceptionHandler<Exception, object> unauthorizedHandler)
            {
                unauthorizedHandler.StatusCode.Should().Be(HttpStatusCode.UnavailableForLegalReasons);
            }
        }
    }
}