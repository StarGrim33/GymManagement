using Serilog.Context;

namespace GymManagement.Api.Middleware;

public class RequestContextLoggingMiddleware(RequestDelegate next)
{
    private const string CorrelationIdHeader = "X-CorrelationId-Id";

    public Task Invoke(HttpContext httpContext)
    {
        using (LogContext.PushProperty(CorrelationIdHeader, GetCorrelationId(httpContext)))
        {
            return next(httpContext);
        }

        static string GetCorrelationId(HttpContext httpContext)
        {
            return httpContext
                .Request
                .Headers.TryGetValue(CorrelationIdHeader, out var correlationId)
                ? correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier
                : httpContext.TraceIdentifier;
        }
    }
}