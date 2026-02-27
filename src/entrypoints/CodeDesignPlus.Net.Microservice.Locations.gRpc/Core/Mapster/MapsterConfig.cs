using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.DataTransferObjects;
using NodaTime;
using NodaTime.Serialization.Protobuf;

namespace CodeDesignPlus.Net.Microservice.Locations.gRpc.Core.Mapster;

public static class MapsterConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<CountryDto, GetCountryResponse>.NewConfig();
        TypeAdapterConfig<CurrencyDto, Currency>.NewConfig();
        TypeAdapterConfig<CurrencyDto, GetCurrencyResponse>.NewConfig();        
    }
}