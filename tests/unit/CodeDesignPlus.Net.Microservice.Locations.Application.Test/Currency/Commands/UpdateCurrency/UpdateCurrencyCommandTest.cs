using System;
using Xunit;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.UpdateCurrency;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Currency.Commands.UpdateCurrency;

public class UpdateCurrencyCommandTest
{
    private readonly Validator validator;

    public UpdateCurrencyCommandTest()
    {
        validator = new Validator();
    }


    [Fact]
    public void Should_Pass_Validation_For_Valid_Command()
    {
        var command = new UpdateCurrencyCommand(
            Guid.NewGuid(),
            "US Dollar",
            "USD",
            840,
            2,
            "$",
            true
        );

        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new UpdateCurrencyCommand(
            Guid.Empty,
            "US Dollar",
            "USD",
            840,
            2,
            "$",
            true
        );

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new UpdateCurrencyCommand(
            Guid.NewGuid(),
            "",
            "USD",
            840,
            2,
            "$",
            true
        );

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_Max_Length()
    {
        var command = new UpdateCurrencyCommand(
            Guid.NewGuid(),
            new string('A', 101),
            "USD",
            840,
            2,
            "$",
            true
        );

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Code_Is_Empty()
    {
        var command = new UpdateCurrencyCommand(
            Guid.NewGuid(),
            "US Dollar",
            "",
            840,
            2,
            "$",
            true
        );

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Should_Have_Error_When_Code_Is_Not_Three_Characters()
    {
        var command = new UpdateCurrencyCommand(
            Guid.NewGuid(),
            "US Dollar",
            "US",
            840,
            2,
            "$",
            true
        );

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Should_Have_Error_When_Code_Is_Not_Uppercase()
    {
        var command = new UpdateCurrencyCommand(
            Guid.NewGuid(),
            "US Dollar",
            "usd",
            840,
            2,
            "$",
            true
        );

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1000)]
    public void Should_Have_Error_When_NumericCode_Is_Out_Of_Range(short numericCode)
    {
        var command = new UpdateCurrencyCommand(
            Guid.NewGuid(),
            "US Dollar",
            "USD",
            numericCode,
            2,
            "$",
            true
        );

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.NumericCode);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(5)]
    public void Should_Have_Error_When_DecimalDigits_Is_Out_Of_Range(short decimalDigits)
    {
        var command = new UpdateCurrencyCommand(
            Guid.NewGuid(),
            "US Dollar",
            "USD",
            840,
            decimalDigits,
            "$",
            true
        );

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.DecimalDigits);
    }

    [Fact]
    public void Should_Have_Error_When_Symbol_Is_Empty()
    {
        var command = new UpdateCurrencyCommand(
            Guid.NewGuid(),
            "US Dollar",
            "USD",
            840,
            2,
            "",
            true
        );

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Symbol);
    }

    [Fact]
    public void Should_Have_Error_When_Symbol_Exceeds_Max_Length()
    {
        var command = new UpdateCurrencyCommand(
            Guid.NewGuid(),
            "US Dollar",
            "USD",
            840,
            2,
            new string('$', 11),
            true
        );

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Symbol);
    }

}
