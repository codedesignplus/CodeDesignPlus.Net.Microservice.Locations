using System;
using FluentValidation.TestHelper;
using Xunit;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.DeleteCurrency;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Currency.Commands.DeleteCurrency;

public class DeleteCurrencyCommandTest
{
    private readonly Validator _validator;

    public DeleteCurrencyCommandTest()
    {
        _validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new DeleteCurrencyCommand(Guid.Empty);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Null()
    {
        var command = new DeleteCurrencyCommand(Guid.Empty);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        var command = new DeleteCurrencyCommand(Guid.NewGuid());
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
