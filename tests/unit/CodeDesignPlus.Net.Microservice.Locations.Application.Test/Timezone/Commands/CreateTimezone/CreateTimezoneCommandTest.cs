using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.CreateTimezone;
using CodeDesignPlus.Net.Microservice.Locations.Domain.ValueObjects;
using FluentValidation.TestHelper;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Timezone.Commands.CreateTimezone;

public class CreateTimezoneCommandTest
{
    private readonly Validator validator;

    public CreateTimezoneCommandTest()
    {
        validator = new Validator();
    }

    private static CreateTimezoneCommand CreateValidCommand()
    {
        return new CreateTimezoneCommand(
            Guid.NewGuid(),
            "Test Timezone",
            ["Alias1", "Alias2"],
            Location.Create("CO", "Colombia", 1.0, 2.0),
            ["+01:00", "+02:00"],
            "+01:00",
            true
        );
    }

    [Fact]
    public void Should_Pass_Validation_For_Valid_Command()
    {
        var command = CreateValidCommand();
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = CreateValidCommand() with { Id = Guid.Empty };
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Null_Or_Empty()
    {
        var command1 = CreateValidCommand() with { Name = null! };
        var command2 = CreateValidCommand() with { Name = "" };
        validator.TestValidate(command1).ShouldHaveValidationErrorFor(x => x.Name);
        validator.TestValidate(command2).ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_MaxLength()
    {
        var longName = new string('a', 129);
        var command = CreateValidCommand() with { Name = longName };
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Aliases_Is_Null()
    {
        var command1 = CreateValidCommand() with { Aliases = null! };
        validator.TestValidate(command1).ShouldHaveValidationErrorFor(x => x.Aliases);
    }

    [Fact]
    public void Should_Have_Error_When_Location_Is_Null()
    {
        var command = CreateValidCommand() with { Location = null! };
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Location);
    }

    [Fact]
    public void Should_Have_Error_When_Offsets_Is_Null_Or_Empty()
    {
        var command1 = CreateValidCommand() with { Offsets = null! };
        var command2 = CreateValidCommand() with { Offsets = new List<string>() };
        validator.TestValidate(command1).ShouldHaveValidationErrorFor(x => x.Offsets);
        validator.TestValidate(command2).ShouldHaveValidationErrorFor(x => x.Offsets);
    }

    [Fact]
    public void Should_Have_Error_When_CurrentOffset_Is_Null_Or_Empty()
    {
        var command1 = CreateValidCommand() with { CurrentOffset = null! };
        var command2 = CreateValidCommand() with { CurrentOffset = "" };
        validator.TestValidate(command1).ShouldHaveValidationErrorFor(x => x.CurrentOffset);
        validator.TestValidate(command2).ShouldHaveValidationErrorFor(x => x.CurrentOffset);
    }
}
