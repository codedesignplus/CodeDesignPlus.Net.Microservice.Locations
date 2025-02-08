using System;
using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Helpers;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NodaTime.Serialization.SystemTextJson;

namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Controllers;

public class LocalityControllerTest : ServerBase<Program>, IClassFixture<Server<Program>>
{
    private readonly System.Text.Json.JsonSerializerOptions options = new System.Text.Json.JsonSerializerOptions()
    {
        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
    }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

    private readonly Utils Utils = new();

    public LocalityControllerTest(Server<Program> server) : base(server)
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
    public async Task GetLocalities_ReturnOk()
    {
        var data = await this.CreateLocalityAsync();

        var response = await this.RequestAsync("http://localhost/api/Locality", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var Localities = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<LocalityDto>>(json, this.options);

        Assert.NotNull(Localities);
        Assert.NotEmpty(Localities);
        Assert.Contains(Localities, x => x.Id == data.Id);
    }

    [Fact]
    public async Task GetLocalityById_ReturnOk()
    {
        var data = await this.CreateLocalityAsync();

        var response = await this.RequestAsync($"http://localhost/api/Locality/{data.Id}", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var locality = System.Text.Json.JsonSerializer.Deserialize<LocalityDto>(json, this.options);

        Assert.NotNull(locality);
        Assert.Equal(data.Id, locality.Id);
        Assert.Equal(data.Name, locality.Name);
        Assert.Equal(data.IdCity, locality.IdCity);
    }

    [Fact]
    public async Task CreateLocality_ReturnNoContent()
    {
        var data = Utils.CreateLocality;

        var json = System.Text.Json.JsonSerializer.Serialize(data, this.options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync("http://localhost/api/Locality", content, HttpMethod.Post);

        var locality = await this.GetRecordAsync(data.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Equal(data.Id, locality.Id);
        Assert.Equal(data.Id, locality.Id);
        Assert.Equal(data.Name, locality.Name);
        Assert.Equal(data.IdCity, locality.IdCity);
    }

    [Fact]
    public async Task UpdateLocality_ReturnNoContent()
    {
        var localityCreated = await this.CreateLocalityAsync();

        var data = Utils.UpdateLocality;

        var json = System.Text.Json.JsonSerializer.Serialize(data, this.options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync($"http://localhost/api/Locality/{localityCreated.Id}", content, HttpMethod.Put);

        var locality = await this.GetRecordAsync(data.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Equal(data.Id, locality.Id);
        Assert.Equal(data.Id, locality.Id);
        Assert.Equal(data.Name, locality.Name);
        Assert.Equal(data.IdCity, locality.IdCity);
    }

    [Fact]
    public async Task DeleteLocality_ReturnNoContent()
    {
        var localityCreated = await this.CreateLocalityAsync();

        var response = await this.RequestAsync($"http://localhost/api/Locality/{localityCreated.Id}", null, HttpMethod.Delete);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    private async Task<CreateLocalityDto> CreateLocalityAsync()
    {
        var data = Utils.CreateLocality;

        var json = System.Text.Json.JsonSerializer.Serialize(data, this.options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        await this.RequestAsync("http://localhost/api/Locality", content, HttpMethod.Post);

        return data;
    }

    private async Task<LocalityDto> GetRecordAsync(Guid id)
    {
        var response = await this.RequestAsync($"http://localhost/api/Locality/{id}", null, HttpMethod.Get);

        var json = await response.Content.ReadAsStringAsync();

        return System.Text.Json.JsonSerializer.Deserialize<LocalityDto>(json, this.options)!;
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