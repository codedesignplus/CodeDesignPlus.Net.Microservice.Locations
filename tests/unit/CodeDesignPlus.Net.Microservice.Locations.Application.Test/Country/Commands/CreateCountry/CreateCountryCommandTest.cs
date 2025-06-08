using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.CreateCountry;
using FluentValidation.TestHelper;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Country.Commands.CreateCountry;

public class CreateCountryCommandTest
{
    private readonly Validator validator;

    public CreateCountryCommandTest()
    {
        validator = new Validator();
    }


    [Fact]
    public void Validator_Should_Pass_For_Valid_Command()
    {
        var command = new CreateCountryCommand(
            Guid.NewGuid(),
            "Colombia",
            "CO",
            "COL",
            "170",
            "Bogotá",
            Guid.NewGuid(),
            "America/Bogota",
            "Colombia",
            "Americas",
            "South America",
            4.5709,
            -74.2973,
            "🇨🇴",
            true
        );

        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_Should_Fail_When_Required_Fields_Are_Empty()
    {
        var command = new CreateCountryCommand(
            Guid.Empty,
            "",
            "",
            "",
            "",
            null,
            Guid.Empty,
            "",
            "",
            "",
            "",
            0,
            0,
            null,
            true
        );

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Id);
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Alpha2);
        result.ShouldHaveValidationErrorFor(x => x.Alpha3);
        result.ShouldHaveValidationErrorFor(x => x.Code);
        result.ShouldHaveValidationErrorFor(x => x.IdCurrency);
        result.ShouldHaveValidationErrorFor(x => x.Timezone);
        result.ShouldHaveValidationErrorFor(x => x.NameNative);
        result.ShouldHaveValidationErrorFor(x => x.Region);
        result.ShouldHaveValidationErrorFor(x => x.SubRegion);
        result.ShouldHaveValidationErrorFor(x => x.Latitude);
        result.ShouldHaveValidationErrorFor(x => x.Longitude);
    }

    [Fact]
    public void Validator_Should_Fail_When_Fields_Exceed_Max_Length()
    {
        var command = new CreateCountryCommand(
            Guid.NewGuid(),
            new string('A', 101),
            "ABC",
            "ABCD",
            "123",
            new string('B', 101),
            Guid.NewGuid(),
            new string('C', 101),
            new string('D', 101),
            new string('E', 101),
            new string('F', 101),
            1,
            1,
            new string('G', 101),
            true
        );

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Alpha2);
        result.ShouldHaveValidationErrorFor(x => x.Alpha3);
        result.ShouldHaveValidationErrorFor(x => x.Capital);
        result.ShouldHaveValidationErrorFor(x => x.Timezone);
        result.ShouldHaveValidationErrorFor(x => x.NameNative);
        result.ShouldHaveValidationErrorFor(x => x.Region);
        result.ShouldHaveValidationErrorFor(x => x.SubRegion);
        result.ShouldHaveValidationErrorFor(x => x.Flag);
    }
}