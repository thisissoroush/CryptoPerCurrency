using System.Collections.Concurrent;
using CryptoPerCurrency.Api.Configurations.Currencies;
using CryptoPerCurrency.Api.Exceptions;
using CryptoPerCurrency.Api.Primitives.Models;
using CryptoPerCurrency.Api.Services.Exchanges.CoinMarketCap;
using CryptoPerCurrency.Api.Services.Exchanges.ExchangeRate;
using CryptoPerCurrency.Api.Services.Utils.MemoryCache;
using Microsoft.Extensions.Options;

namespace CryptoPerCurrency.Api.Features.Rates;

public sealed class RateProccesor
{
    private readonly IMemoryCacheService<CryptoRateModel> _cache;
    private readonly CoinMarketCapClient _client;
    private readonly CurrencySettings _currencySettings;
    private readonly int _cahceDurationInMinutes = 5;
    private readonly ConcurrentDictionary<string,decimal> _rates;

    public RateProccesor(
        IMemoryCacheService<CryptoRateModel> cache, 
        CoinMarketCapClient client, 
        IOptions<CurrencySettings> currencySettings)
    {
        _cache = cache;
        _client = client;
        _currencySettings = currencySettings.Value;
        _rates = new ConcurrentDictionary<string, decimal>();
    }

    public async Task<RateResponse> ProcessAsync(CancellationToken ct,RateRequest request)
    {

        if (string.IsNullOrEmpty(request.Symbol))
            throw new CryptoPerCurrencyException(400,"Symbol cannot be empty");
        
        var result = _cache.Get(request.Symbol);

        if (result is not null)
            return new RateResponse(result);

        var allowedCurrencies = _currencySettings.AllowedCurrencies.Distinct().ToArray();
            
        List<Task> tasks = new List<Task>();
        
        foreach (var currency in allowedCurrencies)
        {
            tasks.Add(Task.Run(() => SetRatePerCurrency(ct, request.Symbol, currency)));
        }
        
        await Task.WhenAll(tasks);
        
        result = new CryptoRateModel(request.Symbol);
        result.SetRate(_rates.ToDictionary());        
        
        _cache.Set(request.Symbol, result, TimeSpan.FromMinutes(_cahceDurationInMinutes));
        
        return new RateResponse(result);
    }

    private async Task SetRatePerCurrency(CancellationToken ct, string symbol, string currency)
    {
        var price = await _client.GetLatestRatesAsync(ct, symbol, currency);
        if (price > decimal.Zero)
            _rates.TryAdd(currency, price);
    }
}