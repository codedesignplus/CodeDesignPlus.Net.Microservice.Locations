using System;
using CodeDesignPlus.Microservice.Api.Dtos;
using CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.CreateCity;
using CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.DeleteCity;
using CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.UpdateCity;
using CodeDesignPlus.Net.Microservice.Locations.Application.City.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.CreateCountry;
using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.DeleteCountry;
using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.UpdateCountry;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.CreateCurrency;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.DeleteCurrency;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.UpdateCurrency;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.CreateLocality;
using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.DeleteLocality;
using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.UpdateLocality;
using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.CreateNeighborhood;
using CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.DeleteNeighborhood;
using CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.UpdateNeighborhood;
using CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.CreateState;
using CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.DeleteState;
using CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.UpdateState;
using CodeDesignPlus.Net.Microservice.Locations.Application.State.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.CreateTimezone;
using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.UpdateTimezone;
using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.DataTransferObjects;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;


public class FakeData
{
    public readonly CurrencyDto Currency;
    public readonly CountryDto Country;
    public readonly StateDto State;
    public readonly CityDto City;
    public readonly LocalityDto Locality;
    public readonly NeighborhoodDto Neighborhood;
    public readonly TimezoneDto Timezone;

    public readonly CreateCurrencyDto CreateCurrency;
    public readonly CreateCountryDto CreateCountry;
    public readonly CreateStateDto CreateState;
    public readonly CreateCityDto CreateCity;
    public readonly CreateLocalityDto CreateLocality;
    public readonly CreateNeighborhoodDto CreateNeighborhood;
    public readonly CreateTimezoneDto CreateTimezone;

    public readonly UpdateCurrencyDto UpdateCurrency;
    public readonly UpdateCountryDto UpdateCountry;
    public readonly UpdateStateDto UpdateState;
    public readonly UpdateCityDto UpdateCity;
    public readonly UpdateLocalityDto UpdateLocality;
    public readonly UpdateNeighborhoodDto UpdateNeighborhood;
    public readonly UpdateTimezoneDto UpdateTimezone;

    public readonly CreateCurrencyCommand CreateCurrencyCommand;
    public readonly CreateCountryCommand CreateCountryCommand;
    public readonly CreateStateCommand CreateStateCommand;
    public readonly CreateCityCommand CreateCityCommand;
    public readonly CreateLocalityCommand CreateLocalityCommand;
    public readonly CreateNeighborhoodCommand CreateNeighborhoodCommand;
    public readonly CreateTimezoneCommand CreateTimezoneCommand;

    public readonly UpdateCurrencyCommand UpdateCurrencyCommand;
    public readonly UpdateCountryCommand UpdateCountryCommand;
    public readonly UpdateStateCommand UpdateStateCommand;
    public readonly UpdateCityCommand UpdateCityCommand;
    public readonly UpdateLocalityCommand UpdateLocalityCommand;
    public readonly UpdateNeighborhoodCommand UpdateNeighborhoodCommand;
    public readonly UpdateTimezoneCommand UpdateTimezoneCommand;

    public readonly DeleteCurrencyCommand DeleteCurrencyCommand;
    public readonly DeleteCountryCommand DeleteCountryCommand;
    public readonly DeleteStateCommand DeleteStateCommand;
    public readonly DeleteCityCommand DeleteCityCommand;
    public readonly DeleteLocalityCommand DeleteLocalityCommand;
    public readonly DeleteNeighborhoodCommand DeleteNeighborhoodCommand;

    public readonly CurrencyAggregate CurrencyAggregate;
    public readonly CountryAggregate CountryAggregate;
    public readonly StateAggregate StateAggregate;
    public readonly CityAggregate CityAggregate;
    public readonly LocalityAggregate LocalityAggregate;
    public readonly NeighborhoodAggregate NeighborhoodAggregate;
    public readonly TimezoneAggregate TimezoneAggregate;



    public FakeData()
    {

        Currency = new()
        {
            Id = Guid.NewGuid(),
            Name = "Dollar",
            Code = "USD",
            Symbol = "$"
        };

        Country = new()
        {
            Id = Guid.NewGuid(),
            Name = "United States",
            Alpha2 = "US",
            Alpha3 = "USA",
            Capital = "Washington",
            IdCurrency = Currency.Id,
            Code = 103,
            TimeZone = "America/New_York",
        };

        State = new()
        {
            Id = Guid.NewGuid(),
            Name = "New York",
            Code = "NY",
            IdCountry = Country.Id
        };

        City = new()
        {
            Id = Guid.NewGuid(),
            Name = "New York City",
            TimeZone = "America/New_York",
            IdState = State.Id
        };

        Locality = new()
        {
            Id = Guid.NewGuid(),
            Name = "Manhattan",
            IdCity = City.Id
        };

        Neighborhood = new()
        {
            Id = Guid.NewGuid(),
            Name = "Upper East Side",
            IdLocality = Locality.Id
        };

        Timezone = new()
        {
            Id = Guid.NewGuid(),
            Name = "America/New_York"
        };


        CreateCurrency = new()
        {
            Id = Currency.Id,
            Name = "Dollar",
            Code = "USD",
            Symbol = "$"
        };


        CreateCountry = new()
        {
            Id = Country.Id,
            Name = Country.Name,
            Alpha2 = Country.Alpha2,
            Alpha3 = Country.Alpha3,
            Capital = Country.Capital,
            IdCurrency = Country.IdCurrency,
            Code = Country.Code,
            TimeZone = Country.TimeZone
        };

        CreateState = new()
        {
            Id = State.Id,
            Name = State.Name,
            Code = State.Code,
            IdCountry = State.IdCountry
        };

        CreateCity = new()
        {
            Id = City.Id,
            Name = City.Name,
            TimeZone = City.TimeZone,
            IdState = City.IdState
        };

        CreateLocality = new()
        {
            Name = Locality.Name,
            Id = Locality.Id,
            IdCity = Locality.IdCity
        };

        CreateNeighborhood = new()
        {
            Name = Neighborhood.Name,
            Id = Neighborhood.Id,
            IdLocality = Neighborhood.IdLocality
        };

        CreateTimezone = new()
        {
            Id = Timezone.Id,
            Name = Timezone.Name
        };

        UpdateCurrency = new()
        {
            Id = Currency.Id,
            Name = "Pesos",
            Code = "COP",
            Symbol = "$",
            IsActive = true
        };


        UpdateCountry = new()
        {
            Id = Country.Id,
            Name = "Colombia",
            Alpha2 = "CO",
            Alpha3 = "COL",
            Capital = "Bogotá",
            Code = 106,
            IdCurrency = Country.IdCurrency,
            TimeZone = "America/Bogota",
            IsActive = true
        };

        UpdateState = new()
        {
            Id = State.Id,
            Name = "Bogotá",
            Code = "BO",
            IdCountry = State.IdCountry,
            IsActive = true
        };

        UpdateCity = new()
        {
            Id = City.Id,
            Name = "Bogotá",
            IdState = City.IdState,
            TimeZone = "America/Bogota",
            IsActive = true,
        };

        UpdateLocality = new()
        {
            Id = Locality.Id,
            Name = "Puente Aranda",
            IdCity = Locality.IdCity,
            IsActive = true
        };

        UpdateNeighborhood = new()
        {
            Id = Neighborhood.Id,
            Name = "Galán",
            IdLocality = Neighborhood.IdLocality,
            IsActive = true
        };

        UpdateTimezone = new()
        {
            Id = Timezone.Id,
            Name = "America/Bogota"
        };

        CreateCurrencyCommand = new(Currency.Id, CreateCurrency.Name, CreateCurrency.Code, CreateCurrency.Symbol);

        CreateCountryCommand = new(Country.Id, CreateCountry.Name, CreateCountry.Alpha2, CreateCountry.Alpha3, CreateCountry.Code, CreateCountry.Capital, CreateCountry.IdCurrency, CreateCountry.TimeZone);

        CreateStateCommand = new(State.Id, State.IdCountry, CreateState.Code, CreateState.Name);

        CreateCityCommand = new(City.Id, State.Id, City.Name, City.TimeZone);

        CreateLocalityCommand = new(Locality.Id, Locality.Name, Locality.IdCity);

        CreateNeighborhoodCommand = new(Neighborhood.Id, Neighborhood.Name, Neighborhood.IdLocality);

        CreateTimezoneCommand = new(Timezone.Id, Timezone.Name);


        UpdateCurrencyCommand = new(UpdateCurrency.Id, UpdateCurrency.Name, UpdateCurrency.Code, UpdateCurrency.Symbol, UpdateCurrency.IsActive);

        UpdateCountryCommand = new(UpdateCountry.Id, UpdateCountry.Name, UpdateCountry.Alpha2, UpdateCountry.Alpha3, UpdateCountry.Code, UpdateCountry.Capital, UpdateCountry.IdCurrency, UpdateCountry.TimeZone, UpdateCountry.IsActive);

        UpdateStateCommand = new(UpdateState.Id, UpdateState.IdCountry, UpdateState.Code, UpdateState.Name, UpdateState.IsActive);

        UpdateCityCommand = new(UpdateCity.Id, UpdateCity.Id, UpdateCity.Name, UpdateCity.TimeZone, UpdateCity.IsActive);

        UpdateLocalityCommand = new(UpdateLocality.Id, UpdateLocality.Name, UpdateLocality.IdCity, UpdateLocality.IsActive);

        UpdateNeighborhoodCommand = new(UpdateNeighborhood.Id, UpdateNeighborhood.Name, UpdateNeighborhood.IdLocality, UpdateNeighborhood.IsActive);

        UpdateTimezoneCommand = new(UpdateTimezone.Id, UpdateTimezone.Name, true);

        DeleteCurrencyCommand = new(Currency.Id);

        DeleteCountryCommand = new(Country.Id);

        DeleteStateCommand = new(State.Id);

        DeleteCityCommand = new(City.Id);

        DeleteLocalityCommand = new(Locality.Id);

        DeleteNeighborhoodCommand = new(Neighborhood.Id);


        CurrencyAggregate = CurrencyAggregate.Create(Currency.Id,  Currency.Code, Currency.Symbol, Currency.Name, Guid.NewGuid());

        CountryAggregate = CountryAggregate.Create(Country.Id, Country.Name, Country.Alpha2, Country.Alpha3, Country.Code, Country.Capital, Country.IdCurrency, Country.TimeZone, Guid.NewGuid());

        StateAggregate = StateAggregate.Create(State.Id, State.IdCountry, State.Code,State.Name, Guid.NewGuid());

        CityAggregate = CityAggregate.Create(City.Id, City.IdState, City.Name, City.TimeZone, Guid.NewGuid());

        LocalityAggregate = LocalityAggregate.Create(Locality.Id, Locality.IdCity, Locality.Name, Guid.NewGuid());

        NeighborhoodAggregate = NeighborhoodAggregate.Create(Neighborhood.Id, Neighborhood.IdLocality, Neighborhood.Name, Guid.NewGuid());

        TimezoneAggregate = TimezoneAggregate.Create(Timezone.Id, Timezone.Name, Guid.NewGuid());

    }

}
