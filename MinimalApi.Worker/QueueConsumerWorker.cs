using MinimalApi.Adapters.Events.RabbitMq;

namespace MinimalApi.Worker;

public class QueueConsumerWorker : BackgroundService
{
    private readonly QueueConsumer _entityCreatedConsumer;
    private readonly RabbitMqConfig? _config;
    private bool _disposed;

    public QueueConsumerWorker(
        IConfiguration config,
        QueueConsumer entityCreatedConsumer
    )
    {
        _entityCreatedConsumer = entityCreatedConsumer;
        _config = config.GetSection("RabbitMq").Get<RabbitMqConfig>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _entityCreatedConsumer.Consume(_config.EntityCreatedQueue);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
        
        Dispose();
    }

    public override void Dispose()
    {
        if (_disposed)
            return;
        
        base.Dispose();
        _entityCreatedConsumer.Dispose();
        _disposed = true;
    }
}
