using bookingEvent.Data;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace bookingEvent.Infrastructure.Middlewares;
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
        var principal = context.User;
        if (principal?.Identity?.IsAuthenticated != true)
        {
            await _next(context);
            return;
        }

        // Lấy userId từ claims
        var userIdStr = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? principal.FindFirst("sub")?.Value
                        ?? principal.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userIdStr))
        {
            await _next(context);
            return;
        }

        var endpoint = context.GetEndpoint();
        var actionDesc = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();
        if (actionDesc == null)
        {
            await _next(context);
            return;
        }

        // Lấy attribute trên action (hoặc controller nếu bạn muốn mở rộng)
        var permissionAttr = actionDesc.MethodInfo
            .GetCustomAttributes(typeof(PermissionAttribute), false)
            .FirstOrDefault() as PermissionAttribute;

        if (permissionAttr == null)
        {
            await _next(context);
            return;
        }

        var permissionName = permissionAttr.PermissionName;

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var userId = Guid.Parse(userIdStr);

        // 1) Lấy permissionId theo Name (tránh Include)
        var permissionId = await db.Permissions
            .Where(p => p.Name == permissionName)
            .Select(p => (Guid?)p.Id)
            .FirstOrDefaultAsync();

        if (permissionId is null)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Permission không tồn tại hoặc chưa được cấu hình.");
            return;
        }

        // 2) Check quyền gán trực tiếp cho user
        var hasDirect = await db.UserPermissions
            .AnyAsync(up => up.UserId == userId && up.PermissionId == permissionId.Value);

        // 3) Check quyền qua role của user (join theo Id, không cần Include)
        var hasViaRole = await db.UserRoles
            .Where(ur => ur.UserId == userId)
            .Join(db.RolePermissions, ur => ur.RoleId, rp => rp.RoleId, (ur, rp) => rp)
            .AnyAsync(rp => rp.PermissionId == permissionId.Value);

        if (!(hasDirect || hasViaRole))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Bạn không có quyền thực hiện thao tác này.");
            return;
        }

        await _next(context);
    }
}
