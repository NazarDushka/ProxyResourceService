namespace ProxyResource.Middleware;

using ProxyResource.Models;
using Serilog;
using ProxyResource.Interfaces;
using System.Text.Json;

public class HeaderValidationMiddleware
{
    private readonly RequestDelegate _next;

    public HeaderValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Если заголовка нет, сохраняем флаг в контексте
        if (!context.Request.Headers.ContainsKey("Full-Header"))
        {
            Log.Information("Missing Full-Header.");
            // Сохраняем информацию в Items для использования в контроллере
            context.Items["FullHeaderPresent"] = false;
        }
        else
        {
            // Заголовок присутствует
            Log.Information("Full-Header found.");
            context.Items["FullHeaderPresent"] = true;
        }

        // Передаем управление дальше
        await _next(context);
    }
}

