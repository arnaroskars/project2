using OrderService.Services.Interfaces;
using OrderService.Data.Interfaces;
using OrderService.Models;
using OrderService.Events;

using OrderService.Services.HttpClients.Interfaces;

namespace OrderService.Services.Implementations
{
    public class OrderServiceClass : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IInventoryServiceClient _inventoryServiceClient;
        private readonly IMerchantServiceClient _merchantServiceClient;
        private readonly IBuyerServiceClient _buyerServiceClient;

        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<OrderServiceClass> _logger;

        public OrderServiceClass(IOrderRepository orderRepository, IInventoryServiceClient inventoryServiceClient, IMerchantServiceClient merchantServiceClient, IBuyerServiceClient buyerServiceClient, IEventPublisher eventPublisher, ILogger<OrderServiceClass> logger)
        {
            _orderRepository = orderRepository;
            _inventoryServiceClient = inventoryServiceClient;
            _merchantServiceClient = merchantServiceClient;
            _buyerServiceClient = buyerServiceClient;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        private string MaskCardNumber(string cardNumber) =>
            cardNumber.Length > 4 ? new string('*', cardNumber.Length - 4) + cardNumber[^4..] : cardNumber;

        public async Task<int> CreateOrderAsync(OrderRequestModel orderRequest)
        {

            string maskedCardNumber = MaskCardNumber(orderRequest.creditCard.cardNumber);
            // _logger.LogInformation("Attempting to fetch product information for ProductId: {ProductId}", orderRequest.productId);
            var orderProduct = await _inventoryServiceClient.GetProductByIdAsync(orderRequest.productId);
            if (orderProduct == null)
            {
                _logger.LogWarning("Product not found for ProductId: {ProductId}", orderRequest.productId);

                throw new Exception("Product does not exist.");

            }

            var orderMerchant = await _merchantServiceClient.GetMerchantByIdAsync(orderRequest.merchantId);
            if (orderMerchant == null)
            {
                throw new Exception("Merchant does not exist");
            }

            var orderBuyer = await _buyerServiceClient.GetBuyerByIdAsync(orderRequest.buyerId);
            if (orderBuyer == null)
            {
                throw new Exception("Buyer does not exist");
            }

            decimal productPrice = orderProduct.price * (1 - orderRequest.discount);
            // _logger.LogInformation("Product Price retrieved: {ProductPrice}", productPrice);

            var order = new Order
            {
                ProductId = orderRequest.productId,
                MerchantId = orderRequest.merchantId,
                BuyerId = orderRequest.buyerId,
                CardNumberMasked = maskedCardNumber,
                TotalPrice = productPrice,
                CreatedAt = DateTime.UtcNow,
            };
            var createdOrderId = await _orderRepository.CreateOrderAsync(order);

            var orderCreatedEvent = new OrderCreatedEvent
            {
                OrderId = createdOrderId,
                ProductId = order.ProductId,
                MerchantId = order.MerchantId,
                BuyerId = order.BuyerId,
                TotalPrice = order.TotalPrice,
                CreditCard = orderRequest.creditCard
            };

            _eventPublisher.PublishOrderCreated(orderCreatedEvent);

            return createdOrderId;
        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            return order;
        }

    }
}