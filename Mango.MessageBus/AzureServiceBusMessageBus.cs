using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.MessageBus
{
    public class AzureServiceBusMessageBus : IMessageBus
    {
        //can be improved to appsetting
        private string connectionString = "Endpoint=sb://mangorestaurantmircoservice.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=a3nY+cqzp3ZDv/pUDgz6oCgN35CE60NEl+ASbDamI5c=";
        public async Task PublishMessage(BaseMessage message, string topicName)
        {
            ISenderClient sendClient = new TopicClient(connectionString, topicName);
            var JsonMessage = JsonConvert.SerializeObject(message);
            var finalMessage = new Message(Encoding.UTF8.GetBytes(JsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };

            await sendClient.SendAsync(finalMessage);   

            await sendClient.CloseAsync();
        }

 
    }
}
