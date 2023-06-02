using Mango.MessageBus;
using Mango.Services.PaymentAPI.Messages;
using Mango.Services.PaymentAPI.RabbitMQSender;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using PaymentProcessor;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Mango.Services.PaymentAPI.Messaging
{
    public class RabbitMQCheckoutConsumer : BackgroundService
    {
        private IConnection _connetion;
        private IModel _channel;
        private readonly string orderPaymentProcessTopics;
        private readonly string checkoutMessageTopic;
        private readonly IRabbitMQPaymentMessageSender _rabbitMQPaymentMessageSender;
        private readonly IProcessPayment _processPayment;

        private readonly IConfiguration _configuration;

        public RabbitMQCheckoutConsumer(IProcessPayment processPayment,IConfiguration configuration, IRabbitMQPaymentMessageSender rabbitMQPaymentMessageSender)
        {
            
            _configuration = configuration;
            _rabbitMQPaymentMessageSender = rabbitMQPaymentMessageSender;
            _processPayment = processPayment;

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
   
            _channel.QueueDeclare(queue: orderPaymentProcessTopics, false, false, false, arguments: null);
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

                    PaymentRequestMessage paymentRequestMessage = JsonConvert.DeserializeObject<PaymentRequestMessage>(content);
                    HandleMessage(paymentRequestMessage).GetAwaiter().GetResult();

                    _channel.BasicAck(ea.DeliveryTag, false);
                }catch (Exception ex) 
                {
                    throw;
                }
            };
            _channel.BasicConsume(checkoutMessageTopic, false, consumer);

            return Task.CompletedTask;
        }

        private async  Task HandleMessage(PaymentRequestMessage paymentRequestMessage)
        {

            var result = _processPayment.PaymentProcessor();
            UpdatePaymentResultMessage updatePaymentResultMessage = new()
            {
                Status = result,
                OrderId = paymentRequestMessage.OrderId,
                Email = paymentRequestMessage.Email
            };

            try
            {
               
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
    }
}
