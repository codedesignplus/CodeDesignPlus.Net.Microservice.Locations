using System;
using CodeDesignPlus.Net.Microservice.Locations.Application.State.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Helpers;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NodaTime.Serialization.SystemTextJson;

namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Controllers;

public class StateControllerTest : ServerBase<Program>, IClassFixture<Server<Program>>
{
    private readonly System.Text.Json.JsonSerializerOptions options = new System.Text.Json.JsonSerializerOptions()
    {
        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
    }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

    private readonly Utils Utils = new();

    public StateControllerTest(Server<Program> server) : base(server)
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
    public async Task GetStates_ReturnOk()
    {
        var data = await this.CreateStateAsync();

        var response = await this.RequestAsync("http://localhost/api/State", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var states = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<StateDto>>(json, this.options);

        Assert.NotNull(states);
        Assert.NotEmpty(states);
        Assert.Contains(states, x => x.Id == data.Id);
    }

    [Fact]
    public async Task GetStateById_ReturnOk()
    {
        var data = await this.CreateStateAsync();

        var response = await this.RequestAsync($"http://localhost/api/State/{data.Id}", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var state = System.Text.Json.JsonSerializer.Deserialize<StateDto>(json, this.options);

        Assert.NotNull(state);
        Assert.Equal(data.Id, state.Id);
        Assert.Equal(data.Name, state.Name);
        Assert.Equal(data.IdCountry, state.IdCountry);
        Assert.Equal(data.Code, state.Code);
    }

    [Fact]
    public async Task CreateState_ReturnNoContent()
    {
        var data = Utils.CreateState;

        var json = System.Text.Json.JsonSerializer.Serialize(data, this.options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync("http://localhost/api/State", content, HttpMethod.Post);

        var state = await this.GetRecordAsync(data.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Equal(data.Id, state.Id);
        Assert.Equal(data.Name, state.Name);
        Assert.Equal(data.IdCountry, state.IdCountry);
        Assert.Equal(data.Code, state.Code);
    }

    [Fact]
    public async Task UpdateState_ReturnNoContent()
    {
        var stateCreated = await this.CreateStateAsync();

        var data = Utils.UpdateState;

        var json = System.Text.Json.JsonSerializer.Serialize(data, this.options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync($"http://localhost/api/State/{stateCreated.Id}", content, HttpMethod.Put);

        var state = await this.GetRecordAsync(data.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Equal(data.Id, state.Id);
        Assert.Equal(data.Name, state.Name);
        Assert.Equal(data.IdCountry, state.IdCountry);
        Assert.Equal(data.Code, state.Code);
    }

    [Fact]
    public async Task DeleteState_ReturnNoContent()
    {
        var stateCreated = await this.CreateStateAsync();

        var response = await this.RequestAsync($"http://localhost/api/State/{stateCreated.Id}", null, HttpMethod.Delete);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    private async Task<CreateStateDto> CreateStateAsync()
    {
        var data = Utils.CreateState;

        var json = System.Text.Json.JsonSerializer.Serialize(data, this.options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        await this.RequestAsync("http://localhost/api/State", content, HttpMethod.Post);

        return data;
    }

    private async Task<StateDto> GetRecordAsync(Guid id)
    {
        var response = await this.RequestAsync($"http://localhost/api/State/{id}", null, HttpMethod.Get);

        var json = await response.Content.ReadAsStringAsync();

        return System.Text.Json.JsonSerializer.Deserialize<StateDto>(json, this.options)!;
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