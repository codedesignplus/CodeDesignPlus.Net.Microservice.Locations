using System;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.CreateCurrency;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Currency.Commands.CreateCurrency
{
    public class CreateCurrencyCommandTest
    {
        private readonly Validator validator;

        public CreateCurrencyCommandTest()
        {
            validator = new Validator();
        }

        [Fact]
        public void Should_Have_Error_When_Id_Is_Empty()
        {
            var command = new CreateCurrencyCommand(Guid.Empty, "USD", "USD", "$");
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var command = new CreateCurrencyCommand(Guid.NewGuid(), "", "USD", "$");
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Exceeds_MaxLength()
        {
            var command = new CreateCurrencyCommand(Guid.NewGuid(), new string('A', 101), "USD", "$");
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Code_Is_Empty()
        {
            var command = new CreateCurrencyCommand(Guid.NewGuid(), "USD", "", "$");
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void Should_Have_Error_When_Code_Exceeds_MaxLength()
        {
            var command = new CreateCurrencyCommand(Guid.NewGuid(), "USD", "USDX", "$");
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Command_Is_Valid()
        {
            var command = new CreateCurrencyCommand(Guid.NewGuid(), "USD", "USD", "$");
            var result = validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
