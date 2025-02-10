using System;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.DeleteCountry;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Country.Commands.DeleteCountry;

public class DeleteCountryCommandTest
{
    private readonly Validator validator;

    public DeleteCountryCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new DeleteCountryCommand(Guid.Empty);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Null()
    {
        var command = new DeleteCountryCommand(Guid.Empty);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        var command = new DeleteCountryCommand(Guid.NewGuid());
        var result = validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
