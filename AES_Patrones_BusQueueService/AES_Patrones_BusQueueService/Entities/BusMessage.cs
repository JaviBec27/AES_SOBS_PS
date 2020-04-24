using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace AES_Patrones_BusQueueService.Entities
{
    public class BusMessage
    {
        private readonly string tokenID;
        private readonly string topicName;
        private readonly string subscriptionName;
        private readonly byte[] messageBody;

        public BusMessage(string tokenID, string topicName, string subscriptionName, byte[] cotizacionMessage)
        {
            this.tokenID = tokenID;
            this.topicName = topicName;
            this.subscriptionName = subscriptionName;
            this.messageBody = cotizacionMessage;
        }

        public string TopicName => topicName;

        public string SubscriptionName => subscriptionName;

        public byte[] MessageBody => messageBody;

        public string TokenID => tokenID;
    }
}
