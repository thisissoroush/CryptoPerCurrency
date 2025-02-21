using CryptoPerCurrency.Api.Features.Rates;

namespace CryptoPerCurrency.Api.Configurations;

public static class FeaturesInjector
{
    public static void InjectFeatures(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        
        services.AddScoped<RateProccesor>();
    }
}