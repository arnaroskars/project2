using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace OrderService.Events
{
    public interface IEventPublisher
    {
        void PublishOrderCreated(OrderCreatedEvent orderCreatedEvent);
    }

    public class EventPublisher : IEventPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public EventPublisher()
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "order_exchange", type: ExchangeType.Fanout);
        }

        public void PublishOrderCreated(OrderCreatedEvent orderCreatedEvent)
        {
            var message = JsonSerializer.Serialize(orderCreatedEvent);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "order_exchange", routingKey: "", body: body);
        }
    }
}
