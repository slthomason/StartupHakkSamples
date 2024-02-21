public class NotifyCustomerCreatedSubscriber : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private const string Queue = "notifications-service/customer-created";

    public NotifyCustomerCreatedSubscriber()
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        _connection = connectionFactory.CreateConnection("notifications-service-customer-created-consumer");

        _channel = _connection.CreateModel();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (sender, eventArgs) =>
        {
            var contentArray = eventArgs.Body.ToArray();
            var contentString = Encoding.UTF8.GetString(contentArray);
            var message = JsonConvert.DeserializeObject<CustomerCreatedEvent>(contentString);

            Console.WriteLine($"Message CustomerCreatedEvent received with Email {message.Email}");

            _channel.BasicAck(eventArgs.DeliveryTag, false);
        };

        _channel.BasicConsume(Queue, false, consumer);

        return Task.CompletedTask;
    }
}