using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Data.SqlClient;
using MinimalApi.Adapters.Cache;
using MinimalApi.Adapters.Events.RabbitMq;
using MinimalApi.Adapters.Storage.SqlServer;
using MinimalApi.Domain.UseCases;
using MinimalApi.Domain.UseCases.Commands;
using MinimalApi.Domain.UseCases.Queries;
using MinimalApi.Domain.UseCases.Validations;
using MinimalApi.Observability;
using MinimalApi.WebApi.Middlewares;
using MinimalApi.WebApi.Router;
using Npgsql;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddRabbitMQ()
    .AddCacheServices()
    .AddRepositories()
    .AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetSection("Redis").GetValue<string>("ConnectionString");
        options.InstanceName = builder.Configuration.GetSection("Redis").GetValue<string>("InstanceName");
    })
    .AddSingleton<IConnectionMultiplexer>(sp => { return ConnectionMultiplexer.Connect(builder.Configuration.GetSection("Redis").GetValue<string>("ConnectionString")); })
    .AddSingleton<DbProviderFactory>(NpgsqlFactory.Instance)
    .AddSingleton<MinimalApi.Domain.UseCases.Validations.IValidator<CreateCompanyCmd>, CreateCompanyCmdValidator>()
    .AddSingleton<MinimalApi.Domain.UseCases.Validations.IValidator<CreateEmployeeCmd>, CreateEmployeeCmdValidator>()
    .AddSingleton<MinimalApi.Domain.UseCases.Validations.IValidator<GetCompanyEmployeesQuery>, GetCompanyEmployeesQueryValidator>()
    .AddSingleton<IUseCaseHandler<CreateCompanyCmd>, CreateCompanyHandler>()
    .AddSingleton<IUseCaseHandler<CreateEmployeeCmd>, CreateEmployeeHandler>()
    .AddSingleton<IUseCaseHandler<GetCompanyByIdQuery>, GetCompanyByIdHandler>()
    .AddSingleton<IUseCaseHandler<GetCompanyEmployeesQuery>, GetCompanyEmployeesHandler>()
    .AddSingleton<IUseCaseHandler<GetEmployeeByIdQuery>, GetEmployeeByIdHandler>();

builder.Services
    .AddHealthChecks()
    // .AddSqlServer(builder.Configuration.GetConnectionString("MinimalApi"), "SELECT 1;", null, "Database", null, new[] { "ready" })
    .AddNpgSql(builder.Configuration.GetConnectionString("MinimalApiPg"), "SELECT 1;", null, "Database", null, new[] { "ready" })
    .AddRedis(builder.Configuration.GetSection("Redis").GetValue<string>("ConnectionString"), "Redis", null, new[] { "ready" })
    .AddRabbitMQ("RabbitMq", null, new[] { "ready" });

builder.Services
    .AddTracing()
    .AddMetrics();

// builder.AddAppLogging();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.Configure<JsonOptions>(options => { options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<MdcMiddleware>();
app.UseMiddleware<ErrorMiddleware>();
app.UseMiddleware<LogMiddleware>();

app
    .MapCompanyRoutes()
    .MapEmployeeRoutes();

app.MapHealthChecks("/healthz/ready", new HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("ready"),
    AllowCachingResponses = false
});

app.MapHealthChecks("/healthz/live", new HealthCheckOptions
{
    Predicate = _ => false,
    AllowCachingResponses = false
});

app.Run();
