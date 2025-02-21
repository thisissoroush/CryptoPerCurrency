using CryptoPerCurrency.Api.Configurations.Currencies;
using CryptoPerCurrency.Api.Configurations.Exchanges;
using CryptoPerCurrency.Api.Services.Exchanges.CoinMarketCap;
using CryptoPerCurrency.Api.Services.Exchanges.ExchangeRate;
using CryptoPerCurrency.Api.Services.Utils.MemoryCache;
using CryptoPerCurrency.Api.Services.Utils.Quartz;
using Quartz;

namespace CryptoPerCurrency.Api.Configurations;

public static class ServiceInjector
{
    public static void InjectServices(this WebApplicationBuilder builder)
    {
        builder.Services.InjectMemoryCacheServices();
        builder.InjectAllowedCurrencies();
        builder.InjectExchangeClients();
        // builder.InjectQuartz();
        
     
    }

    public static void InjectExchangeClients(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<CoinMarketCapApiSettings>(
            builder.Configuration.GetSection("CoinMarketCapApiSettings"));
        
        builder.Services.AddHttpClient<CoinMarketCapClient>();
    }

    public static void InjectAllowedCurrencies(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<CurrencySettings>(
            builder.Configuration.GetSection("CurrencySettings"));
    }

    private static void InjectMemoryCacheServices(this IServiceCollection services)
    {
        services.AddMemoryCache();

        services.AddScoped(typeof(IMemoryCacheService<>), typeof(MemoryCacheService<>));
    }

    private static void InjectQuartz(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<QuartzSettings>(builder.Configuration.GetSection("QuartzSettings"));
        
        
        builder.Services.AddQuartz(q =>
        {
            var jobKey = new JobKey("RateJob");
            
            q.AddJob<CryptoRatesJobs>(opts => opts.WithIdentity(jobKey));

            
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("RateJobTrigger")
                .WithSimpleSchedule(x =>
                    x.WithIntervalInSeconds(builder.Configuration.GetValue<int>("QuartzSettings:IntervalSeconds"))
                        .WithMisfireHandlingInstructionNowWithExistingCount()
                        .RepeatForever()
                )
                .StartNow());
        });

        builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }
}