using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Audacia.ExceptionHandling.Builders;
using FluentAssertions;
using Xunit;

namespace Audacia.ExceptionHandling.Tests
{
    public class ExceptionHandlerBuilderTests
    {
        private ExceptionHandlerBuilder ExceptionHandlerBuilder { get; } = new ExceptionHandlerBuilder();

        public ExceptionHandlerBuilderTests()
        {
            ExceptionHandlerBuilder.Add((InvalidOperationException e) =>
                new ErrorResult(nameof(InvalidOperationException)), HttpStatusCode.Ambiguous);
            ExceptionHandlerBuilder.Add((SystemException e) => new ErrorResult(nameof(SystemException)),
                HttpStatusCode.Ambiguous);
        }

        public class Match : ExceptionHandlerBuilderTests
        {
            [Fact]
            public void Matches_the_exact_type()
            {
                var match = ExceptionHandlerBuilder.Get(new InvalidOperationException());
                match.Should().NotBeNull();
                match?.ExceptionType.Should().Be(typeof(InvalidOperationException));
                var actions = match?.Action(new InvalidOperationException()).As<IEnumerable<ErrorResult>>().ToList();
                actions.Should().HaveCount(1);
                actions?[0].Message.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public void Matches_a_base_type()
            {
                var match = ExceptionHandlerBuilder.Get(new FormatException());
                match.Should().NotBeNull();
                match?.ExceptionType.Should().Be(typeof(SystemException));
            }
        }
    }
}