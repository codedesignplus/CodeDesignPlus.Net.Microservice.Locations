using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Helpers;
using NodaTime.Serialization.SystemTextJson;

namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Controllers;

public class CurrencyControllerTest : ServerBase<Program>, IClassFixture<Server<Program>>
{
    private readonly FakeData fakeData = new();

    public CurrencyControllerTest(Server<Program> server) : base(server)
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
    public async Task GetCurrencies_ReturnOk()
    {
        await Client.CreateCurrencyAsync(fakeData);

        var response = await Client.RequestAsync("http://localhost/api/Currency", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();
        
        var currencies = System.Text.Json.JsonSerializer.Deserialize<Pagination<CurrencyDto>>(json, Utils.Options);

        Assert.NotNull(currencies);
        Assert.NotEmpty(currencies.Data);
        Assert.Contains(currencies.Data, x => x.Id == fakeData.CreateCurrency.Id);
    }

    [Fact]
    public async Task GetCurrencyById_ReturnOk()
    {
        await Client.CreateCurrencyAsync(fakeData);

        var response = await Client.RequestAsync($"http://localhost/api/Currency/{fakeData.CreateCurrency.Id}", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var currency = System.Text.Json.JsonSerializer.Deserialize<CurrencyDto>(json, Utils.Options);

        Assert.NotNull(currency);
        Assert.Equal(fakeData.CreateCurrency.Id, currency.Id);
        Assert.Equal(fakeData.CreateCurrency.Name, currency.Name);
        Assert.Equal(fakeData.CreateCurrency.Code, currency.Code);
        Assert.Equal(fakeData.CreateCurrency.Symbol, currency.Symbol);

    }

    [Fact]
    public async Task CreateCurrency_ReturnNoContent()
    {
        var data = fakeData.CreateCurrency;

        var json = System.Text.Json.JsonSerializer.Serialize(data, Utils.Options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.RequestAsync("http://localhost/api/Currency", content, HttpMethod.Post);

        var currency = await Client.GetRecordAsync<CurrencyDto>("Currency", data.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Equal(data.Id, currency.Id);
        Assert.Equal(data.Name, currency.Name);
        Assert.Equal(data.Code, currency.Code);
        Assert.Equal(data.Symbol, currency.Symbol);
    }

    [Fact]
    public async Task UpdateCurrency_ReturnNoContent()
    {
        await Client.CreateCurrencyAsync(fakeData);

        var data = fakeData.UpdateCurrency;

        var json = System.Text.Json.JsonSerializer.Serialize(data, Utils.Options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.RequestAsync($"http://localhost/api/Currency/{fakeData.CreateCurrency.Id}", content, HttpMethod.Put);

        var currency = await Client.GetRecordAsync<CurrencyDto>("Currency", data.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Equal(data.Id, currency.Id);
        Assert.Equal(data.Name, currency.Name);
        Assert.Equal(data.Code, currency.Code);
        Assert.Equal(data.Symbol, currency.Symbol);
    }

    [Fact]
    public async Task DeleteCurrency_ReturnNoContent()
    {
        await Client.CreateCurrencyAsync(fakeData);

        var response = await Client.RequestAsync($"http://localhost/api/Currency/{fakeData.CreateCurrency.Id}", null, HttpMethod.Delete);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}