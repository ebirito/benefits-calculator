using System;
using System.Net.Http;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ApiTests;

public class IntegrationTest : IDisposable, IClassFixture<WebApplicationFactory<Program>>
{
    private HttpClient? _httpClient;

    private readonly WebApplicationFactory<Program> _factory;

    public IntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    protected HttpClient HttpClient
    {
        get
        {
            if (_httpClient == default)
            {
                /*_httpClient = new HttpClient
                {
                    //task: update your port if necessary
                    BaseAddress = new Uri("https://localhost:7124")
                };*/
                _httpClient = _factory.CreateClient();
                _httpClient.DefaultRequestHeaders.Add("accept", "text/plain");
            }

            return _httpClient;
        }
    }

    public void Dispose()
    {
        HttpClient.Dispose();
    }
}

