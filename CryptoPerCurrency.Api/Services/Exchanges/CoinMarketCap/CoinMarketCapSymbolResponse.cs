using CryptoPerCurrency.Api.Services.Exchanges.CoinMarketCap;

namespace CryptoPerCurrency.Api.Services.Exchanges.ExchangeRate;

public class CoinMarketCapSymbolResponse
{
        public CoinMarketCapStatus Status { get; set; } = new();
        public List<CoinMarketCapSymbolData> Data { get; set; } = new();
}