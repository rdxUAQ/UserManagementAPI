using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;
using UserManagementAPI.Base;
using UserManagementAPI.Const;

namespace UserManagementAPI.Api.Middleware
{
    public class ApiKeyAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyAuthenticationMiddleware> _logger;
        private readonly string _apiKey;
        private const string API_KEY_HEADER = "X-API-Key";

        public ApiKeyAuthenticationMiddleware(
            RequestDelegate next, 
            ILogger<ApiKeyAuthenticationMiddleware> logger,
            IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _apiKey = configuration["API_KEY"] ?? throw new System.Exception("API_KEY not configured");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip authentication for Swagger endpoints
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next(context);
                return;
            }

            // Check if API Key header exists
            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var extractedApiKey))
            {
                _logger.LogWarning("API Key missing from request: {Path}", context.Request.Path);
                await WriteUnauthorizedResponse(context, "API Key is missing");
                return;
            }

            // Validate API Key
            if (!_apiKey.Equals(extractedApiKey))
            {
                _logger.LogWarning("Invalid API Key attempt from {IP}", context.Connection.RemoteIpAddress);
                await WriteUnauthorizedResponse(context, "Invalid API Key");
                return;
            }

            _logger.LogInformation("API Key validated successfully for {Path}", context.Request.Path);
            await _next(context);
        }

        private static Task WriteUnauthorizedResponse(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var response = new BaseResponse<object>
            {
                Data = null,
                Error = new BaseError
                {
                    Code = "AUTH401",
                    Description = message
                }
            };

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return context.Response.WriteAsync(jsonResponse);
        }
    }
}