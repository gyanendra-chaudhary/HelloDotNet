using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace NLogDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // create the web application builder
            var builder = WebApplication.CreateBuilder(args);
            
            // add services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            // Remove default logging providers to avoid duplicate logs
            // Clears build-in providers (console, debug, etc.)
            builder.Logging.ClearProviders();
            // Set NLog as the logging provider for the application.
            // Configures the host to use NLog
            builder.Host.UseNLog();
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.MapControllers();
            // run the application
            app.Run();
        }
    }
}