using System;
using FluentValidation.TestHelper;
using Xunit;
using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Queries.FindLocalityById;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Locality.Queries.FindLocalityById;

public class FindLocalityByIdQueryTest
{
    private readonly Validator validator;

    public FindLocalityByIdQueryTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var query = new FindLocalityByIdQuery(Guid.Empty);
        var result = validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Not_Empty()
    {
        var query = new FindLocalityByIdQuery(Guid.NewGuid());
        var result = validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
