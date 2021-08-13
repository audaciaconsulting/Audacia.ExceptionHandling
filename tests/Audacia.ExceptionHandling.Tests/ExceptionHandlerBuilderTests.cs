using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Audacia.ExceptionHandling.Results;
using FluentAssertions;
using Xunit;

namespace Audacia.ExceptionHandling.Tests
{
    public class ExceptionHandlerBuilderTests
    {
        private const string ExampleCustomerReference = "4444-3333-2222";
        private const string MessageFormat = "An {0} has occurred.";

        private ExceptionHandlerMap ExceptionHandlerMap { get; } = new ExceptionHandlerMap();

        public ExceptionHandlerBuilderTests()
        {
            ExceptionHandlerMap.Add((string customerReference, InvalidOperationException e) => new[]
            {
                new ErrorResult(
                    customerReference: customerReference,
                    message: string.Format(MessageFormat, nameof(InvalidOperationException)),
                    errorCode: nameof(InvalidOperationException).GetHashCode().ToString(),
                    errorType: nameof(InvalidOperationException))
            }, HttpStatusCode.Ambiguous);

            ExceptionHandlerMap.Add((string customerReference, SystemException e) => new []
            {
                new ErrorResult(
                    customerReference: customerReference,
                    message: string.Format(MessageFormat, nameof(SystemException)),
                    errorCode: nameof(SystemException).GetHashCode().ToString(),
                    errorType: nameof(SystemException))
            },  HttpStatusCode.Ambiguous);
        }

        public class Match : ExceptionHandlerBuilderTests
        {
            [Fact]
            public void Matches_The_Exact_Type()
            {
                var handler = ExceptionHandlerMap.Get<InvalidOperationException>();
                handler.Should().NotBeNull();

                var result = handler?.Invoke(ExampleCustomerReference, new InvalidOperationException());

                var actions = result?.As<IEnumerable<ErrorResult>>().ToList();
                actions.Should().HaveCount(1);

                var errorResult = actions?[0];
                errorResult.CustomerReference.Should().Be(ExampleCustomerReference);
                errorResult.Message.Should().Be(string.Format(MessageFormat, nameof(InvalidOperationException)));
                errorResult.ErrorCode.Should().Be(nameof(InvalidOperationException).GetHashCode().ToString());
                errorResult.ErrorType.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public void Matches_A_Base_Type()
            {
                var match = ExceptionHandlerMap.Get<SystemException>();
                match.Should().NotBeNull();
            }
        }
    }
}