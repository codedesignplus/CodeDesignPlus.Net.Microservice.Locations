using System;
using Xunit;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.UpdateCity;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.City.Commands.UpdateCityTest;

public class UpdateCityCommandTest
{
    private readonly Validator validator;

    public UpdateCityCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new UpdateCityCommand(Guid.Empty, Guid.NewGuid(), "CityName", "Timezone", true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_IdState_Is_Empty()
    {
        var command = new UpdateCityCommand(Guid.NewGuid(), Guid.Empty, "CityName", "Timezone", true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.IdState);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new UpdateCityCommand(Guid.NewGuid(), Guid.NewGuid(), string.Empty, "Timezone", true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_MaxLength()
    {
        var command = new UpdateCityCommand(Guid.NewGuid(), Guid.NewGuid(), new string('a', 101), "Timezone", true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Timezone_Is_Empty()
    {
        var command = new UpdateCityCommand(Guid.NewGuid(), Guid.NewGuid(), "CityName", string.Empty, true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Timezone);
    }

    [Fact]
    public void Should_Have_Error_When_Timezone_Exceeds_MaxLength()
    {
        var command = new UpdateCityCommand(Guid.NewGuid(), Guid.NewGuid(), "CityName", new string('a', 101), true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Timezone);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new UpdateCityCommand(Guid.NewGuid(), Guid.NewGuid(), "CityName", "Timezone", true);
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
