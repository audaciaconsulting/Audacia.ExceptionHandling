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
        private ExceptionHandlerCollection ExceptionHandlerCollection { get; } = new ExceptionHandlerCollection();

        public ExceptionHandlerBuilderTests()
        {
            ExceptionHandlerCollection.Add((InvalidOperationException e) =>
                new List<ErrorResult>
                {
                    new ErrorResult(nameof(InvalidOperationException), nameof(InvalidOperationException),
                        nameof(InvalidOperationException))
                }, HttpStatusCode.Ambiguous);
            ExceptionHandlerCollection.Add((SystemException e) =>
                    new List<ErrorResult>
                    {
                        new ErrorResult(nameof(SystemException), nameof(SystemException), nameof(SystemException))
                    },
                HttpStatusCode.Ambiguous);
        }

        public class Match : ExceptionHandlerBuilderTests
        {
            [Fact]
            public void Matches_The_Exact_Type()
            {
                var match = ExceptionHandlerCollection.Get<InvalidOperationException>();
                match.Should().NotBeNull();
                var result = match?.Invoke(new InvalidOperationException());
                var actions = result?.As<IEnumerable<ErrorResult>>().ToList();
                actions.Should().HaveCount(1);
                actions?[0].Message.Should().Be(nameof(InvalidOperationException));
                actions?[0].ErrorCode.Should().Be(nameof(InvalidOperationException));
                actions?[0].ErrorType.Should().Be(nameof(InvalidOperationException));
            }

            [Fact]
            public void Matches_A_Base_Type()
            {
                var match = ExceptionHandlerCollection.Get<SystemException>();
                match.Should().NotBeNull();
            }
        }
    }
}