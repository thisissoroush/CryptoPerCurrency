using CryptoPerCurrency.Api.Services.Exchanges.ExchangeRate;

namespace CryptoPerCurrency.Api.Services.Exchanges.CoinMarketCap;

public interface ICoinMarketCapClient
{
    //Get Price per Currency
    ValueTask<decimal> GetLatestRatesAsync(CancellationToken ct, string symbol, string currency);
    
    //Get Symbols
    Task<CoinMarketCapSymbolResponse> GetSymbolsAsync(CancellationToken ct);
}