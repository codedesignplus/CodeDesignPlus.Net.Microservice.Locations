using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;
using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Helpers;

namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Controllers;

public class LocalityControllerTest : ServerBase<Program>, IClassFixture<Server<Program>>
{
    private readonly FakeData fakeData = new();

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
        await Client.CreateLocaliyAsync(fakeData);

        var response = await Client.RequestAsync($"http://localhost/api/Locality?filters=IdCity={fakeData.CreateLocality.IdCity}", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var Localities = System.Text.Json.JsonSerializer.Deserialize<Pagination<LocalityDto>>(json, Utils.Options);

        Assert.NotNull(Localities);
        Assert.NotEmpty(Localities.Data);
        Assert.Contains(Localities.Data, x => x.Id == fakeData.CreateLocality.Id);
    }

    [Fact]
    public async Task GetLocalityById_ReturnOk()
    {
        await Client.CreateLocaliyAsync(fakeData);

        var response = await Client.RequestAsync($"http://localhost/api/Locality/{fakeData.CreateLocality.Id}", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var locality = System.Text.Json.JsonSerializer.Deserialize<LocalityDto>(json, Utils.Options);

        Assert.NotNull(locality);
        Assert.Equal(fakeData.CreateLocality.Id, locality.Id);
        Assert.Equal(fakeData.CreateLocality.Name, locality.Name);
        Assert.Equal(fakeData.CreateLocality.IdCity, locality.IdCity);
    }

    [Fact]
    public async Task CreateLocality_ReturnNoContent()
    {
        await Client.CreateCityAsync(fakeData);
        
        var data = fakeData.CreateLocality;

        var json = System.Text.Json.JsonSerializer.Serialize(data, Utils.Options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.RequestAsync("http://localhost/api/Locality", content, HttpMethod.Post);

        var locality = await Client.GetRecordAsync<LocalityDto>("Locality", data.Id);

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
        await Client.CreateLocaliyAsync(fakeData);

        var data = fakeData.UpdateLocality;

        var json = System.Text.Json.JsonSerializer.Serialize(data, Utils.Options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.RequestAsync($"http://localhost/api/Locality/{fakeData.CreateLocality.Id}", content, HttpMethod.Put);

        var locality = await Client.GetRecordAsync<LocalityDto>("Locality", data.Id);

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
        await Client.CreateLocaliyAsync(fakeData);

        var response = await Client.RequestAsync($"http://localhost/api/Locality/{fakeData.CreateLocality.Id}", null, HttpMethod.Delete);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}