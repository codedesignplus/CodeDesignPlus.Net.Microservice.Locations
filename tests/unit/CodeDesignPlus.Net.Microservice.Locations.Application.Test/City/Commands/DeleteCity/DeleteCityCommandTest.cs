using System;
using FluentValidation.TestHelper;
using Xunit;
using CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.DeleteCity;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.City.Commands.DeleteCity;

public class DeleteCityCommandTest
{
    private readonly Validator _validator;

    public DeleteCityCommandTest()
    {
        _validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new DeleteCityCommand(Guid.Empty);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        var command = new DeleteCityCommand(Guid.NewGuid());
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
