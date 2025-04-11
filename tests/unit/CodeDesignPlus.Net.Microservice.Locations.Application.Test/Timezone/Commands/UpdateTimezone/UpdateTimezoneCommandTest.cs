using System;
using Xunit;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.UpdateTimezone;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Timezone.Commands.UpdateTimezone
{
    public class UpdateTimezoneCommandTest
    {
        private readonly Validator validator;

        public UpdateTimezoneCommandTest()
        {
            validator = new Validator();
        }

        [Fact]
        public void Should_Have_Error_When_Id_Is_Empty()
        {
            var command = new UpdateTimezoneCommand(Guid.Empty, "Valid Name", true);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var command = new UpdateTimezoneCommand(Guid.NewGuid(), string.Empty, true);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Null()
        {
            var command = new UpdateTimezoneCommand(Guid.NewGuid(), null!, true);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Exceeds_Maximum_Length()
        {
            var command = new UpdateTimezoneCommand(Guid.NewGuid(), new string('a', 129), true);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Command_Is_Valid()
        {
            var command = new UpdateTimezoneCommand(Guid.NewGuid(), "Valid Name", true);
            var result = validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
