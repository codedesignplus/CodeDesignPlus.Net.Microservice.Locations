using CodeDesignPlus.Microservice.Api.Dtos;
using CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.CreateCity;
using CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.UpdateCity;
using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.CreateCountry;
using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.UpdateCountry;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.CreateCurrency;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.UpdateCurrency;
using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.CreateLocality;
using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.UpdateLocality;
using CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.CreateNeighborhood;
using CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.UpdateNeighborhood;
using CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.CreateState;
using CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.UpdateState;
using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.CreateTimezone;
using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.UpdateTimezone;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Setup;

public static class MapsterConfigCountry
{
    public static void Configure()
    {

        //City
        TypeAdapterConfig<CreateCityDto, CreateCityCommand>.NewConfig();
        TypeAdapterConfig<UpdateCityDto, UpdateCityCommand>.NewConfig();
        TypeAdapterConfig<CityAggregate, CityDto>.NewConfig();

        //Country
        TypeAdapterConfig<CreateCountryDto, CreateCountryCommand>.NewConfig();
        TypeAdapterConfig<UpdateCountryDto, UpdateCountryCommand>.NewConfig();
        TypeAdapterConfig<CountryAggregate, CountryDto>.NewConfig();

        //Currency
        TypeAdapterConfig<CreateCurrencyDto, CreateCurrencyCommand>.NewConfig();
        TypeAdapterConfig<UpdateCurrencyDto, UpdateCurrencyCommand>.NewConfig();
        TypeAdapterConfig<CurrencyAggregate, CurrencyDto>.NewConfig();

        //Locality
        TypeAdapterConfig<CreateLocalityDto, CreateLocalityCommand>.NewConfig();
        TypeAdapterConfig<UpdateLocalityDto, UpdateLocalityCommand>.NewConfig();
        TypeAdapterConfig<LocalityAggregate, LocalityDto>.NewConfig();

        //Neighborhood
        TypeAdapterConfig<CreateNeighborhoodDto, CreateNeighborhoodCommand>.NewConfig();
        TypeAdapterConfig<UpdateNeighborhoodDto, UpdateNeighborhoodCommand>.NewConfig();
        TypeAdapterConfig<NeighborhoodAggregate, NeighborhoodDto>.NewConfig();

        //State
        TypeAdapterConfig<CreateStateDto, CreateStateCommand>.NewConfig();
        TypeAdapterConfig<UpdateStateDto, UpdateStateCommand>.NewConfig();
        TypeAdapterConfig<StateAggregate, StateDto>.NewConfig();

        //Timezone
        TypeAdapterConfig<CreateTimezoneDto, CreateTimezoneCommand>.NewConfig();
        TypeAdapterConfig<UpdateTimezoneDto, UpdateTimezoneCommand>.NewConfig();
        TypeAdapterConfig<TimezoneAggregate, TimezoneDto>.NewConfig();



    }
}
