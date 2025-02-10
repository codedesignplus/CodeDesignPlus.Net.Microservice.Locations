using System;
using Xunit;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Queries.GetCountryById;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Country.Queries.GetCountryById;

public class GetCountryByIdQueryTest
{
    private readonly Validator validator;

    public GetCountryByIdQueryTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var query = new GetCountryByIdQuery(Guid.Empty);
        var result = validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Provided()
    {
        var query = new GetCountryByIdQuery(Guid.NewGuid());
        var result = validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
