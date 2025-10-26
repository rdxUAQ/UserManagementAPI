
using UserManagementAPI.Api.Services;
using UserManagementAPI.Api.Interfaces;
using UserManagementAPI.Api.Middleware;
using Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace UserManagementAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //serilog
            //set serilog
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/appLogs.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();



            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<EndpointUsageTracker>();

            builder.Services.AddControllers();
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            


            //TODO Swagger 

            

            //logger
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            //http loging
            builder.Host.UseSerilog();
            

            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            //app build

            var app = builder.Build();

            //Middleware
            app.UseMiddleware<RequestCountMiddleware>();
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            
            //TODO: app swagger

            // middleware: cpetition counter
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapGet("/metrics/usage", (EndpointUsageTracker tracker) =>
            {
                return Results.Json(tracker.GetCounts());
            });


            //RUN

            app.Run();

        }
    }

  }