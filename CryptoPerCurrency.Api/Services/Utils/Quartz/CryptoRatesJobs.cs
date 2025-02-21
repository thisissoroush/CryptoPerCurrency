using CryptoPerCurrency.Api.Features.Rates;
using CryptoPerCurrency.Api.Services.Exchanges.CoinMarketCap;
using CryptoPerCurrency.Api.Services.Utils.MemoryCache;
using Quartz;

namespace CryptoPerCurrency.Api.Services.Utils.Quartz;

[DisallowConcurrentExecution]
public class CryptoRatesJobs : IJob
{
    private readonly IMemoryCacheService<string[]> _memoryCache;
    private readonly CoinMarketCapClient _client;
    private readonly RateProccesor _rateProccesor;
    private readonly CancellationToken _cancelationToken = new CancellationTokenSource().Token;
    private readonly string _symbolsKey = "Symbols";
    private readonly int _symbolsCacheDurationInMinute = 60;
    private readonly int _symbolsMaxRank = 100;
    private readonly int _rateLimiterDelay = 3000;
    
    public CryptoRatesJobs(
        CoinMarketCapClient client, 
        RateProccesor rateProccesor, 
        IMemoryCacheService<string[]> memoryCache)
    {
        _client = client;
        _rateProccesor = rateProccesor;
        _memoryCache = memoryCache;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        var result = _memoryCache.Get(_symbolsKey);

        if (result is null)
        { 
            var response = await _client.GetSymbolsAsync(_cancelationToken);
            
            result = response
                .Data
                .Where(r => r.Rank <= _symbolsMaxRank)
                .Select(r => r.Symbol)
                .Distinct()
                .ToArray();
            
            _memoryCache.Set(_symbolsKey,result, TimeSpan.FromMinutes(_symbolsCacheDurationInMinute));    
        }
           
        List<Task> tasks = new List<Task>();
        foreach (var symbol in result)
        {
            tasks.Add(Task.Run(() => InvokeProcessor(symbol)));
        }
        
        await Task.WhenAll(tasks);
    }

    private async Task InvokeProcessor(string symbol)
    {
        await Task.Delay(_rateLimiterDelay);
        await _rateProccesor.ProcessAsync(_cancelationToken, new RateRequest(symbol));
    }
    

}