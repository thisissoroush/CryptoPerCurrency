using CryptoPerCurrency.Api.Services.Exchanges.CoinMarketCap;

namespace CryptoPerCurrency.Api.Services.Exchanges.ExchangeRate;

public class CoinMarketCapResponse
{
    public CoinMarketCapStatus Status { get; set; }
    public Dictionary<string, CoinMarketCapData> Data { get; set; }
}