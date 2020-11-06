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
                new List<ErrorResult>
                {
                    new ErrorResult(nameof(InvalidOperationException))
                }, HttpStatusCode.Ambiguous);
            ExceptionHandlerBuilder.Add((SystemException e) =>
                    new List<ErrorResult>
                    {
                        new ErrorResult(nameof(SystemException))
                    },
                HttpStatusCode.Ambiguous);
        }

        public class Match : ExceptionHandlerBuilderTests
        {
            [Fact]
            public void Matches_The_Exact_Type()
            {
                var match = ExceptionHandlerBuilder.Get<InvalidOperationException>();
                match.Should().NotBeNull();
                var result = match?.Invoke(new InvalidOperationException());
                var actions = result?.As<IEnumerable<ErrorResult>>().ToList();
                actions.Should().HaveCount(1);
                actions?[0].Message.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public void Matches_A_Base_Type()
            {
                var match = ExceptionHandlerBuilder.Get<SystemException>();
                match.Should().NotBeNull();
            }
        }
    }
}