using System;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Helpers;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NodaTime.Serialization.SystemTextJson;

namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Controllers;

public class CurrencyControllerTest : ServerBase<Program>, IClassFixture<Server<Program>>
{
    private readonly System.Text.Json.JsonSerializerOptions options = new System.Text.Json.JsonSerializerOptions()
    {
        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
    }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

    private readonly Utils Utils = new();

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
        var data = await this.CreateCurrencyAsync();

        var response = await this.RequestAsync("http://localhost/api/Currency", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var currencies = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<CurrencyDto>>(json, this.options);

        Assert.NotNull(currencies);
        Assert.NotEmpty(currencies);
        Assert.Contains(currencies, x => x.Id == data.Id);
    }

    [Fact]
    public async Task GetCurrencyById_ReturnOk()
    {
        var data = await this.CreateCurrencyAsync();

        var response = await this.RequestAsync($"http://localhost/api/Currency/{data.Id}", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var currency = System.Text.Json.JsonSerializer.Deserialize<CurrencyDto>(json, this.options);

        Assert.NotNull(currency);
        Assert.Equal(data.Id, currency.Id);
        Assert.Equal(data.Name, currency.Name);
        Assert.Equal(data.Code, currency.Code);
        Assert.Equal(data.Symbol, currency.Symbol);

    }

    [Fact]
    public async Task CreateCurrency_ReturnNoContent()
    {
        var data = Utils.CreateCurrency;

        var json = System.Text.Json.JsonSerializer.Serialize(data, this.options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync("http://localhost/api/Currency", content, HttpMethod.Post);

        var currency = await this.GetRecordAsync(data.Id);

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
        var currencyCreated = await this.CreateCurrencyAsync();

        var data = Utils.UpdateCurrency;

        var json = System.Text.Json.JsonSerializer.Serialize(data, this.options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync($"http://localhost/api/Currency/{currencyCreated.Id}", content, HttpMethod.Put);

        var currency = await this.GetRecordAsync(data.Id);

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
        var currencyCreated = await this.CreateCurrencyAsync();

        var response = await this.RequestAsync($"http://localhost/api/Currency/{currencyCreated.Id}", null, HttpMethod.Delete);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    private async Task<CreateCurrencyDto> CreateCurrencyAsync()
    {
        var data = Utils.CreateCurrency;

        var json = System.Text.Json.JsonSerializer.Serialize(data, this.options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        await this.RequestAsync("http://localhost/api/Currency", content, HttpMethod.Post);

        return data;
    }

    private async Task<CurrencyDto> GetRecordAsync(Guid id)
    {
        var response = await this.RequestAsync($"http://localhost/api/Currency/{id}", null, HttpMethod.Get);

        var json = await response.Content.ReadAsStringAsync();

        return System.Text.Json.JsonSerializer.Deserialize<CurrencyDto>(json, this.options)!;
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