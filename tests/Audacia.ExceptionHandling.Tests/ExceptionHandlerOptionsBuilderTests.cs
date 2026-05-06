using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using Audacia.ExceptionHandling.Results;
using Shouldly;
using Xunit;

namespace Audacia.ExceptionHandling.Tests
{
    public class ExceptionHandlerOptionsBuilderTests
    {
        private const string MessageFormat = "An {0} has occurred.";

        private ExceptionHandlerOptionsBuilder Builder { get; } = new ExceptionHandlerOptionsBuilder();

        public ExceptionHandlerOptionsBuilderTests()
        {
            Builder.Handle(
                (InvalidOperationException _) =>
            {
                return new ErrorResult(
                    nameof(InvalidOperationException),
                    string.Format(
                        CultureInfo.InvariantCulture, MessageFormat, nameof(InvalidOperationException)));
            }, HttpStatusCode.Ambiguous);

            Builder.Handle(
                (SystemException _) =>
            {
                return new ErrorResult(
                    nameof(SystemException),
                    string.Format(
                        CultureInfo.InvariantCulture, MessageFormat, nameof(SystemException)));
            }, HttpStatusCode.Ambiguous);

            Builder.Handle(
                (ValidationException _) =>
                {
                    return
                    [
                        new ErrorResult("FirstName", "First name is required."),
                        new ErrorResult(
                            "DateOfBirth", 
                            "Must be 18+ years old to register.", 
                            "Date must be entered in the format YYYY-MM-DD"),
                        new ErrorResult("Consent", new List<string>
                        {
                            "Users must over the age of 18 to register.",
                            "Consent must be provided in order to create an account.",
                            "You must actually read our terms and conditions before clicking accept."
                        })
                    ];
                }, 
                HttpStatusCode.Ambiguous);
        }

        [Fact]
        public void Matches_The_Exact_Type()
        {
            const string expectedCode = nameof(InvalidOperationException);
            var expectedMessage = new[] 
            { 
                string.Format(CultureInfo.InvariantCulture, MessageFormat, nameof(InvalidOperationException)) 
            };

            var provider = Builder.Build();

            var exceptionHandler = provider.ResolveExceptionHandler<InvalidOperationException>();

            exceptionHandler.ShouldNotBeNull();

            var exception = new InvalidOperationException();
            var handledErrorEnumerable = exceptionHandler.Invoke(exception);

            var errorModels = handledErrorEnumerable.ToArray();
            errorModels.ShouldHaveSingleItem();

            errorModels[0].ShouldNotBeNull();
            errorModels[0].Code.ShouldBe(expectedCode);
            errorModels[0].Messages.ShouldBeEquivalentTo(expectedMessage);
        }

        [Fact]
        public void Matches_A_Base_Type()
        {
            const string expectedCode = nameof(SystemException);
            var expectedMessage = new[] 
            { 
                string.Format(CultureInfo.InvariantCulture, MessageFormat, nameof(SystemException)) 
            };

            var provider = Builder.Build();

            var exceptionHandler = provider.ResolveExceptionHandler<SystemException>();

            exceptionHandler.ShouldNotBeNull();

            var exception = new SystemException();
            var handledErrorEnumerable = exceptionHandler.Invoke(exception);

            var errorModels = handledErrorEnumerable.ToArray();
            errorModels.ShouldHaveSingleItem();

            errorModels[0].ShouldNotBeNull();
            errorModels[0].Code.ShouldBe(expectedCode);
            errorModels[0].Messages.ShouldBeEquivalentTo(expectedMessage);
        }

        [Fact]
        public void Returns_Multiple_Validation_Errors()
        {
            var provider = Builder.Build();

            var exceptionHandler = provider.ResolveExceptionHandler<ValidationException>();

            exceptionHandler.ShouldNotBeNull();

            var exception = new ValidationException();
            var handledErrorEnumerable = exceptionHandler.Invoke(exception);

            var errorModels = handledErrorEnumerable.ToArray();
            errorModels.Length.ShouldBe(3);

            errorModels[0].Code.ShouldBe("FirstName");
            errorModels[0].Messages.ShouldHaveSingleItem();

            errorModels[1].Code.ShouldBe("DateOfBirth");
            errorModels[1].Messages.Count().ShouldBe(2);

            errorModels[2].Code.ShouldBe("Consent");
            errorModels[2].Messages.Count().ShouldBe(3);
        }
    }
}