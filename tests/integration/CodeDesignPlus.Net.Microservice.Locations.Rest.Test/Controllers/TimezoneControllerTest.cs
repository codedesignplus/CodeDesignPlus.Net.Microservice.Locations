using System;
using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Helpers;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NodaTime.Serialization.SystemTextJson;

namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Controllers;

public class TimezoneControllerTest : ServerBase<Program>, IClassFixture<Server<Program>>
{
    private readonly System.Text.Json.JsonSerializerOptions options = new System.Text.Json.JsonSerializerOptions()
    {
        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
    }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

    private readonly Utils Utils = new();

    public TimezoneControllerTest(Server<Program> server) : base(server)
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
    public async Task GetTimezones_ReturnOk()
    {
        var data = await this.CreateTimezoneAsync();

        var response = await this.RequestAsync("http://localhost/api/Timezone", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var Timezones = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<TimezoneDto>>(json, this.options);

        Assert.NotNull(Timezones);
        Assert.NotEmpty(Timezones);
        Assert.Contains(Timezones, x => x.Id == data.Id);
    }

    [Fact]
    public async Task GetTimezoneById_ReturnOk()
    {
        var data = await this.CreateTimezoneAsync();

        var response = await this.RequestAsync($"http://localhost/api/Timezone/{data.Id}", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var timezone = System.Text.Json.JsonSerializer.Deserialize<TimezoneDto>(json, this.options);

        Assert.NotNull(timezone);
        Assert.Equal(data.Id, timezone.Id);
        Assert.Equal(data.Name, timezone.Name);
    }

    [Fact]
    public async Task CreateTimezone_ReturnNoContent()
    {
        var data = Utils.CreateTimezone;

        var json = System.Text.Json.JsonSerializer.Serialize(data, this.options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync("http://localhost/api/Timezone", content, HttpMethod.Post);

        var Timezone = await this.GetRecordAsync(data.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Equal(data.Id, Timezone.Id);
        Assert.Equal(data.Name, Timezone.Name);
    }

    [Fact]
    public async Task UpdateTimezone_ReturnNoContent()
    {
        var TimezoneCreated = await this.CreateTimezoneAsync();

        var data = Utils.UpdateTimezone;

        var json = System.Text.Json.JsonSerializer.Serialize(data, this.options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync($"http://localhost/api/Timezone/{TimezoneCreated.Id}", content, HttpMethod.Put);

        var Timezone = await this.GetRecordAsync(data.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Equal(data.Id, Timezone.Id);
        Assert.Equal(data.Name, Timezone.Name);
    }

    [Fact]
    public async Task DeleteTimezone_ReturnNoContent()
    {
        var TimezoneCreated = await this.CreateTimezoneAsync();

        var response = await this.RequestAsync($"http://localhost/api/Timezone/{TimezoneCreated.Id}", null, HttpMethod.Delete);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    private async Task<CreateTimezoneDto> CreateTimezoneAsync()
    {
        var data = Utils.CreateTimezone;

        var json = System.Text.Json.JsonSerializer.Serialize(data, this.options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        await this.RequestAsync("http://localhost/api/Timezone", content, HttpMethod.Post);

        return data;
    }

    private async Task<TimezoneDto> GetRecordAsync(Guid id)
    {
        var response = await this.RequestAsync($"http://localhost/api/Timezone/{id}", null, HttpMethod.Get);

        var json = await response.Content.ReadAsStringAsync();

        return System.Text.Json.JsonSerializer.Deserialize<TimezoneDto>(json, this.options)!;
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