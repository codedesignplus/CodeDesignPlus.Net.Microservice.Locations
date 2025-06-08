using System;
using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;
using CodeDesignPlus.Net.Microservice.Locations.Application.Country.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Helpers;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NodaTime.Serialization.SystemTextJson;

namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Controllers;

public class CountryControllerTest : ServerBase<Program>, IClassFixture<Server<Program>>
{
    private readonly FakeData fakeData = new();

    public CountryControllerTest(Server<Program> server) : base(server)
    {
        server.InMemoryCollection = (x) =>
        {
            x.Add("Vault:Enable", "false");
            x.Add("Vault:Address", "http://localhost:8200");
            x.Add("Vault:Token", "root");
            x.Add("Solution", "CodeDesignPlus");
            x.Add("AppName", "my-test");
            x.Add("RabbitMQ:UserName", "guest");
            x.Add("RabbitMQ:Password", "guest");
            x.Add("Security:ValidAudiences:0", Guid.NewGuid().ToString());
        };
    }

    [Fact]
    public async Task GetCountries_ReturnOk()
    {
        await Client.CreateCountryAsync(fakeData);
        
        var response = await Client.RequestAsync("http://localhost/api/Country", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var countries = System.Text.Json.JsonSerializer.Deserialize<Pagination<CountryDto>>(json, Utils.Options);

        Assert.NotNull(countries);
        Assert.NotEmpty(countries.Data);
        Assert.Contains(countries.Data, x => x.Id == this.fakeData.CreateCountry.Id);
    }

    [Fact]
    public async Task GetCountryById_ReturnOk()
    {
        await Client.CreateCountryAsync(fakeData);

        var response = await Client.RequestAsync($"http://localhost/api/Country/{fakeData.CreateCountry.Id}", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var country = System.Text.Json.JsonSerializer.Deserialize<CountryDto>(json, Utils.Options);

        Assert.NotNull(country);
        Assert.Equal(this.fakeData.CreateCountry.Id, country.Id);
        Assert.Equal(this.fakeData.CreateCountry.Name, country.Name);
        Assert.Equal(this.fakeData.CreateCountry.Alpha2, country.Alpha2);
        Assert.Equal(this.fakeData.CreateCountry.Alpha3, country.Alpha3);
        Assert.Equal(this.fakeData.CreateCountry.Code, country.Code);
        Assert.Equal(this.fakeData.CreateCountry.Capital, country.Capital);
        Assert.Equal(this.fakeData.CreateCountry.IdCurrency, country.IdCurrency);
        Assert.Equal(this.fakeData.CreateCountry.Timezone, country.Timezone);

    }

    [Fact]
    public async Task CreateCountry_ReturnNoContent()
    {
        await Client.CreateCurrencyAsync(fakeData);

        var data = this.fakeData.CreateCountry;

        var json = System.Text.Json.JsonSerializer.Serialize(data, Utils.Options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.RequestAsync("http://localhost/api/Country", content, HttpMethod.Post);

        var country = await Client.GetRecordAsync<CountryDto>("Country", data.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Equal(data.Id, country.Id);
        Assert.Equal(data.Name, country.Name);
        Assert.Equal(data.Name, country.Name);
        Assert.Equal(data.Alpha2, country.Alpha2);
        Assert.Equal(data.Alpha3, country.Alpha3);
        Assert.Equal(data.Code, country.Code);
        Assert.Equal(data.Capital, country.Capital);
        Assert.Equal(data.IdCurrency, country.IdCurrency);
        Assert.Equal(data.Timezone, country.Timezone);
    }

    [Fact]
    public async Task UpdateCountry_ReturnNoContent()
    {
        await Client.CreateCountryAsync(fakeData);

        var data = this.fakeData.UpdateCountry;

        var json = System.Text.Json.JsonSerializer.Serialize(data, Utils.Options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.RequestAsync($"http://localhost/api/Country/{fakeData.CreateCountry.Id}", content, HttpMethod.Put);

        var country = await Client.GetRecordAsync<CountryDto>("Country", data.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Equal(data.Id, country.Id);
        Assert.Equal(data.Name, country.Name);
        Assert.Equal(data.Name, country.Name);
        Assert.Equal(data.Alpha2, country.Alpha2);
        Assert.Equal(data.Alpha3, country.Alpha3);
        Assert.Equal(data.Code, country.Code);
        Assert.Equal(data.Capital, country.Capital);
        Assert.Equal(data.IdCurrency, country.IdCurrency);
        Assert.Equal(data.Timezone, country.Timezone);
    }

    [Fact]
    public async Task DeleteCountry_ReturnNoContent()
    {
        await Client.CreateCountryAsync(fakeData);

        var response = await Client.RequestAsync($"http://localhost/api/Country/{fakeData.CreateCountry.Id}", null, HttpMethod.Delete);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}