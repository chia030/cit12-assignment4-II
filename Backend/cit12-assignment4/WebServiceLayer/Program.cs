using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add controllers and JSON support
builder.Services.AddControllers()
                .AddNewtonsoftJson(); // needed for JObject/JArray in tests

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
            policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

// Enable CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();
app.UseSwagger();
app.Run();
