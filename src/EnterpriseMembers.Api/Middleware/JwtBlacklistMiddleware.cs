using EnterpriseMembers.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace EnterpriseMembers.Api.Middleware;

public class JwtBlacklistMiddleware
{
    private readonly RequestDelegate _next;

    public JwtBlacklistMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITokenBlacklistService blacklistService)
    {
        // Only check if user is authenticated
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                var isBlacklisted = await blacklistService.IsTokenBlacklistedAsync(token);

                if (isBlacklisted)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Token has been revoked");
                    return;
                }
            }
        }

        await _next(context);
    }
}
