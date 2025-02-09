using System;
using CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Helpers;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NodaTime.Serialization.SystemTextJson;

namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Controllers;

public class NeighborhoodControllerTest : ServerBase<Program>, IClassFixture<Server<Program>>
{
    private readonly System.Text.Json.JsonSerializerOptions options = new System.Text.Json.JsonSerializerOptions()
    {
        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
    }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

    private readonly FakeData fakeData = new();

    public NeighborhoodControllerTest(Server<Program> server) : base(server)
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
    public async Task GetNeighborhoods_ReturnOk()
    {
        await Client.CreateNeighborhoodAsync(fakeData);

        var response = await Client.RequestAsync("http://localhost/api/Neighborhood", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var Neighborhoods = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<NeighborhoodDto>>(json, Utils.Options);

        Assert.NotNull(Neighborhoods);
        Assert.NotEmpty(Neighborhoods);
        Assert.Contains(Neighborhoods, x => x.Id == fakeData.CreateNeighborhood.Id);
    }

    [Fact]
    public async Task GetNeighborhoodById_ReturnOk()
    {
        await Client.CreateNeighborhoodAsync(fakeData);

        var response = await Client.RequestAsync($"http://localhost/api/Neighborhood/{fakeData.CreateNeighborhood.Id}", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var neighborhood = System.Text.Json.JsonSerializer.Deserialize<NeighborhoodDto>(json, Utils.Options);

        Assert.NotNull(neighborhood);
        Assert.Equal(fakeData.CreateNeighborhood.Id, neighborhood.Id);
        Assert.Equal(fakeData.CreateNeighborhood.Name, neighborhood.Name);
        Assert.Equal(fakeData.CreateNeighborhood.IdLocality, neighborhood.IdLocality);
    }

    [Fact]
    public async Task CreateNeighborhood_ReturnNoContent()
    {
        await Client.CreateLocaliyAsync(fakeData);

        var data = fakeData.CreateNeighborhood;

        var json = System.Text.Json.JsonSerializer.Serialize(data, Utils.Options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.RequestAsync("http://localhost/api/Neighborhood", content, HttpMethod.Post);

        var neighborhood = await Client.GetRecordAsync<NeighborhoodDto>("Neighborhood", data.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Equal(data.Id, neighborhood.Id);
        Assert.Equal(data.Id, neighborhood.Id);
        Assert.Equal(data.Name, neighborhood.Name);
        Assert.Equal(data.IdLocality, neighborhood.IdLocality);
    }

    [Fact]
    public async Task UpdateNeighborhood_ReturnNoContent()
    {
        await Client.CreateNeighborhoodAsync(fakeData);

        var data = fakeData.UpdateNeighborhood;

        var json = System.Text.Json.JsonSerializer.Serialize(data, Utils.Options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.RequestAsync($"http://localhost/api/Neighborhood/{fakeData.CreateNeighborhood.Id}", content, HttpMethod.Put);

        var neighborhood = await Client.GetRecordAsync<NeighborhoodDto>("Neighborhood", data.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Equal(data.Id, neighborhood.Id);
        Assert.Equal(data.Id, neighborhood.Id);
        Assert.Equal(data.Name, neighborhood.Name);
        Assert.Equal(data.IdLocality, neighborhood.IdLocality);
    }

    [Fact]
    public async Task DeleteNeighborhood_ReturnNoContent()
    {
        await Client.CreateNeighborhoodAsync(fakeData);

        var response = await Client.RequestAsync($"http://localhost/api/Neighborhood/{fakeData.CreateNeighborhood.Id}", null, HttpMethod.Delete);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }


}