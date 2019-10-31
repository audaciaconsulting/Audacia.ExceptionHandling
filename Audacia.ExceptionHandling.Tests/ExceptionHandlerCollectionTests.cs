using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Audacia.ExceptionHandling.Tests
{
	public class ExceptionHandlerCollectionTests
	{
		private ExceptionHandlerCollection ExceptionHandlerCollection { get; } = new ExceptionHandlerCollection();

		public ExceptionHandlerCollectionTests()
		{
			ExceptionHandlerCollection.Add((InvalidOperationException e) =>
				new ErrorResult(nameof(InvalidOperationException)));
			ExceptionHandlerCollection.Add((SystemException e) => new ErrorResult(nameof(SystemException)));
		}

		public class Match : ExceptionHandlerCollectionTests
		{
			[Fact]
			public void Matches_the_exact_type()
			{
				var match = ExceptionHandlerCollection.Match(new InvalidOperationException());
				match.Should().NotBeNull();
				match.ExceptionType.Should().Be(typeof(InvalidOperationException));
				var actions = match.Action(new InvalidOperationException()).ToList();
				actions.Should().HaveCount(1);
				actions[0].Message.Should().Be(nameof(InvalidOperationException));
			}

			[Fact]
			public void Matches_a_base_type()
			{
				var match = ExceptionHandlerCollection.Match(new FormatException());
				match.Should().NotBeNull();
				match.ExceptionType.Should().Be(typeof(SystemException));
			}
		}
	}
}