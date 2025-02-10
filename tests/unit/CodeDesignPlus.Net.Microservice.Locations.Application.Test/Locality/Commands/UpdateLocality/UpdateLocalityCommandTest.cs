using System;
using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.UpdateLocality;
using FluentValidation.TestHelper;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Locality.Commands.UpdateLocality;

public class UpdateLocalityCommandTest
{
    private readonly Validator validator;

    public UpdateLocalityCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new UpdateLocalityCommand(Guid.Empty, "Test Name", Guid.NewGuid(), true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new UpdateLocalityCommand(Guid.NewGuid(), string.Empty, Guid.NewGuid(), true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_Max_Length()
    {
        var command = new UpdateLocalityCommand(Guid.NewGuid(), new string('a', 129), Guid.NewGuid(), true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_IdCity_Is_Empty()
    {
        var command = new UpdateLocalityCommand(Guid.NewGuid(), "Test Name", Guid.Empty, true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.IdCity);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new UpdateLocalityCommand(Guid.NewGuid(), "Test Name", Guid.NewGuid(), true);
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
