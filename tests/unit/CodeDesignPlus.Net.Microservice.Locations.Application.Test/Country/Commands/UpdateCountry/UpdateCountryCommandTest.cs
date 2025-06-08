using System;
using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.UpdateCountry;
using FluentValidation.TestHelper;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Country.Commands.UpdateCountry;

public class UpdateCountryCommandTest
{
    private readonly Validator validator;

    public UpdateCountryCommandTest()
    {
        validator = new Validator();
    }


    [Fact]
    public void Validator_Should_Pass_For_Valid_Command()
    {
        var command = new UpdateCountryCommand(
            Id: Guid.NewGuid(),
            Name: "Colombia",
            Alpha2: "CO",
            Alpha3: "COL",
            Code: "170",
            Capital: "Bogotá",
            IdCurrency: Guid.NewGuid(),
            Timezone: "America/Bogota",
            NameNative: "Colombia",
            Region: "Americas",
            SubRegion: "South America",
            Latitude: 4.5709,
            Longitude: -74.2973,
            Flag: "🇨🇴",
            IsActive: true
        );

        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_Should_Fail_When_Required_Fields_Are_Empty()
    {
        var command = new UpdateCountryCommand(
            Id: Guid.Empty,
            Name: "",
            Alpha2: "",
            Alpha3: "",
            Code: "",
            Capital: null,
            IdCurrency: Guid.Empty,
            Timezone: "",
            NameNative: "",
            Region: "",
            SubRegion: "",
            Latitude: 0,
            Longitude: 0,
            Flag: null,
            IsActive: false
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
    public void Validator_Should_Fail_When_Alpha2_Is_Not_Length_2()
    {
        var command = new UpdateCountryCommand(
            Id: Guid.NewGuid(),
            Name: "Colombia",
            Alpha2: "C",
            Alpha3: "COL",
            Code: "170",
            Capital: "Bogotá",
            IdCurrency: Guid.NewGuid(),
            Timezone: "America/Bogota",
            NameNative: "Colombia",
            Region: "Americas",
            SubRegion: "South America",
            Latitude: 4.5709,
            Longitude: -74.2973,
            Flag: "🇨🇴",
            IsActive: true
        );

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Alpha2);
    }

    [Fact]
    public void Validator_Should_Fail_When_Alpha3_Is_Not_Length_3()
    {
        var command = new UpdateCountryCommand(
            Id: Guid.NewGuid(),
            Name: "Colombia",
            Alpha2: "CO",
            Alpha3: "CO",
            Code: "170",
            Capital: "Bogotá",
            IdCurrency: Guid.NewGuid(),
            Timezone: "America/Bogota",
            NameNative: "Colombia",
            Region: "Americas",
            SubRegion: "South America",
            Latitude: 4.5709,
            Longitude: -74.2973,
            Flag: "🇨🇴",
            IsActive: true
        );

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Alpha3);
    }

    [Fact]
    public void Validator_Should_Fail_When_Name_Exceeds_MaxLength()
    {
        var command = new UpdateCountryCommand(
            Id: Guid.NewGuid(),
            Name: new string('A', 101),
            Alpha2: "CO",
            Alpha3: "COL",
            Code: "170",
            Capital: "Bogotá",
            IdCurrency: Guid.NewGuid(),
            Timezone: "America/Bogota",
            NameNative: "Colombia",
            Region: "Americas",
            SubRegion: "South America",
            Latitude: 4.5709,
            Longitude: -74.2973,
            Flag: "🇨🇴",
            IsActive: true
        );

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validator_Should_Fail_When_Capital_Exceeds_MaxLength()
    {
        var command = new UpdateCountryCommand(
            Id: Guid.NewGuid(),
            Name: "Colombia",
            Alpha2: "CO",
            Alpha3: "COL",
            Code: "170",
            Capital: new string('B', 101),
            IdCurrency: Guid.NewGuid(),
            Timezone: "America/Bogota",
            NameNative: "Colombia",
            Region: "Americas",
            SubRegion: "South America",
            Latitude: 4.5709,
            Longitude: -74.2973,
            Flag: "🇨🇴",
            IsActive: true
        );

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Capital);
    }

    [Fact]
    public void Validator_Should_Fail_When_Flag_Exceeds_MaxLength()
    {
        var command = new UpdateCountryCommand(
            Id: Guid.NewGuid(),
            Name: "Colombia",
            Alpha2: "CO",
            Alpha3: "COL",
            Code: "170",
            Capital: "Bogotá",
            IdCurrency: Guid.NewGuid(),
            Timezone: "America/Bogota",
            NameNative: "Colombia",
            Region: "Americas",
            SubRegion: "South America",
            Latitude: 4.5709,
            Longitude: -74.2973,
            Flag: new string('F', 101),
            IsActive: true
        );

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Flag);
    }
}