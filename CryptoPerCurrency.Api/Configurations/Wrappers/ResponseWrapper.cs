namespace CryptoPerCurrency.Api.Configurations.Wrappers;

public record ResponseWrapper
{
    public ResponseWrapper(
        bool isSuccess = true,
        int statusCode = 200,
        string? message = null
    )
    {
        IsSuccess = isSuccess;
        StatusCode = statusCode;
        Message = message;
    }

    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }

    public string? Message { get; set; }
}