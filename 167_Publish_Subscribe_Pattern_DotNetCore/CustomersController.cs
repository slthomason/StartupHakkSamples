[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private const string Exchange = "customers-service";

    public CustomersController()
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        _connection = connectionFactory.CreateConnection("customers-service-publisher");

        _channel = _connection.CreateModel();
    }

    [HttpPost]
    public IActionResult Post(CustomerInputModel model)
    {
        var customerCreated = new CustomerCreatedEvent(model.FullName, model.Email);

        var payload = JsonConvert.SerializeObject(customerCreated);
        var byteArray = Encoding.UTF8.GetBytes(payload);

        Console.WriteLine("CustomerCreatedEvent Published");

        _channel.BasicPublish(Exchange, "customer-created", null, byteArray);

        return NoContent();
    }
}