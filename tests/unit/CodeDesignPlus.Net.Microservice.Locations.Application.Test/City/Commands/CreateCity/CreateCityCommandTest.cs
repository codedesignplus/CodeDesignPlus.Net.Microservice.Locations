using System;
using CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.CreateCity;
using FluentValidation.TestHelper;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.City.Commands.CreateCity;

public class CreateCityCommandTest
{
    private readonly Validator _validator;

    public CreateCityCommandTest()
    {
        _validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new CreateCityCommand(Guid.Empty, Guid.NewGuid(), "CityName", "TimeZone");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_IdState_Is_Empty()
    {
        var command = new CreateCityCommand(Guid.NewGuid(), Guid.Empty, "CityName", "TimeZone");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.IdState);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new CreateCityCommand(Guid.NewGuid(), Guid.NewGuid(), string.Empty, "TimeZone");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_MaxLength()
    {
        var command = new CreateCityCommand(Guid.NewGuid(), Guid.NewGuid(), new string('a', 101), "TimeZone");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_TimeZone_Is_Empty()
    {
        var command = new CreateCityCommand(Guid.NewGuid(), Guid.NewGuid(), "CityName", string.Empty);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.TimeZone);
    }

    [Fact]
    public void Should_Have_Error_When_TimeZone_Exceeds_MaxLength()
    {
        var command = new CreateCityCommand(Guid.NewGuid(), Guid.NewGuid(), "CityName", new string('a', 101));
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.TimeZone);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new CreateCityCommand(Guid.NewGuid(), Guid.NewGuid(), "CityName", "TimeZone");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
