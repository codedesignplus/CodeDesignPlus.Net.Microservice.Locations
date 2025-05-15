using System;
using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;
using CodeDesignPlus.Net.Microservice.Locations.Application.State.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Helpers;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NodaTime.Serialization.SystemTextJson;

namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Controllers;

public class StateControllerTest : ServerBase<Program>, IClassFixture<Server<Program>>
{
    private readonly FakeData fakeData = new();

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
        await Client.CreateStateAsync(fakeData);

        var response = await Client.RequestAsync($"http://localhost/api/State?filters=IdCountry={fakeData.CreateState.IdCountry}", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var states = System.Text.Json.JsonSerializer.Deserialize<Pagination<StateDto>>(json, Utils.Options);

        Assert.NotNull(states);
        Assert.NotEmpty(states.Data);
        Assert.Contains(states.Data, x => x.Id == fakeData.CreateState.Id);
    }

    [Fact]
    public async Task GetStateById_ReturnOk()
    {
        await Client.CreateStateAsync(fakeData);

        var response = await Client.RequestAsync($"http://localhost/api/State/{fakeData.CreateState.Id}", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var state = System.Text.Json.JsonSerializer.Deserialize<StateDto>(json, Utils.Options);

        Assert.NotNull(state);
        Assert.Equal(fakeData.CreateState.Id, state.Id);
        Assert.Equal(fakeData.CreateState.Name, state.Name);
        Assert.Equal(fakeData.CreateState.IdCountry, state.IdCountry);
        Assert.Equal(fakeData.CreateState.Code, state.Code);
    }

    [Fact]
    public async Task CreateState_ReturnNoContent()
    {
        await Client.CreateCountryAsync(fakeData);

        var data = fakeData.CreateState;

        var json = System.Text.Json.JsonSerializer.Serialize(data, Utils.Options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.RequestAsync("http://localhost/api/State", content, HttpMethod.Post);

        var state = await Client.GetRecordAsync<StateDto>("State", data.Id);

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
        await Client.CreateStateAsync(fakeData);

        var data = fakeData.UpdateState;

        var json = System.Text.Json.JsonSerializer.Serialize(data, Utils.Options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.RequestAsync($"http://localhost/api/State/{fakeData.CreateState.Id}", content, HttpMethod.Put);

        var state = await Client.GetRecordAsync<StateDto>("State", data.Id);

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
        await Client.CreateStateAsync(fakeData);

        var response = await Client.RequestAsync($"http://localhost/api/State/{fakeData.CreateState.Id}", null, HttpMethod.Delete);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

}