using bookingEvent.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace bookingEvent.Infrastructure.Middlewares
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;
        public PermissionMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var user = context.User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                var accountGroupId = user.FindFirst("AccountGroupId")?.Value;

                if (!string.IsNullOrEmpty(accountGroupId))
                {
                    var endpoint = context.GetEndpoint();
                    var actionDesc = endpoint?.Metadata
                        .GetMetadata<ControllerActionDescriptor>();

                    if (actionDesc != null)
                    {
                        var formName = actionDesc.ControllerName;
                        var action = actionDesc.ActionName;

                        using var scope = _scopeFactory.CreateScope();
                        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        Console.WriteLine($"[Middleware] Controller={formName}, Action={action}, AccountGroupId={accountGroupId}");


                        bool allowed = dbContext.AccountGroupPermissions
                            .Any(p => p.AccountGroupId == Guid.Parse(accountGroupId)
                                   && p.FormName == formName
                                   && p.Action == action
                                   && p.AllowAction);

                        if (!allowed)
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            await context.Response.WriteAsync("Bạn không có quyền thực hiện thao tác này.");
                            return;
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}
