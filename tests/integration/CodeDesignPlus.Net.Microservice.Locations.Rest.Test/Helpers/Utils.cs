using System;
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

namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Helpers;

public class Utils
{
    public readonly CurrencyDto Currency;
    public readonly CountryDto Country;
    public readonly StateDto State;
    public readonly CityDto City;
    public readonly LocalityDto Locality;
    public readonly NeighborhoodDto Neighborhood;
    public readonly CreateCurrencyDto CreateCurrency;
    public readonly CreateCountryDto CreateCountry;
    public readonly CreateStateDto CreateState;
    public readonly CreateCityDto CreateCity;
    public readonly CreateLocalityDto CreateLocality;
    public readonly CreateNeighborhoodDto CreateNeighborhood;

    public readonly UpdateCurrencyDto UpdateCurrency;
    public readonly UpdateCountryDto UpdateCountry;
    public readonly UpdateStateDto UpdateState;
    public readonly UpdateCityDto UpdateCity;
    public readonly UpdateLocalityDto UpdateLocality;
    public readonly UpdateNeighborhoodDto UpdateNeighborhood;

    public readonly CreateCurrencyCommand CreateCurrencyCommand;
    public readonly CreateCountryCommand CreateCountryCommand;
    public readonly CreateStateCommand CreateStateCommand;
    public readonly CreateCityCommand CreateCityCommand;
    public readonly CreateLocalityCommand CreateLocalityCommand;
    public readonly CreateNeighborhoodCommand CreateNeighborhoodCommand;

    public readonly UpdateCurrencyCommand UpdateCurrencyCommand;
    public readonly UpdateCountryCommand UpdateCountryCommand;
    public readonly UpdateStateCommand UpdateStateCommand;
    public readonly UpdateCityCommand UpdateCityCommand;
    public readonly UpdateLocalityCommand UpdateLocalityCommand;
    public readonly UpdateNeighborhoodCommand UpdateNeighborhoodCommand;

    public Utils()
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


        CreateCurrency = new()
        {
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

        CreateCurrencyCommand = new(Currency.Id, CreateCurrency.Name, CreateCurrency.Code, CreateCurrency.Symbol);

        CreateCountryCommand = new(Country.Id, CreateCountry.Name, CreateCountry.Alpha2, CreateCountry.Alpha3, CreateCountry.Code, CreateCountry.Capital, CreateCountry.IdCurrency, CreateCountry.TimeZone);

        CreateStateCommand = new(State.Id, State.IdCountry, CreateState.Code, CreateState.Name);

        CreateCityCommand = new(City.Id, State.Id, City.Name, City.TimeZone);

        CreateLocalityCommand = new(Locality.Id, Locality.Name, Locality.IdCity);

        CreateNeighborhoodCommand = new(Neighborhood.Id, Neighborhood.Name, Neighborhood.IdLocality);


        UpdateCurrencyCommand = new(UpdateCurrency.Id, UpdateCurrency.Name, UpdateCurrency.Code, UpdateCurrency.Symbol, UpdateCurrency.IsActive);

        UpdateCountryCommand = new(UpdateCountry.Id, UpdateCountry.Name, UpdateCountry.Alpha2, UpdateCountry.Alpha3, UpdateCountry.Code, UpdateCountry.Capital, UpdateCountry.IdCurrency, UpdateCountry.TimeZone, UpdateCountry.IsActive);

        UpdateStateCommand = new(UpdateState.Id, UpdateState.IdCountry, UpdateState.Code, UpdateState.Name, UpdateState.IsActive);

        UpdateCityCommand = new(UpdateCity.Id, UpdateCity.Id, UpdateCity.Name, UpdateCity.TimeZone, UpdateCity.IsActive);

        UpdateLocalityCommand = new(UpdateLocality.Id, UpdateLocality.Name, UpdateLocality.IdCity, UpdateLocality.IsActive);

        UpdateNeighborhoodCommand = new(UpdateNeighborhood.Id, UpdateNeighborhood.Name, UpdateNeighborhood.IdLocality, UpdateNeighborhood.IsActive);


    }

    // public readonly CurrencyDto Currency = new()
    // {
    //     Id = Guid.NewGuid(),
    //     Name = "Dollar",
    //     Code = "USD",
    //     Symbol = "$"
    // };

    // public readonly CountryDto Country = new()
    // {
    //     Id = Guid.NewGuid(),
    //     Name = "United States",
    //     Alpha2 = "US",
    //     Alpha3 = "USA",
    //     Capital = "Washington",
    //     IdCurrency = Currency.Id,
    //     Code = 103,
    //     TimeZone = "America/New_York",
    // };

    // public readonly StateDto State = new()
    // {
    //     Id = Guid.NewGuid(),
    //     Name = "New York",
    //     Code = "NY",
    //     IdCountry = Country.Id
    // };

    // public readonly CityDto City = new()
    // {
    //     Id = Guid.NewGuid(),
    //     Name = "New York City",
    //     TimeZone = "America/New_York",
    //     IdState = State.Id
    // };

    // public readonly LocalityDto Locality = new()
    // {
    //     Id = Guid.NewGuid(),
    //     Name = "Manhattan",
    //     IdCity = City.Id
    // };

    // public readonly NeighborhoodDto Neighborhood = new()
    // {
    //     Id = Guid.NewGuid(),
    //     Name = "Upper East Side",
    //     IdLocality = Locality.Id
    // };


    // public readonly CreateCurrencyDto CreateCurrency = new()
    // {
    //     Name = "Dollar",
    //     Code = "USD",
    //     Symbol = "$"
    // };


    // public readonly CreateCountryDto CreateCountry = new()
    // {
    //     Id = Country.Id,
    //     Name = Country.Name,
    //     Alpha2 = Country.Alpha2,
    //     Alpha3 = Country.Alpha3,
    //     Capital = Country.Capital,
    //     IdCurrency = Country.IdCurrency,
    //     Code = Country.Code,
    //     TimeZone = Country.TimeZone
    // };

    // public readonly CreateStateDto CreateState = new()
    // {
    //     Name = State.Name,
    //     Code = State.Code,
    //     IdCountry = State.IdCountry
    // };

    // public readonly CreateCityDto CreateCity = new()
    // {
    //     Id = City.Id,
    //     Name = City.Name,
    //     TimeZone = City.TimeZone,
    //     IdState = City.IdState
    // };

    // public readonly CreateLocalityDto CreateLocality = new()
    // {
    //     Name = Locality.Name,
    //     Id = Locality.Id,
    //     IdCity = Locality.IdCity
    // };

    // public readonly CreateNeighborhoodDto CreateNeighborhood = new()
    // {
    //     Name = Neighborhood.Name,
    //     Id = Neighborhood.Id,
    //     IdLocality = Neighborhood.IdLocality
    // };


    // public readonly UpdateCurrencyDto UpdateCurrency = new()
    // {
    //     Id = Currency.Id,
    //     Name = "Pesos",
    //     Code = "COP",
    //     Symbol = "$",
    //     IsActive = true
    // };


    // public readonly UpdateCountryDto UpdateCountry = new()
    // {
    //     Id = Country.Id,
    //     Name = "Colombia",
    //     Alpha2 = "CO",
    //     Alpha3 = "COL",
    //     Capital = "Bogotá",
    //     Code = 106,
    //     IdCurrency = Country.IdCurrency,
    //     TimeZone = "America/Bogota",
    //     IsActive = true
    // };

    // public readonly UpdateStateDto UpdateState = new()
    // {
    //     Id = State.Id,
    //     Name = "Bogotá",
    //     Code = "BO",
    //     IdCountry = State.IdCountry,
    //     IsActive = true
    // };

    // public readonly UpdateCityDto UpdateCity = new()
    // {
    //     Id = City.Id,
    //     Name = "Bogotá",
    //     IdState = City.IdState,
    //     TimeZone = "America/Bogota",
    //     IsActive = true,
    // };

    // public readonly UpdateLocalityDto UpdateLocality = new()
    // {
    //     Id = Locality.Id,
    //     Name = "Puente Aranda",
    //     IdCity = Locality.IdCity,
    //     IsActive = true
    // };

    // public readonly UpdateNeighborhoodDto UpdateNeighborhood = new()
    // {
    //     Id = Neighborhood.Id,
    //     Name = "Galán",
    //     IdLocality = Neighborhood.IdLocality,
    //     IsActive = true
    // };

    // public readonly CreateCurrencyCommand CreateCurrencyCommand = new(Currency.Id, CreateCurrency.Name, CreateCurrency.Code, CreateCurrency.Symbol);

    // public readonly CreateCountryCommand CreateCountryCommand = new(Country.Id, CreateCountry.Name, CreateCountry.Alpha2, CreateCountry.Alpha3, CreateCountry.Code, CreateCountry.Capital, CreateCountry.IdCurrency, CreateCountry.TimeZone);

    // public readonly CreateStateCommand CreateStateCommand = new(State.Id, State.IdCountry, CreateState.Code, CreateState.Name);

    // public readonly CreateCityCommand CreateCityCommand = new(City.Id, State.Id, City.Name, City.TimeZone);

    // public readonly CreateLocalityCommand CreateLocalityCommand = new(Locality.Id, Locality.Name, Locality.IdCity);

    // public readonly CreateNeighborhoodCommand CreateNeighborhoodCommand = new(Neighborhood.Id, Neighborhood.Name, Neighborhood.IdLocality);


    // public readonly UpdateCurrencyCommand UpdateCurrencyCommand = new(UpdateCurrency.Id, UpdateCurrency.Name, UpdateCurrency.Code, UpdateCurrency.Symbol, UpdateCurrency.IsActive);

    // public readonly UpdateCountryCommand UpdateCountryCommand = new(UpdateCountry.Id, UpdateCountry.Name, UpdateCountry.Alpha2, UpdateCountry.Alpha3, UpdateCountry.Code, UpdateCountry.Capital, UpdateCountry.IdCurrency, UpdateCountry.TimeZone, UpdateCountry.IsActive);

    // public readonly UpdateStateCommand UpdateStateCommand = new(UpdateState.Id, UpdateState.IdCountry, UpdateState.Code, UpdateState.Name, UpdateState.IsActive);

    // public readonly UpdateCityCommand UpdateCityCommand = new(UpdateCity.Id, UpdateCity.Id, UpdateCity.Name, UpdateCity.TimeZone, UpdateCity.IsActive);

    // public readonly UpdateLocalityCommand UpdateLocalityCommand = new(UpdateLocality.Id, UpdateLocality.Name, UpdateLocality.IdCity, UpdateLocality.IsActive);

    // public readonly UpdateNeighborhoodCommand UpdateNeighborhoodCommand = new(UpdateNeighborhood.Id, UpdateNeighborhood.Name, UpdateNeighborhood.IdLocality, UpdateNeighborhood.IsActive);


}
