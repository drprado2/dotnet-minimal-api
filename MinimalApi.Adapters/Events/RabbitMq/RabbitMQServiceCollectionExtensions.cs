using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinimalApi.Domain.Events;
using RabbitMQ.Client;

namespace MinimalApi.Adapters.Events.RabbitMq;

public static class RabbitMQServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionFactory>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>().GetSection(RabbitMqConfig.ConfigSection).Get<RabbitMqConfig>();
            return new ConnectionFactory() { HostName = config.Host, Port = config.Port, UserName = config.User,
                Password = config.Password, VirtualHost = config.VirtualHost, SocketReadTimeout = TimeSpan.FromMilliseconds(config.ReadTimeout),
                SocketWriteTimeout = TimeSpan.FromMilliseconds(config.WriteTimeout)
            };
        });

        services.AddSingleton<IModel>(sp =>
        {
            var connectionFactory = sp.GetRequiredService<IConnectionFactory>();
            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();
            return channel;
        });

        services.AddSingleton<IEventSender, EventSender>();

        return services;
    }
    
    public static IServiceCollection AddRabbitMQConumers(this IServiceCollection services)
    {
        services.AddTransient<QueueConsumer>();

        return services;
    }
}
