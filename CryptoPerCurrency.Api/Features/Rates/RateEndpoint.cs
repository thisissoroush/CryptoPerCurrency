namespace CryptoPerCurrency.Api.Features.Rates;

public static class RateEndpoint
{
    public static void RegisterEndPoints(this RouteGroupBuilder api)
    {
        api.MapGet("/rate", async ([AsParameters]RateRequest request, RateProccesor processor, CancellationToken ct) =>
        {
            var response = await processor.ProcessAsync(ct,request);
            return Results.Ok(response);
        });
    }

}