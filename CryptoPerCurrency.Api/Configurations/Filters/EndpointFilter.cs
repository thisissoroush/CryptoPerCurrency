using System.Text;
using CryptoPerCurrency.Api.Configurations.Wrappers;
using CryptoPerCurrency.Api.Exceptions;

namespace CryptoPerCurrency.Api.Configurations.Filters;


public class EndpointsFilter : IEndpointFilter
{
    public EndpointsFilter()
    {
    }

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    )
    {

        try
        {
            var result = await next(context);

            return result;
        }
        catch (CryptoPerCurrencyException ex)
        {
            StringBuilder message = new(ex.Message);

            if (!string.IsNullOrEmpty(ex.TechnicalMessage))
                message.Append($"| Technical Message: {ex.TechnicalMessage}");

            return new ResponseWrapper(false, ex.StatusCode, message.ToString());
        }
        catch (Exception ex)
        {
            StringBuilder message = new();

            return new ResponseWrapper(
                false, 
                500, 
                string.Concat("An unhandled error occured! ", ex.Message));
        }
    }

}