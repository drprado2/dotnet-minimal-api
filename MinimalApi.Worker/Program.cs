using System.Data.Common;
using System.Globalization;
using MinimalApi.Worker;
using FluentValidation;
using Microsoft.Data.SqlClient;
using MinimalApi.Adapters.Cache;
using MinimalApi.Adapters.Events.RabbitMq;
using MinimalApi.Adapters.Storage.SqlServer;
using MinimalApi.Domain.Events;
using MinimalApi.Domain.Events.Consumers;
using Npgsql;
using StackExchange.Redis;

ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(builder =>
    {
        builder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    })
    .ConfigureServices((builder, services) =>
    {
        services.AddRabbitMQ()
            .AddRabbitMQConumers()
            .AddCacheServices()
            .AddRepositories()
            .AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetSection("Redis").GetValue<string>("ConnectionString");
                options.InstanceName = builder.Configuration.GetSection("Redis").GetValue<string>("InstanceName");
            })
            .AddSingleton<IConnectionMultiplexer>(sp => { return ConnectionMultiplexer.Connect(builder.Configuration.GetSection("Redis").GetValue<string>("ConnectionString")); })
            .AddSingleton<DbProviderFactory>(NpgsqlFactory.Instance)
            .AddSingleton<IEventConsumer<CompanyCreated>, CompanyCreatedConsumer>()
            .AddSingleton<IEventConsumer<EmployeeCreated>, EmployeeCreatedConsumer>()
            .AddSingleton<IEventFactory, EventFactory>()
            .AddSingleton<IEventConsumer<Event>, ConsumerDispatcher>();
        
        services.AddHostedService<QueueConsumerWorker>();
    })
    .Build();

await host.RunAsync();
