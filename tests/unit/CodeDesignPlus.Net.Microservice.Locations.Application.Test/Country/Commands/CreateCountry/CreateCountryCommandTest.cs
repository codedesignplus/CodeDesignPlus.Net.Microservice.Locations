using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.CreateCountry;
using FluentValidation.TestHelper;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Country.Commands.CreateCountry;

public class CreateCountryCommandTest
{
    private readonly Validator _validator;

    public CreateCountryCommandTest()
    {
        _validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new CreateCountryCommand(Guid.Empty, "Name", "AL", "ALB", 1, "Capital", Guid.NewGuid(), "TimeZone");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new CreateCountryCommand(Guid.NewGuid(), "", "AL", "ALB", 1, "Capital", Guid.NewGuid(), "TimeZone");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Alpha2_Is_Empty()
    {
        var command = new CreateCountryCommand(Guid.NewGuid(), "Name", "", "ALB", 1, "Capital", Guid.NewGuid(), "TimeZone");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Alpha2);
    }

    [Fact]
    public void Should_Have_Error_When_Alpha3_Is_Empty()
    {
        var command = new CreateCountryCommand(Guid.NewGuid(), "Name", "AL", "", 1, "Capital", Guid.NewGuid(), "TimeZone");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Alpha3);
    }

    [Fact]
    public void Should_Have_Error_When_Code_Is_Zero()
    {
        var command = new CreateCountryCommand(Guid.NewGuid(), "Name", "AL", "ALB", 0, "Capital", Guid.NewGuid(), "TimeZone");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Should_Have_Error_When_Capital_Exceeds_MaxLength()
    {
        var command = new CreateCountryCommand(Guid.NewGuid(), "Name", "AL", "ALB", 1, new string('A', 101), Guid.NewGuid(), "TimeZone");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Capital);
    }

    [Fact]
    public void Should_Have_Error_When_IdCurrency_Is_Empty()
    {
        var command = new CreateCountryCommand(Guid.NewGuid(), "Name", "AL", "ALB", 1, "Capital", Guid.Empty, "TimeZone");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.IdCurrency);
    }

    [Fact]
    public void Should_Have_Error_When_TimeZone_Is_Empty()
    {
        var command = new CreateCountryCommand(Guid.NewGuid(), "Name", "AL", "ALB", 1, "Capital", Guid.NewGuid(), "");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.TimeZone);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new CreateCountryCommand(Guid.NewGuid(), "Name", "AL", "ALB", 1, "Capital", Guid.NewGuid(), "TimeZone");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
