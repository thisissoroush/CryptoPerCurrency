using CryptoPerCurrency.Api.Configurations.Exchanges;
using CryptoPerCurrency.Api.Exceptions;
using CryptoPerCurrency.Api.Services.Exchanges.ExchangeRate;
using Microsoft.Extensions.Options;

namespace CryptoPerCurrency.Api.Services.Exchanges.CoinMarketCap;

public sealed class CoinMarketCapClient : ICoinMarketCapClient
{
    private readonly HttpClient _httpClient;
    private readonly CoinMarketCapApiSettings _settings;
    private readonly int _cancelTokenAfterSecond = 10;

    public CoinMarketCapClient(HttpClient httpClient, IOptions<CoinMarketCapApiSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public async ValueTask<decimal> GetLatestRatesAsync(CancellationToken ct, string symbol, string currency)
    {
        var requestUrl = $"{_settings.BaseUrl}/v1/cryptocurrency/quotes/latest?symbol={symbol}&convert={currency}";
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        requestMessage.Headers.Add("X-CMC_PRO_API_KEY", _settings.ApiKey); 

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        timeoutCts.CancelAfter(TimeSpan.FromSeconds(_cancelTokenAfterSecond));
        
        var response = await _httpClient.SendAsync(requestMessage, timeoutCts.Token);
        response.EnsureSuccessStatusCode();

        var coinMarketCapResponse = await response.Content.ReadFromJsonAsync<CoinMarketCapResponse>();
        
        if (coinMarketCapResponse.Data.TryGetValue(symbol, out var data))
        {
            if (data.Quote.TryGetValue(currency, out var quoteDetails))
            {
                return quoteDetails.Price; // Return the price
            }
        }

        throw new CryptoPerCurrencyException(500,"Failed to get data from CoinMarketCap");
    }

    public async Task<CoinMarketCapSymbolResponse> GetSymbolsAsync(CancellationToken ct)
    {
        var requestUrl = $"{_settings.BaseUrl}/v1/cryptocurrency/map";

        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        requestMessage.Headers.Add("X-CMC_PRO_API_KEY", _settings.ApiKey); // Authentication via header

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        timeoutCts.CancelAfter(TimeSpan.FromSeconds(_cancelTokenAfterSecond));
        
        var response = await _httpClient.SendAsync(requestMessage, timeoutCts.Token);
        response.EnsureSuccessStatusCode();

        var exchangeRateSymbolResponse = await response.Content.ReadFromJsonAsync<CoinMarketCapSymbolResponse>(cancellationToken: ct);
        return exchangeRateSymbolResponse!;
    }
}