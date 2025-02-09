using System;
using Xunit;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.UpdateState;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.State.Commands.UpdateState;

public class UpdateStateCommandTest
{
    private readonly Validator _validator;

    public UpdateStateCommandTest()
    {
        _validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new UpdateStateCommand(Guid.Empty, Guid.NewGuid(), "ABC", "TestName", true);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_IdCountry_Is_Empty()
    {
        var command = new UpdateStateCommand(Guid.NewGuid(), Guid.Empty, "ABC", "TestName", true);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.IdCountry);
    }

    [Fact]
    public void Should_Have_Error_When_Code_Is_Empty()
    {
        var command = new UpdateStateCommand(Guid.NewGuid(), Guid.NewGuid(), "", "TestName", true);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Should_Have_Error_When_Code_Exceeds_MaxLength()
    {
        var command = new UpdateStateCommand(Guid.NewGuid(), Guid.NewGuid(), "ABCD", "TestName", true);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new UpdateStateCommand(Guid.NewGuid(), Guid.NewGuid(), "ABC", "", true);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_MaxLength()
    {
        var command = new UpdateStateCommand(Guid.NewGuid(), Guid.NewGuid(), "ABC", new string('A', 129), true);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new UpdateStateCommand(Guid.NewGuid(), Guid.NewGuid(), "ABC", "TestName", true);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
