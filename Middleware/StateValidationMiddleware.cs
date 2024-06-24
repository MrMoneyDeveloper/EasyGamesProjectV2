using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace EasyGamesProjectV2.Middleware
{
    public class StateValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public StateValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Method.Equals("GET", System.StringComparison.OrdinalIgnoreCase))
            {
                // Example: Validate if request body is not null
                if (context.Request.ContentLength == null || context.Request.ContentLength == 0)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Request body cannot be empty.");
                    return;
                }
            }

            await _next(context);
        }
    }
}
