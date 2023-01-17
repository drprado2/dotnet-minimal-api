using MinimalApi.Domain.Errors;
using MinimalApi.Domain.Events;
using MinimalApi.Domain.Events.Consumers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MinimalApi.Adapters.Events.RabbitMq;

public class QueueConsumer : IDisposable
{
    private readonly IEventFactory _eventFactory;
    private readonly IEventConsumer<Event> _consumerDispatcher;
    private readonly IModel _channel;

    public QueueConsumer(IConnectionFactory connectionFactory, IEventFactory eventFactory, IEventConsumer<Event> consumerDispatcher)
    {
        _eventFactory = eventFactory;
        _consumerDispatcher = consumerDispatcher;
        var connection = connectionFactory.CreateConnection();
        _channel = connection.CreateModel();
    }

    public void Consume(string queue)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (ch, ea) =>
        {
            await ConsumeAsync(ea);
        };
        
        string consumerTag = _channel.BasicConsume(queue, false, consumer);
    }

    private async Task ConsumeAsync(BasicDeliverEventArgs eventArgs)
    {
        try
        {
            if (!eventArgs.BasicProperties.Headers.TryGetValue("x-event-type", out object? eventType))
                _channel.BasicNack(eventArgs.DeliveryTag, false, false);

            var @event = _eventFactory.Build(System.Text.Encoding.Default.GetString(eventType as byte[] ?? Array.Empty<byte>()), eventArgs.Body.ToArray());
            await _consumerDispatcher.ConsumeAsync(@event);

            _channel.BasicAck(eventArgs.DeliveryTag, false);
        }
        catch (BusinessException e)
        {
            _channel.BasicNack(eventArgs.DeliveryTag, false, false);
        }
        catch (InvalidInputException e)
        {
            _channel.BasicNack(eventArgs.DeliveryTag, false, false);
        }
        catch (Exception e)
        {
            _channel.BasicNack(eventArgs.DeliveryTag, false, true);
        }
        finally
        {
            await Task.Yield();
        }
    }

    public void Dispose()
    {
        _channel.Dispose();
    }
}
