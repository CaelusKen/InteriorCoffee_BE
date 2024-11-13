using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.Services.Implements;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Implements;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using InteriorCoffeeAPIs.Extensions;
using InteriorCoffeeAPIs.Middlewares;
using InteriorCoffeeAPIs.Validate;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Text.Json;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsConstant.PolicyName,
        policy => { policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod(); });
});

builder.Services.AddControllers(options =>
{
    options.OutputFormatters.RemoveType<StringOutputFormatter>();
})
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.KebabCaseLower;
    });
builder.Services.Configure<MongoDBContext>(builder.Configuration.GetSection("MongoDbSection"));

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddServices(builder.Configuration);
builder.Services.AddJwtValidation(builder.Configuration);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHangfireServices(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddConfigSwagger();

builder.Services.AddJsonSchemaValidation("Validate"); // Add JSON Schema validation from the directory (from root to Validate folder)

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>(); // Ensure this is before other middlewares
app.UseMiddleware<ValidationMiddleware>(); // Add your validation middleware here

app.UseSwagger();
app.UseSwaggerUI();

// Uncomment for production
// app.UseHttpsRedirection();

app.UseCors(CorsConstant.PolicyName);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Use Hangfire dashboard
app.UseHangfireDashboard();

app.Run();
