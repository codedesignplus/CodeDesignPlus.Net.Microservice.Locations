using System;
using CodeDesignPlus.Net.Microservice.Locations.Application.City.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Helpers;
using Ductus.FluentDocker.Commands;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NodaTime.Serialization.SystemTextJson;

namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Controllers;

public class CityControllerTest : ServerBase<Program>, IClassFixture<Server<Program>>
{
    private readonly FakeData fakeData = new();

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
        await Client.CreateCityAsync(fakeData);

        var response = await Client.RequestAsync("http://localhost/api/City", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var cities = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<CityDto>>(json, Utils.Options);

        Assert.NotNull(cities);
        Assert.NotEmpty(cities);
        Assert.Contains(cities, x => x.Id == fakeData.CreateCity.Id);
    }

    [Fact]
    public async Task GetCityById_ReturnOk()
    {
        await Client.CreateCityAsync(fakeData);

        var response = await Client.RequestAsync($"http://localhost/api/City/{fakeData.CreateCity.Id}", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var city = System.Text.Json.JsonSerializer.Deserialize<CityDto>(json, Utils.Options);

        Assert.NotNull(city);
        Assert.Equal(fakeData.CreateCity.Id, city.Id);
        Assert.Equal(fakeData.CreateCity.Name, city.Name);
        Assert.Equal(fakeData.CreateCity.IdState, city.IdState);
        Assert.Equal(fakeData.CreateCity.TimeZone, city.TimeZone);
    }

    [Fact]
    public async Task CreateCity_ReturnNoContent()
    {
        await Client.CreateStateAsync(fakeData);
        
        var data = fakeData.CreateCity;

        var json = System.Text.Json.JsonSerializer.Serialize(data, Utils.Options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.RequestAsync("http://localhost/api/City", content, HttpMethod.Post);

        var city = await Client.GetRecordAsync<CityDto>("City", data.Id);

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
        await Client.CreateCityAsync(fakeData);

        var data = fakeData.UpdateCity;

        var json = System.Text.Json.JsonSerializer.Serialize(data, Utils.Options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.RequestAsync($"http://localhost/api/City/{fakeData.CreateCity.Id}", content, HttpMethod.Put);

        var city = await Client.GetRecordAsync<CityDto>("City", data.Id);

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
        await Client.CreateCityAsync(fakeData);

        var response = await Client.RequestAsync($"http://localhost/api/City/{fakeData.CreateCity.Id}", null, HttpMethod.Delete);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}