using System;
using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.UpdateCountry;
using FluentValidation.TestHelper;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Country.Commands.UpdateCountry
{
    public class UpdateCountryCommandTest
    {
        private readonly Validator _validator;

        public UpdateCountryCommandTest()
        {
            _validator = new Validator();
        }

        [Fact]
        public void Should_Have_Error_When_Id_Is_Empty()
        {
            var command = new UpdateCountryCommand(Guid.Empty, "Name", "US", "USA", 1, "Capital", Guid.NewGuid(), "TimeZone", true);
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var command = new UpdateCountryCommand(Guid.NewGuid(), "", "US", "USA", 1, "Capital", Guid.NewGuid(), "TimeZone", true);
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Exceeds_MaxLength()
        {
            var command = new UpdateCountryCommand(Guid.NewGuid(), new string('a', 101), "US", "USA", 1, "Capital", Guid.NewGuid(), "TimeZone", true);
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Alpha2_Is_Invalid()
        {
            var command = new UpdateCountryCommand(Guid.NewGuid(), "Name", "U", "USA", 1, "Capital", Guid.NewGuid(), "TimeZone", true);
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Alpha2);
        }

        [Fact]
        public void Should_Have_Error_When_Alpha3_Is_Invalid()
        {
            var command = new UpdateCountryCommand(Guid.NewGuid(), "Name", "US", "US", 1, "Capital", Guid.NewGuid(), "TimeZone", true);
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Alpha3);
        }

        [Fact]
        public void Should_Have_Error_When_Code_Is_Zero()
        {
            var command = new UpdateCountryCommand(Guid.NewGuid(), "Name", "US", "USA", 0, "Capital", Guid.NewGuid(), "TimeZone", true);
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void Should_Have_Error_When_Capital_Exceeds_MaxLength()
        {
            var command = new UpdateCountryCommand(Guid.NewGuid(), "Name", "US", "USA", 1, new string('a', 101), Guid.NewGuid(), "TimeZone", true);
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Capital);
        }

        [Fact]
        public void Should_Have_Error_When_IdCurrency_Is_Empty()
        {
            var command = new UpdateCountryCommand(Guid.NewGuid(), "Name", "US", "USA", 1, "Capital", Guid.Empty, "TimeZone", true);
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.IdCurrency);
        }
    }
}
