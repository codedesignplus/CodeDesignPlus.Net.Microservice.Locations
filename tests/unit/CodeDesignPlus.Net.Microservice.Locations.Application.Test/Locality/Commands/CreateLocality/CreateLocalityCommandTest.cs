using System;
using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.CreateLocality;
using FluentValidation.TestHelper;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Locality.Commands.CreateLocality
{
    public class CreateLocalityCommandTest
    {
        private readonly Validator validator;

        public CreateLocalityCommandTest()
        {
            validator = new Validator();
        }

        [Fact]
        public void Should_Have_Error_When_Id_Is_Empty()
        {
            var command = new CreateLocalityCommand(Guid.Empty, "TestName", Guid.NewGuid());
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var command = new CreateLocalityCommand(Guid.NewGuid(), string.Empty, Guid.NewGuid());
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Exceeds_Maximum_Length()
        {
            var command = new CreateLocalityCommand(Guid.NewGuid(), new string('a', 101), Guid.NewGuid());
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_IdCity_Is_Empty()
        {
            var command = new CreateLocalityCommand(Guid.NewGuid(), "TestName", Guid.Empty);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.IdCity);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Command_Is_Valid()
        {
            var command = new CreateLocalityCommand(Guid.NewGuid(), "TestName", Guid.NewGuid());
            var result = validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
