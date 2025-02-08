using System;
using CodeDesignPlus.Net.Microservice.Locations.Application.Country.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Helpers;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NodaTime.Serialization.SystemTextJson;

namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Controllers;

public class CountryControllerTest : ServerBase<Program>, IClassFixture<Server<Program>>
{
    private readonly System.Text.Json.JsonSerializerOptions options = new System.Text.Json.JsonSerializerOptions()
    {
        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
    }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

    private readonly Utils Utils = new();

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
        var data = await this.CreateCountryAsync();

        var response = await this.RequestAsync("http://localhost/api/Country", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var countries = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<CountryDto>>(json, this.options);

        Assert.NotNull(countries);
        Assert.NotEmpty(countries);
        Assert.Contains(countries, x => x.Id == data.Id);
    }

    [Fact]
    public async Task GetCountryById_ReturnOk()
    {
        var data = await this.CreateCountryAsync();

        var response = await this.RequestAsync($"http://localhost/api/Country/{data.Id}", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var country = System.Text.Json.JsonSerializer.Deserialize<CountryDto>(json, this.options);

        Assert.NotNull(country);
        Assert.Equal(data.Id, country.Id);
        Assert.Equal(data.Name, country.Name);
        Assert.Equal(data.Alpha2, country.Alpha2);
        Assert.Equal(data.Alpha3, country.Alpha3);
        Assert.Equal(data.Code, country.Code);
        Assert.Equal(data.Capital, country.Capital);
        Assert.Equal(data.IdCurrency, country.IdCurrency);
        Assert.Equal(data.TimeZone, country.TimeZone);

    }

    [Fact]
    public async Task CreateCountry_ReturnNoContent()
    {
        var data = Utils.CreateCountry;

        var json = System.Text.Json.JsonSerializer.Serialize(data, this.options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync("http://localhost/api/Country", content, HttpMethod.Post);

        var country = await this.GetRecordAsync(data.Id);

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
        Assert.Equal(data.TimeZone, country.TimeZone);
    }

    [Fact]
    public async Task UpdateCountry_ReturnNoContent()
    {
        var CountryCreated = await this.CreateCountryAsync();

        var data = Utils.UpdateCountry;

        var json = System.Text.Json.JsonSerializer.Serialize(data, this.options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync($"http://localhost/api/Country/{CountryCreated.Id}", content, HttpMethod.Put);

        var country = await this.GetRecordAsync(data.Id);

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
        Assert.Equal(data.TimeZone, country.TimeZone);
    }

    [Fact]
    public async Task DeleteCountry_ReturnNoContent()
    {
        var countryCreated = await this.CreateCountryAsync();

        var response = await this.RequestAsync($"http://localhost/api/Country/{countryCreated.Id}", null, HttpMethod.Delete);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    private async Task<CreateCountryDto> CreateCountryAsync()
    {
        var data = Utils.CreateCountry;

        var json = System.Text.Json.JsonSerializer.Serialize(data, this.options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        await this.RequestAsync("http://localhost/api/Country", content, HttpMethod.Post);

        return data;
    }

    private async Task<CountryDto> GetRecordAsync(Guid id)
    {
        var response = await this.RequestAsync($"http://localhost/api/Country/{id}", null, HttpMethod.Get);

        var json = await response.Content.ReadAsStringAsync();

        return System.Text.Json.JsonSerializer.Deserialize<CountryDto>(json, this.options)!;
    }

    private async Task<HttpResponseMessage> RequestAsync(string uri, HttpContent? content, HttpMethod method)
    {
        var httpRequestMessage = new HttpRequestMessage()
        {
            RequestUri = new Uri(uri),
            Content = content,
            Method = method
        };
        httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("TestAuth");

        var response = await Client.SendAsync(httpRequestMessage);

        if (!response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            throw new Exception(data);
        }

        return response;
    }

}