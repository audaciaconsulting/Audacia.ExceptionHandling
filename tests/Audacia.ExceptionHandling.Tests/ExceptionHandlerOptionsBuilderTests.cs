using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using Audacia.ExceptionHandling.Results;
using FluentAssertions;
using Xunit;

namespace Audacia.ExceptionHandling.Tests
{
    public class ExceptionHandlerOptionsBuilderTests
    {
        private const string MessageFormat = "An {0} has occurred.";

        private ExceptionHandlerOptionsBuilder Builder { get; } = new ExceptionHandlerOptionsBuilder();

        public ExceptionHandlerOptionsBuilderTests()
        {
            Builder.Handle((InvalidOperationException e) =>
            {
                return new ErrorResult(
                    nameof(InvalidOperationException),
                    string.Format(MessageFormat, nameof(InvalidOperationException)));
            }, HttpStatusCode.Ambiguous);

            Builder.Handle((SystemException e) =>
            {
                return new ErrorResult(
                    nameof(SystemException),
                    string.Format(MessageFormat, nameof(SystemException)));
            }, HttpStatusCode.Ambiguous);

            Builder.Handle((ValidationException e) =>
            {
                return new[]
                {
                    new ErrorResult("FirstName", "First name is required."),
                    new ErrorResult("DateOfBirth", 
                        "Must be 18+ years old to register.", 
                        "Date must be entered in the format YYYY-MM-DD"),
                    new ErrorResult("Consent", new List<string>
                    {
                        "Users must over the age of 18 to register.",
                        "Consent must be provided in order to create an account.",
                        "You must actually read our terms and conditions before clicking accept.",
                    })
                };
            }, HttpStatusCode.Ambiguous);
        }

        [Fact]
        public void Matches_The_Exact_Type()
        {
            var expectedCode = nameof(InvalidOperationException);
            var expectedMessage = new[] { string.Format(MessageFormat, nameof(InvalidOperationException)) };

            var provider = Builder.Build();

            var exceptionHandler = provider.ResolveExceptionHandler<InvalidOperationException>();

            exceptionHandler.Should().NotBeNull();

            var handledErrorEnumerable = exceptionHandler.Invoke(new InvalidOperationException());

            var errorModels = handledErrorEnumerable.Cast<ErrorResult>().ToArray();
            errorModels.Should().HaveCount(1);

            errorModels[0].Should().NotBeNull();
            errorModels[0].Code.Should().Be(expectedCode);
            errorModels[0].Messages.Should().BeEquivalentTo(expectedMessage);
        }

        [Fact]
        public void Matches_A_Base_Type()
        {
            var expectedCode = nameof(SystemException);
            var expectedMessage = new[] { string.Format(MessageFormat, nameof(SystemException)) };

            var provider = Builder.Build();

            var exceptionHandler = provider.ResolveExceptionHandler<SystemException>();

            exceptionHandler.Should().NotBeNull();

            var handledErrorEnumerable = exceptionHandler.Invoke(new SystemException());

            var errorModels = handledErrorEnumerable.Cast<ErrorResult>().ToArray();
            errorModels.Should().HaveCount(1);

            errorModels[0].Should().NotBeNull();
            errorModels[0].Code.Should().Be(expectedCode);
            errorModels[0].Messages.Should().BeEquivalentTo(expectedMessage);
        }

        [Fact]
        public void Returns_Multiple_Validation_Errors()
        {
            var provider = Builder.Build();

            var exceptionHandler = provider.ResolveExceptionHandler<ValidationException>();

            exceptionHandler.Should().NotBeNull();

            var handledErrorEnumerable = exceptionHandler.Invoke(new ValidationException());

            var errorModels = handledErrorEnumerable.ToArray();
            errorModels.Should().HaveCount(3);

            errorModels[0].Code.Should().Be("FirstName");
            errorModels[0].Messages.Should().HaveCount(1);

            errorModels[1].Code.Should().Be("DateOfBirth");
            errorModels[1].Messages.Should().HaveCount(2);

            errorModels[2].Code.Should().Be("Consent");
            errorModels[2].Messages.Should().HaveCount(3);
        }
    }
}