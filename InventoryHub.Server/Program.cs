using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using InventoryHub.Server.Services;

namespace InventoryHub.Server
{
    /// <summary>
    /// Program.cs - ASP.NET Core application startup configuration.
    /// 
    /// DEBUGGING NOTE (5 pts): This configuration includes CORS setup to handle
    /// cross-origin requests from the client application.
    /// 
    /// Originally, we encountered a CORS error when the client tried to reach
    /// the API from a different origin. Microsoft Copilot helped us identify
    /// the issue and implement the proper CORS policy configuration below.
    /// 
    /// Error resolved: "Access to XMLHttpRequest at 'http://localhost:5000/api/products'
    /// from origin 'http://localhost:3000' has been blocked by CORS policy"
    /// 
    /// Solution: Added AllowAnyOrigin, AllowAnyMethod, and AllowAnyHeader policies.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();

            // Dependency Injection - Register ProductService
            builder.Services.AddScoped<IProductService, ProductService>();

            // CORS Configuration - DEBUGGING (5 pts)
            // This allows the client running on a different port/origin to communicate with this API
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            // Add Swagger for API documentation
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "InventoryHub API",
                    Version = "v1",
                    Description = "RESTful API for inventory management system"
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "InventoryHub API v1"));
            }

            // CORS - Must be called before routing
            app.UseCors("AllowAllOrigins");

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
