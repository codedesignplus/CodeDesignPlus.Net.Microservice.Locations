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
        var data = await this.CreateNeighborhoodAsync();

        var response = await this.RequestAsync("http://localhost/api/Neighborhood", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var Neighborhoods = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<NeighborhoodDto>>(json, Utils.Options);

        Assert.NotNull(Neighborhoods);
        Assert.NotEmpty(Neighborhoods);
        Assert.Contains(Neighborhoods, x => x.Id == data.Id);
    }

    [Fact]
    public async Task GetNeighborhoodById_ReturnOk()
    {
        var data = await this.CreateNeighborhoodAsync();

        var response = await this.RequestAsync($"http://localhost/api/Neighborhood/{data.Id}", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var neighborhood = System.Text.Json.JsonSerializer.Deserialize<NeighborhoodDto>(json, Utils.Options);

        Assert.NotNull(neighborhood);
        Assert.Equal(data.Id, neighborhood.Id);
        Assert.Equal(data.Name, neighborhood.Name);
        Assert.Equal(data.IdLocality, neighborhood.IdLocality);
    }

    [Fact]
    public async Task CreateNeighborhood_ReturnNoContent()
    {
        var data = fakeData.CreateNeighborhood;

        var json = System.Text.Json.JsonSerializer.Serialize(data, Utils.Options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync("http://localhost/api/Neighborhood", content, HttpMethod.Post);

        var neighborhood = await this.GetRecordAsync(data.Id);

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
        var NeighborhoodCreated = await this.CreateNeighborhoodAsync();

        var data = fakeData.UpdateNeighborhood;

        var json = System.Text.Json.JsonSerializer.Serialize(data, Utils.Options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync($"http://localhost/api/Neighborhood/{NeighborhoodCreated.Id}", content, HttpMethod.Put);

        var neighborhood = await this.GetRecordAsync(data.Id);

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
        var NeighborhoodCreated = await this.CreateNeighborhoodAsync();

        var response = await this.RequestAsync($"http://localhost/api/Neighborhood/{NeighborhoodCreated.Id}", null, HttpMethod.Delete);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    private async Task<CreateNeighborhoodDto> CreateNeighborhoodAsync()
    {
        var data = fakeData.CreateNeighborhood;

        var json = System.Text.Json.JsonSerializer.Serialize(data, Utils.Options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        await this.RequestAsync("http://localhost/api/Neighborhood", content, HttpMethod.Post);

        return data;
    }

    private async Task<NeighborhoodDto> GetRecordAsync(Guid id)
    {
        var response = await this.RequestAsync($"http://localhost/api/Neighborhood/{id}", null, HttpMethod.Get);

        var json = await response.Content.ReadAsStringAsync();

        return System.Text.Json.JsonSerializer.Deserialize<NeighborhoodDto>(json, Utils.Options)!;
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