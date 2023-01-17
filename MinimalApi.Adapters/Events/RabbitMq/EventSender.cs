using System.Runtime.InteropServices;
using System.Text;
using MinimalApi.Domain.Events;
using RabbitMQ.Client;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace MinimalApi.Adapters.Events.RabbitMq;

public class EventSender : IEventSender, IDisposable
{
    private readonly RabbitMqConfig _config;
    private readonly IModel _channel;
    private static Mutex _mutex = new Mutex();
    private static readonly TimeSpan MaxMutexTimeout = TimeSpan.FromSeconds(10);


    public EventSender(IConnectionFactory connectionFactory, IConfiguration config)
    {
        var connection = connectionFactory.CreateConnection();
        _channel = connection.CreateModel();
        _config = config.GetSection("RabbitMq").Get<RabbitMqConfig>();
    }

    public Task SendEntityCreatedAsync<TEvent>(TEvent @event) where TEvent : Event
    {
        return SendEvent(_config.EntityCreatedExchange, @event);
    }

    public Task SendEntityUpdatedAsync<TEvent>(TEvent @event) where TEvent : Event
    {
        return SendEvent(_config.EntityUpdatedExchange, @event);
    }

    private Task SendEvent<TEvent>(string exchange, TEvent @event) where TEvent : Event
    {
        try
        {
            if (!_mutex.WaitOne(MaxMutexTimeout))
                return Task.FromException(new TimeoutException("Timeout waiting to release mutex"));
            
            IBasicProperties props = _channel.CreateBasicProperties();
            props.ContentType = "application/json";
            props.DeliveryMode = 2;
            props.Headers = new Dictionary<string, object>();
            props.Headers.Add("x-event-type",  @event.EventType.ToString());
            props.Headers.Add("x-correlation-id",  @event.CorrelationId);
            props.Headers.Add("x-event-id",  @event.Id.ToString());
            
            string json = JsonSerializer.Serialize(@event);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            ReadOnlyMemory<byte> memory = new ReadOnlyMemory<byte>(bytes);
            _channel.BasicPublish(exchange, @event.UniqueKey, props, memory);
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            return Task.FromException(e);
        }
        finally
        {
            _mutex.ReleaseMutex();
        }
    }

    public void Dispose()
    {
        _channel.Dispose();
        _mutex.Dispose();
    }
}
