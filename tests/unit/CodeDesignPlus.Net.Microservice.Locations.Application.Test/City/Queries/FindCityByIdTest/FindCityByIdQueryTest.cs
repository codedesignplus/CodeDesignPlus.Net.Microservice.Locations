using System;
using FluentValidation.TestHelper;
using Xunit;
using CodeDesignPlus.Net.Microservice.Locations.Application.City.Queries.FindCityById;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.City.Queries.FindCityByIdTest;

public class FindCityByIdQueryTest
{
    private readonly Validator validator;

    public FindCityByIdQueryTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var query = new FindCityByIdQuery(Guid.Empty);
        var result = validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Provided()
    {
        var query = new FindCityByIdQuery(Guid.NewGuid());
        var result = validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
