using System;
using Xunit;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.CreateState;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.State.Commands.CreateState;

public class CreateStateCommandTest
{
    private readonly Validator _validator;

    public CreateStateCommandTest()
    {
        _validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new CreateStateCommand(Guid.Empty, Guid.NewGuid(), "ABC", "Test State");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_IdCountry_Is_Empty()
    {
        var command = new CreateStateCommand(Guid.NewGuid(), Guid.Empty, "ABC", "Test State");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.IdCountry);
    }

    [Fact]
    public void Should_Have_Error_When_Code_Is_Empty()
    {
        var command = new CreateStateCommand(Guid.NewGuid(), Guid.NewGuid(), string.Empty, "Test State");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Should_Have_Error_When_Code_Exceeds_MaxLength()
    {
        var command = new CreateStateCommand(Guid.NewGuid(), Guid.NewGuid(), "ABCD", "Test State");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new CreateStateCommand(Guid.NewGuid(), Guid.NewGuid(), "ABC", string.Empty);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_MaxLength()
    {
        var command = new CreateStateCommand(Guid.NewGuid(), Guid.NewGuid(), "ABC", new string('A', 129));
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new CreateStateCommand(Guid.NewGuid(), Guid.NewGuid(), "ABC", "Test State");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
