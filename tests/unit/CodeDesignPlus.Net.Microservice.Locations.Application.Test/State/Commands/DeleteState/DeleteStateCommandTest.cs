using System;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.DeleteState;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.State.Commands.DeleteState;

public class DeleteStateCommandTest
{
    private readonly Validator validator;

    public DeleteStateCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new DeleteStateCommand(Guid.Empty);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        var command = new DeleteStateCommand(Guid.NewGuid());
        var result = validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
