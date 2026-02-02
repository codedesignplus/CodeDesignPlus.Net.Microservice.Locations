using System;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.CreateCurrency;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Currency.Commands.CreateCurrency
{
    public class CreateCurrencyCommandTest
    {
        [Fact]
        public void Validator_Should_Have_Error_When_Id_Is_Empty()
        {
            var validator = new Validator();
            var command = new CreateCurrencyCommand(Guid.Empty, "Dollar", "USD", 840, 2, "$");
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Name_Is_Empty_Or_Too_Long()
        {
            var validator = new Validator();
            var commandEmpty = new CreateCurrencyCommand(Guid.NewGuid(), "", "USD", 840, 2, "$");
            var resultEmpty = validator.TestValidate(commandEmpty);
            resultEmpty.ShouldHaveValidationErrorFor(x => x.Name);

            var longName = new string('A', 101);
            var commandLong = new CreateCurrencyCommand(Guid.NewGuid(), longName, "USD", 840, 2, "$");
            var resultLong = validator.TestValidate(commandLong);
            resultLong.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData("US")]
        [InlineData("USDE")]
        [InlineData("usd")]
        [InlineData("UsD")]
        public void Validator_Should_Have_Error_When_Code_Is_Invalid(string code)
        {
            var validator = new Validator();
            var command = new CreateCurrencyCommand(Guid.NewGuid(), "Dollar", code, 840, 2, "$");
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1000)]
        public void Validator_Should_Have_Error_When_NumericCode_Is_Out_Of_Range(short numericCode)
        {
            var validator = new Validator();
            var command = new CreateCurrencyCommand(Guid.NewGuid(), "Dollar", "USD", numericCode, 2, "$");
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.NumericCode);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(5)]
        public void Validator_Should_Have_Error_When_DecimalDigits_Is_Out_Of_Range(short decimalDigits)
        {
            var validator = new Validator();
            var command = new CreateCurrencyCommand(Guid.NewGuid(), "Dollar", "USD", 840, decimalDigits, "$");
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.DecimalDigits);
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Symbol_Is_Empty_Or_Too_Long()
        {
            var validator = new Validator();
            var commandEmpty = new CreateCurrencyCommand(Guid.NewGuid(), "Dollar", "USD", 840, 2, "");
            var resultEmpty = validator.TestValidate(commandEmpty);
            resultEmpty.ShouldHaveValidationErrorFor(x => x.Symbol);

            var longSymbol = new string('$', 11);
            var commandLong = new CreateCurrencyCommand(Guid.NewGuid(), "Dollar", "USD", 840, 2, longSymbol);
            var resultLong = validator.TestValidate(commandLong);
            resultLong.ShouldHaveValidationErrorFor(x => x.Symbol);
        }

        [Fact]
        public void Validator_Should_Not_Have_Error_For_Valid_Command()
        {
            var validator = new Validator();
            var command = new CreateCurrencyCommand(Guid.NewGuid(), "Dollar", "USD", 840, 2, "$");
            var result = validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

    }
}