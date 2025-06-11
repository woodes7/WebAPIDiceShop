using Microsoft.AspNetCore.Http;
using System.Text;

public class SwaggerAuthMiddleware
{
    private readonly RequestDelegate _next;
    private const string USERNAME = "admin";
    private const string PASSWORD = "admin123";

    public SwaggerAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            string authHeader = context.Request.Headers["Authorization"];

            if (authHeader != null && authHeader.StartsWith("Basic "))
            {
                var encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
                var parts = decodedUsernamePassword.Split(':');
                var username = parts[0];
                var password = parts[1];

                if (username == USERNAME && password == PASSWORD)
                {
                    await _next(context);
                    return;
                }
            }

            context.Response.Headers["WWW-Authenticate"] = "Basic";
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
        }
        else
        {
            await _next(context);
        }
    }
}
