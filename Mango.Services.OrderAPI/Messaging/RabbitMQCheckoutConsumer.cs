using Mango.Services.OrderAPI.Messages;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.RabbitMQSender;
using Mango.Services.OrderAPI.Repository;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Mango.Services.OrderAPI.Messaging
{
    public class RabbitMQCheckoutConsumer : BackgroundService
    {
        private readonly OrderRepository _orderRepository;
        private IConnection _connetion;
        private IModel _channel;
        private readonly string checkoutMessageTopic;
        private readonly string orderPaymentProcessTopics;
        private readonly IRabbitMQOrderMessageSender _rabbitMQOrderMessageSender;

        private readonly IConfiguration _configuration;

        public RabbitMQCheckoutConsumer(OrderRepository orderRepository, IConfiguration configuration, IRabbitMQOrderMessageSender rabbitMQOrderMessageSender)
        {
            _orderRepository = orderRepository;
            _configuration = configuration;
            _rabbitMQOrderMessageSender = rabbitMQOrderMessageSender;

            checkoutMessageTopic = _configuration.GetValue<string>("CheckOutMessageTopic");
            orderPaymentProcessTopics = _configuration.GetValue<string>("OrderPaymentProcessTopics");

            var factory = new ConnectionFactory
            {
                HostName= "localhost",
                UserName = "guest",
                Password = "guest",
            };
            _connetion = factory.CreateConnection();
            _channel= _connetion.CreateModel();
   
            _channel.QueueDeclare(queue: checkoutMessageTopic, false, false, false, arguments: null);
            //_channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
           
            consumer.Received += (ch, ea) =>
            {
                try
                {
                    var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                    CheckoutHeaderDto checkoutHeaderDto = JsonConvert.DeserializeObject<CheckoutHeaderDto>(content);
                    HandleMessage(checkoutHeaderDto).GetAwaiter().GetResult();

                    _channel.BasicAck(ea.DeliveryTag, false);
                }catch (Exception ex) 
                {
                    throw;
                }
            };
            _channel.BasicConsume(checkoutMessageTopic, false, consumer);

            return Task.CompletedTask;
        }

        private async  Task HandleMessage(CheckoutHeaderDto checkoutHeaderDto)
        {

            OrderHeader orderHeader = new()
            {
                UserId = checkoutHeaderDto.UserId,
                FirstName = checkoutHeaderDto.FirstName,
                LastName = checkoutHeaderDto.LastName,
                OrderDetails = new List<OrderDetails>(),
                CardNumber = checkoutHeaderDto.CardNumber,
                CouponCode = checkoutHeaderDto.CouponCode,
                CVV = checkoutHeaderDto.CVV,
                DiscountTotal = checkoutHeaderDto.DiscountTotal,
                Email = checkoutHeaderDto.Email,
                ExpiryMonthYear = checkoutHeaderDto.ExpiryMonthYear,
                OrderTime = DateTime.Now,
                OrderTotal = checkoutHeaderDto.OrderTotal,
                PaymentStatus = false,
                Phone = checkoutHeaderDto.Phone,
                PickupDateTime = checkoutHeaderDto.PickupDateTime,

            };
            foreach (var detailList in checkoutHeaderDto.CartDetails)
            {
                OrderDetails orderDetails = new()
                {
                    ProductId = detailList.ProductId,
                    ProductName = detailList.Product.Name,
                    Price = detailList.Product.Price,
                    Count = detailList.Count
                };
                orderHeader.CartTotalItems += detailList.Count;
                orderHeader.OrderDetails.Add(orderDetails);
            }

            await _orderRepository.AddOrder(orderHeader);

            PaymentRequestMessage paymentRequestMessage = new()
            {
                Name = orderHeader.FirstName + " " + orderHeader.LastName,
                CardName = orderHeader.CardNumber,
                CVV = orderHeader.CVV,
                ExpiryMonthYear = orderHeader.ExpiryMonthYear,
                OrderId = orderHeader.OrderHeaderId,
                OrderTotal = orderHeader.OrderTotal,
                Email = orderHeader.Email
            };
            try
            {
                //await _messageBus.PublishMessage(paymentRequestMessage, orderPaymentProcessTopic);
                //await args.CompleteMessageAsync(args.Message);
                _rabbitMQOrderMessageSender.SendMessage(paymentRequestMessage, orderPaymentProcessTopics);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
