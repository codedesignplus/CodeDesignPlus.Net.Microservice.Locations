namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Queries.FindCityById;

public record FindCityByIdQuery(Guid Id) : IRequest<CityDto>;

public class Validator : AbstractValidator<FindCityByIdQuery>
{
    public Validator()
    {
        this.RuleFor(x => x.Id).NotEmpty();
    }
}