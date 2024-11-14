using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PaymentService.Models;
using PaymentService.OrderDb;
using PaymentService.Services;


namespace PaymentService.Events
{
    public class OrderCreatedEventListener : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<OrderCreatedEventListener> _logger;
        private const string QueueName = "order_created_queue"; // Specify a constant queue name

        public OrderCreatedEventListener(ILogger<OrderCreatedEventListener> logger, IEventPublisher eventPublisher)
        {
            _logger = logger;
            _eventPublisher = eventPublisher;
            

            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare the exchange and queue
            _channel.ExchangeDeclare(exchange: "order_exchange", type: ExchangeType.Fanout);
            _channel.QueueDeclare(
                queue: QueueName,           // Set a named queue to make it visible in the UI
                durable: true,              // Persist the queue
                exclusive: false,           // Allow multiple consumers
                autoDelete: false           // Keep the queue even if no consumers are connected
            );
            _channel.QueueBind(queue: QueueName, exchange: "order_exchange", routingKey: "");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var orderCreatedEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(message);
                
                _logger.LogInformation("Raw message body: {Message}", message);
                

                // Process the event here
                _logger.LogInformation("Received OrderCreatedEvent for Order ID: {OrderId}", orderCreatedEvent?.OrderId);
                
                bool success = ValidateCreditCard(orderCreatedEvent?.CreditCard);
                // Add any processing logic as needed
                string exchangeName = success ? "payment_success" : "payment_failure";

                _eventPublisher.PublishOrderCreated(orderCreatedEvent, exchangeName);


            };

            _channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer); // Use the named queue

            return Task.CompletedTask;
        }

        private bool ValidateCreditCard(CreditCardModel creditCard)
        {
            if (creditCard == null)
            {
                _logger.LogWarning("Credit card information is missing.");
                return false;
            }

            if (!LuhnAlgorithm(creditCard.cardNumber))
            {
                _logger.LogWarning("Invalid credit card number for Order.");
                return false;
            }

            if (creditCard.expirationMonth < 1 || creditCard.expirationMonth > 12)
            {
                _logger.LogWarning("Invalid expiration month for Order.");
                return false;
            }

            if (creditCard.expirationYear < 1000 || creditCard.expirationYear > 9999)
            {
                _logger.LogWarning("Invalid expiration year for Order.");
                return false;
            }

            if (creditCard.cvc < 100 || creditCard.cvc > 999)
            {
                _logger.LogWarning("Invalid CVC for Order.");
                return false;
            }

            return true;
        }

        private bool LuhnAlgorithm(string cardNumber)
        {
            int sum = 0;
            bool alternate = false;
            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                if (!char.IsDigit(cardNumber[i]))
                {
                    _logger.LogWarning("Card number contains invalid characters.");
                    return false;
                }

                int n = int.Parse(cardNumber[i].ToString());
                if (alternate)
                {
                    n *= 2;
                    if (n > 9)
                    {
                        n -= 9;
                    }
                }
                sum += n;
                alternate = !alternate;
            }
            return (sum % 10 == 0);
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
