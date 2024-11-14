using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

using PaymentService.OrderDb;
using PaymentService.Models;


namespace PaymentService.Events
{
    public interface IEventPublisher
    {
        void PublishOrderCreated(OrderCreatedEvent orderCreatedEvent, string exchangeName);
    }

    public class EventPublisher : IEventPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private readonly IServiceScopeFactory _scopeFactory;

        public EventPublisher(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            // Declare both exchanges
            _channel.ExchangeDeclare(exchange: "payment_success", type: ExchangeType.Fanout);
            _channel.ExchangeDeclare(exchange: "payment_failure", type: ExchangeType.Fanout);        }

        public void PublishOrderCreated(OrderCreatedEvent orderCreatedEvent, string exchangeName)
        {
            var message = JsonSerializer.Serialize(orderCreatedEvent);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: exchangeName, routingKey: "", body: body);
                        // Use IServiceScopeFactory to create a new scope and access PaymentDbContext
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();

                // Log the event details to the database
                var newPayment = new Payment
                {
                    OrderId = orderCreatedEvent.OrderId,
                    TotalPrice = orderCreatedEvent.TotalPrice,
                    Status = exchangeName
                };

                dbContext.Payments.Add(newPayment);
                dbContext.SaveChanges();
            }
        }
    }
}
