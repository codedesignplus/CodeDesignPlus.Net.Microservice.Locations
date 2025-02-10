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
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new UpdateCurrencyCommand(Guid.Empty, "USD", "USD", "$", true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new UpdateCurrencyCommand(Guid.NewGuid(), "", "USD", "$", true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_MaxLength()
    {
        var command = new UpdateCurrencyCommand(Guid.NewGuid(), new string('a', 101), "USD", "$", true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Code_Is_Empty()
    {
        var command = new UpdateCurrencyCommand(Guid.NewGuid(), "USD", "", "$", true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Should_Have_Error_When_Code_Exceeds_MaxLength()
    {
        var command = new UpdateCurrencyCommand(Guid.NewGuid(), "USD", "USDE", "$", true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new UpdateCurrencyCommand(Guid.NewGuid(), "USD", "USD", "$", true);
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
