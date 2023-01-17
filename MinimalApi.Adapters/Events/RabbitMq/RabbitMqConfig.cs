namespace MinimalApi.Adapters.Events.RabbitMq;

public class RabbitMqConfig
{
    public const string ConfigSection = "RabbitMq";
    
    public string Host { get; set; }
    public int Port { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public string VirtualHost { get; set; }
    public double ReadTimeout { get; set; }
    public double WriteTimeout { get; set; }
    public string EntityCreatedExchange { get; set; }
    public string EntityCreatedQueue { get; set; }
    public string EntityUpdatedExchange { get; set; }
    public string EntityUpdatedQueue { get; set; }
}
