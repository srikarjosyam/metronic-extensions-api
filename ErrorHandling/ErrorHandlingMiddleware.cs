namespace metronic_extensions_api.ErrorHandling
{
    public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;

    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        this.next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            //.......
            _logger.LogError(ex.Message);
        }
        finally
        {
            var statusCode = context.Response.StatusCode;
            //catch error
        }
    }
}

public static class ErrorHandlingExtensions
{
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}
}

