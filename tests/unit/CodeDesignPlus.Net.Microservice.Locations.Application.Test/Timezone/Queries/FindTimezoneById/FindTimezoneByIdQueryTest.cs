using System;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Queries.FindTimezoneById;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Timezone.Queries.FindTimezoneById;

public class FindTimezoneByIdQueryTest
{
    private readonly Validator validator;

    public FindTimezoneByIdQueryTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        // Arrange
        var query = new FindTimezoneByIdQuery(Guid.Empty);

        // Act
        var result = validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Provided()
    {
        // Arrange
        var query = new FindTimezoneByIdQuery(Guid.NewGuid());

        // Act
        var result = validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
