using System;
using CodeDesignPlus.Net.Microservice.Locations.Application.City.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Helpers;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NodaTime.Serialization.SystemTextJson;

namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Controllers;

public class CityControllerTest : ServerBase<Program>, IClassFixture<Server<Program>>
{
    private readonly System.Text.Json.JsonSerializerOptions options = new System.Text.Json.JsonSerializerOptions()
    {
        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
    }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

    private readonly Utils Utils = new();

    public CityControllerTest(Server<Program> server) : base(server)
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
    public async Task GetCities_ReturnOk()
    {
        var city = await this.CreateCityAsync();

        var response = await this.RequestAsync("http://localhost/api/City", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var cities = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<CityDto>>(json, this.options);

        Assert.NotNull(cities);
        Assert.NotEmpty(cities);
        Assert.Contains(cities, x => x.Id == city.Id);
    }

    [Fact]
    public async Task GetCityById_ReturnOk()
    {
        var cityCreated = await this.CreateCityAsync();

        var response = await this.RequestAsync($"http://localhost/api/City/{cityCreated.Id}", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var city = System.Text.Json.JsonSerializer.Deserialize<CityDto>(json, this.options);

        Assert.NotNull(city);
        Assert.Equal(cityCreated.Id, city.Id);
        Assert.Equal(cityCreated.Name, city.Name);
        Assert.Equal(cityCreated.IdState, city.IdState);
        Assert.Equal(cityCreated.TimeZone, city.TimeZone);
    }

    [Fact]
    public async Task CreateCity_ReturnNoContent()
    {
        var data = Utils.CreateCity;

        var json = System.Text.Json.JsonSerializer.Serialize(data, this.options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync("http://localhost/api/City", content, HttpMethod.Post);

        var city = await this.GetRecordAsync(data.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Equal(data.Id, city.Id);
        Assert.Equal(data.Name, city.Name);
        Assert.Equal(data.IdState, city.IdState);
        Assert.Equal(data.TimeZone, city.TimeZone);
    }

    [Fact]
    public async Task UpdateCity_ReturnNoContent()
    {
        var cityCreated = await this.CreateCityAsync();

        var data = Utils.UpdateCity;

        var json = System.Text.Json.JsonSerializer.Serialize(data, this.options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync($"http://localhost/api/City/{cityCreated.Id}", content, HttpMethod.Put);

        var city = await this.GetRecordAsync(data.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Equal(data.Id, city.Id);
        Assert.Equal(data.Name, city.Name);
        Assert.Equal(data.IdState, city.IdState);
        Assert.Equal(data.TimeZone, city.TimeZone);
    }

    [Fact]
    public async Task DeleteCity_ReturnNoContent()
    {
        var cityCreated = await this.CreateCityAsync();

        var response = await this.RequestAsync($"http://localhost/api/City/{cityCreated.Id}", null, HttpMethod.Delete);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    private async Task<CreateCityDto> CreateCityAsync()
    {
        var data = Utils.CreateCity;

        var json = System.Text.Json.JsonSerializer.Serialize(data, this.options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        await this.RequestAsync("http://localhost/api/City", content, HttpMethod.Post);

        return data;
    }

    private async Task<CityDto> GetRecordAsync(Guid id)
    {
        var response = await this.RequestAsync($"http://localhost/api/City/{id}", null, HttpMethod.Get);

        var json = await response.Content.ReadAsStringAsync();

        return System.Text.Json.JsonSerializer.Deserialize<CityDto>(json, this.options)!;
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