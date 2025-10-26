using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace UserManagementAPI.Api.Middleware
{
    public class RequestCountMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly EndpointUsageTracker _tracker;

        public RequestCountMiddleware(RequestDelegate next, EndpointUsageTracker tracker)
        {
            _next = next;
            _tracker = tracker;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            string key;

            if (endpoint != null)
            {
                // intenta usar el patrón de ruta si existe para agrupar por acción/route
                var routeEndpoint = endpoint as RouteEndpoint;
                var routePattern = routeEndpoint?.RoutePattern?.RawText;
                key = $"{context.Request.Method} {(string.IsNullOrEmpty(routePattern) ? endpoint.DisplayName : routePattern)}";
            }
            else
            {
                key = $"{context.Request.Method} {context.Request.Path}";
            }

            _tracker.Increment(key);

            await _next(context);
        }
    }
}