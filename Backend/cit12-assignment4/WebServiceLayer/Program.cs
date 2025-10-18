using Microsoft.AspNetCore.Mvc;

namespace WebServiceLayer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        // Add controllers and JSON support
        builder.Services.AddControllers()
                        .AddNewtonsoftJson(); // needed for JObject/JArray in tests

        builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
            });

        var app = builder.Build();

        // Enable CORS
        app.UseCors("AllowAll");

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}