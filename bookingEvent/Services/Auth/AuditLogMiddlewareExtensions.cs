namespace bookingEvent.Services.Auth
{
    public static class AuditLogMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuditLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuditLogMiddleware>();
        }
    }
}
