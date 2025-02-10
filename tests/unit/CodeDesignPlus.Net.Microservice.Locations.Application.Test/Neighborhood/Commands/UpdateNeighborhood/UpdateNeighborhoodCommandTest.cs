using System;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.UpdateNeighborhood;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Neighborhood.Commands.UpdateNeighborhood;

public class UpdateNeighborhoodCommandTest
{
    private readonly Validator validator;

    public UpdateNeighborhoodCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new UpdateNeighborhoodCommand(Guid.Empty, "ValidName", Guid.NewGuid(), true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new UpdateNeighborhoodCommand(Guid.NewGuid(), string.Empty, Guid.NewGuid(), true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Null()
    {
        var command = new UpdateNeighborhoodCommand(Guid.NewGuid(), null, Guid.NewGuid(), true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_Maximum_Length()
    {
        var command = new UpdateNeighborhoodCommand(Guid.NewGuid(), new string('a', 129), Guid.NewGuid(), true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_IdLocality_Is_Empty()
    {
        var command = new UpdateNeighborhoodCommand(Guid.NewGuid(), "ValidName", Guid.Empty, true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.IdLocality);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new UpdateNeighborhoodCommand(Guid.NewGuid(), "ValidName", Guid.NewGuid(), true);
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
