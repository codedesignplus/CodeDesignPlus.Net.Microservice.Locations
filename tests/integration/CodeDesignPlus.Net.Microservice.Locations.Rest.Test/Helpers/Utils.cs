using System;
using System.Text.Json;
using CodeDesignPlus.Net.Core.Abstractions;
using NodaTime.Serialization.SystemTextJson;

namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Helpers;


public static class Utils
{

    public readonly static JsonSerializerOptions Options = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

    public static async Task CreateCurrencyAsync(this HttpClient client, FakeData data)
    {
        await client.CreateResourceAsync("Currency", data.CreateCurrency);
    }

    public static async Task CreateCountryAsync(this HttpClient client, FakeData data)
    {
        await client.CreateCurrencyAsync(data);
        await client.CreateResourceAsync("Country", data.CreateCountry);
    }

    public static async Task CreateStateAsync(this HttpClient client, FakeData data)
    {
        await client.CreateCountryAsync(data);
        await client.CreateResourceAsync("State", data.CreateState);
    }

    public static async Task CreateCityAsync(this HttpClient client, FakeData data)
    {
        await client.CreateStateAsync(data);
        await client.CreateResourceAsync("City", data.CreateCity);
    }

    public static async Task CreateLocaliyAsync(this HttpClient client, FakeData data)
    {
        await client.CreateCityAsync(data);
        await client.CreateResourceAsync("Locality", data.CreateLocality);
    }

    public static async Task CreateNeighborhoodAsync(this HttpClient client, FakeData data)
    {
        await client.CreateLocaliyAsync(data);
        await client.CreateResourceAsync("Neighborhood", data.CreateNeighborhood);
    }

    public static async Task CreateResourceAsync<T>(this HttpClient client, string service, T data)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(data, Options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        await client.RequestAsync($"http://localhost/api/{service}", content, HttpMethod.Post);
    }


    public static async Task<T> GetRecordAsync<T>(this HttpClient client, string service, Guid id)
    {
        var response = await client.RequestAsync($"http://localhost/api/{service}/{id}", null, HttpMethod.Get);

        var json = await response.Content.ReadAsStringAsync();

        return System.Text.Json.JsonSerializer.Deserialize<T>(json, Utils.Options)!;
    }

    public static async Task<HttpResponseMessage> RequestAsync(this HttpClient client, string uri, HttpContent? content, HttpMethod method)
    {
        var httpRequestMessage = new HttpRequestMessage()
        {
            RequestUri = new Uri(uri),
            Content = content,
            Method = method
        };
        httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("TestAuth");

        var response = await client.SendAsync(httpRequestMessage);

        if (!response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            throw new Exception(data);
        }

        return response;
    }
}
