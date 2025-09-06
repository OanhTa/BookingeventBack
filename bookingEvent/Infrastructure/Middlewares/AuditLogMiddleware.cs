using System.Diagnostics;
using System.Text.Json;
using bookingEvent.Model;
using bookingEvent.Services;

public class AuditLogMiddleware
{
    private readonly RequestDelegate _next;

    public AuditLogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AuditLogService auditLogService)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var log = new AuditLog
        {
            Id = Guid.NewGuid(),
            UserName = context.User.Identity?.Name ?? "Anonymous",
            ApplicationName = "BookingAPI",
            HttpMethod = context.Request.Method,
            Url = context.Request.Path + context.Request.QueryString,
            ClientIpAddress = context.Connection.RemoteIpAddress?.ToString(),
            CorrelationId = context.TraceIdentifier,
            ExecutionTime = DateTime.UtcNow
        };

        try
        {
            await _next(context);

            // Ghi status code sau khi pipeline chạy xong
            log.StatusCode = context.Response.StatusCode;
            log.HasException = false;
        }
        catch (Exception ex)
        {
            log.StatusCode = 500;
            log.HasException = true;
            log.ExceptionMessage = ex.Message;

            // vẫn ném lại để middleware ExceptionHandler xử lý
            throw;
        }
        finally
        {
            stopwatch.Stop();
            log.ExecutionDuration = (int)stopwatch.ElapsedMilliseconds;

            await auditLogService.LogAsync(log);
        }
    }
}
