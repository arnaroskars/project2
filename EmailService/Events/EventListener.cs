using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using EmailService.EmailDb;
using EmailService.Models;

namespace EmailService.Events
{
    public class OrderCreatedEventListener : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<OrderCreatedEventListener> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        private const string OrderQueueName = "order_created_queue";
        private const string PaymentSuccessQueueName = "payment_success_queue";
        private const string PaymentFailureQueueName = "payment_failure_queue";

        private readonly ConcurrentDictionary<string, string> _orderStates = new();

        public OrderCreatedEventListener(ILogger<OrderCreatedEventListener> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;

            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            SetupQueue("order_exchange", OrderQueueName);
            SetupQueue("payment_success", PaymentSuccessQueueName);
            SetupQueue("payment_failure", PaymentFailureQueueName);
        }

        private void SetupQueue(string exchangeName, string queueName)
        {
            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);
            _channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false
            );
            _channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: "");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ListenToQueue(OrderQueueName, "OrderCreatedEvent");
            ListenToQueue(PaymentSuccessQueueName, "PaymentSuccessEvent");
            ListenToQueue(PaymentFailureQueueName, "PaymentFailureEvent");

            return Task.CompletedTask;
        }

        private void ListenToQueue(string queueName, string eventType)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("Received {EventType} with message: {Message}", eventType, message);

                try
                {
                    var orderEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(message);
                    if (orderEvent?.OrderId != null)
                    {
                        if (eventType == "OrderCreatedEvent")
                        {
                            _orderStates[orderEvent.OrderId.ToString()] = "Created";
                            _logger.LogInformation("OrderCreatedEvent processed for Order ID: {OrderId}", orderEvent.OrderId);
                            _logger.LogInformation("Sending to Merchant {MerchantId} & Buyer {BuyerId}", orderEvent.MerchantId, orderEvent.BuyerId);


                            LogEmailMessage("Order has been created", $"Order ID: {orderEvent.OrderId}, Product: NameMissing, Price: {orderEvent.TotalPrice}");
                        }
                        else
                        {
                            await Task.Delay(500);  // Delay to ensure OrderCreatedEvent is processed first
                            await ProcessPaymentEvent(eventType, orderEvent);
                        }
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Error deserializing message: {Message}", message);
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        private async Task ProcessPaymentEvent(string eventType, OrderCreatedEvent orderEvent)
        {
            var orderId = orderEvent.OrderId.ToString();
            if (_orderStates.ContainsKey(orderId))
            {
                string status = eventType == "PaymentSuccessEvent" ? "PaymentSuccess" : "PaymentFailure";

                _orderStates[orderId] = status;
                _logger.LogInformation("{Status} processed for Order ID: {OrderId}", status, orderId);

                if (status == "PaymentSuccess")
                {
                    LogEmailMessage("Order has been purchased", $"Order {orderEvent.OrderId} has been successfully purchased");
                }
                else if (status == "PaymentFailure")
                {
                    LogEmailMessage("Order purchase failed", $"Order {orderEvent.OrderId} purchase has failed");
                }

                _orderStates.TryRemove(orderId, out _);
            }
            else
            {
                _logger.LogWarning("Payment event received for untracked or already processed Order ID: {OrderId}", orderId);
            }
        }

        private void LogEmailMessage(string subject, string body)
        {
            _logger.LogInformation("Sending Email:\nSubject: {Subject}\nBody: {Body}", subject, body);
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
