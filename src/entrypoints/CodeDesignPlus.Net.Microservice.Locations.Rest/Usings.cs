global using CodeDesignPlus.Microservice.Api.Dtos;
global using CodeDesignPlus.Net.Logger.Extensions;
global using CodeDesignPlus.Net.Mongo.Extensions;
global using CodeDesignPlus.Net.Observability.Extensions;
global using CodeDesignPlus.Net.RabbitMQ.Extensions;
global using CodeDesignPlus.Net.Redis.Extensions;
global using CodeDesignPlus.Net.Security.Extensions;
global using Mapster;
global using MapsterMapper;
global using MediatR;
global using Microsoft.AspNetCore.Mvc;
global using C = CodeDesignPlus.Net.Core.Abstractions.Models.Criteria;
global using CodeDesignPlus.Net.Serializers;
global using NodaTime;









global using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.CreateCountry;
global using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.UpdateCountry;
global using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.DeleteCountry;
global using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Queries.GetCountryById;
global using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Queries.GetAllCountry;
global using CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.CreateState;
global using CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.UpdateState;
global using CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.DeleteState;
global using CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.CreateCity;
global using CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.UpdateCity;
global using CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.DeleteCity;
global using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.CreateLocality;
global using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.UpdateLocality;
global using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.DeleteLocality;
global using CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.CreateNeighborhood;
global using CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.UpdateNeighborhood;
global using CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.DeleteNeighborhood;
global using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.CreateTimezone;
global using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.UpdateTimezone;
global using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.DeleteTimezone;
global using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.CreateCurrency;
global using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.UpdateCurrency;
global using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.DeleteCurrency;
global using CodeDesignPlus.Net.Microservice.Locations.Application.State.Queries.FindStateById;
global using CodeDesignPlus.Net.Microservice.Locations.Application.State.Queries.FindAllStates;
global using CodeDesignPlus.Net.Microservice.Locations.Application.City.Queries.FindCityById;
global using CodeDesignPlus.Net.Microservice.Locations.Application.City.Queries.FindAllCities;
global using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Queries.FindLocalityById;
global using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Queries.FindAllLocalities;
global using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Queries.FindTimezoneById;
global using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Queries.FindAllTimezones;
global using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.FindCurrencyById;
global using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.FindAllCurrencies;