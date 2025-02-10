using System;
using Xunit;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.CreateNeighborhood;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Neighborhood.Commands.CreateNeighborhood;

public class CreateNeighborhoodCommandTest
{
    private readonly Validator validator;

    public CreateNeighborhoodCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new CreateNeighborhoodCommand(Guid.Empty, "ValidName", Guid.NewGuid());
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new CreateNeighborhoodCommand(Guid.NewGuid(), string.Empty, Guid.NewGuid());
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Too_Long()
    {
        var command = new CreateNeighborhoodCommand(Guid.NewGuid(), new string('a', 129), Guid.NewGuid());
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_IdLocality_Is_Empty()
    {
        var command = new CreateNeighborhoodCommand(Guid.NewGuid(), "ValidName", Guid.Empty);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.IdLocality);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new CreateNeighborhoodCommand(Guid.NewGuid(), "ValidName", Guid.NewGuid());
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
