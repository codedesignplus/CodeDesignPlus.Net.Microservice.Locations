using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Queries.GetAllCountry;

public record GetAllCountryQuery(C.Criteria Criteria) : IRequest<Pagination<CountryDto>>;

