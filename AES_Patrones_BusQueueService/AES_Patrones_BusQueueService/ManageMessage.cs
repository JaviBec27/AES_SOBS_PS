using AES_Patrones_BusQueueService.Entities;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AES_Patrones_BusQueueService
{
    public class ManageMessage
    {
        private readonly IConfigurationRoot settingsCache;
        private readonly AppOptions appOptions;

        public ManageMessage()
        {
            /*For testing purposes
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", false, true);

            settingsCache = builder.Build();
            appOptions = new AppOptions();
            settingsCache.Bind(appOptions);*/
            appOptions = new AppOptions();
            appOptions.BusConnectionString = "Endpoint=sb://pub-sub-patrones.servicebus.windows.net/;SharedAccessKeyName=SendAndReceive;SharedAccessKey=0NNAunFdr/X6ABC4A0qGUYKkGvBAFo38dLYOi7Dhe7c=";
            appOptions.RuleName = "SessionID";
        }

        public void SendMessages(List<BusMessage> appMessages)
        {
            Task[] arrayTaskMessages = new Task[appMessages.Count];
            Action<object> delegeteSendMessage = (messageInformation) =>
            {
                IMessageSender messageSender;
                BusMessage messageInfo = (BusMessage)messageInformation;
                messageSender = new MessageSender(appOptions.BusConnectionString, messageInfo.TopicName);
                var messagesToSend = new List<Message>();
                var message = new Message(messageInfo.MessageBody);
                message.SessionId = messageInfo.TokenID;
                message.UserProperties.Add(appOptions.RuleName, messageInfo.SubscriptionName);
                messagesToSend.Add(message);
                messageSender.SendAsync(messagesToSend).GetAwaiter().GetResult();
                messageSender.CloseAsync().GetAwaiter().GetResult();
            };
            for (int i = 0; i < arrayTaskMessages.Length; i++)
            {
                arrayTaskMessages[i] = Task.Factory.StartNew(delegeteSendMessage, appMessages[i]);
            }
            Task.WaitAll(arrayTaskMessages);
        }

        public async Task SendResponseMessagesAsync(BusMessage messageInfo)
        {            
            IMessageSender messageSender;            
            messageSender = new MessageSender(appOptions.BusConnectionString, messageInfo.TopicName);
            var messagesToSend = new List<Message>();
            var message = new Message(messageInfo.MessageBody);
            message.SessionId = messageInfo.TokenID;
            message.UserProperties.Add(appOptions.RuleName, messageInfo.SubscriptionName);
            messagesToSend.Add(message);
            await messageSender.SendAsync(messagesToSend);
            await messageSender.CloseAsync();
        }

        public List<Message> ReceiveMessages(Dictionary<string, string> topicAndSubscriptionReceived, string tokenID)
        {
            if (topicAndSubscriptionReceived.Count < 1)
                return null;

            List<Message> listMessage = new List<Message>();
            Action<object> delegateGetMessages = (topicSubscriptionMessage) =>
            {
                var messageFound = (KeyValuePair<string, string>)topicSubscriptionMessage;
                ISessionClient sessionClient = new SessionClient(appOptions.BusConnectionString, EntityNameHelper.FormatSubscriptionPath(messageFound.Key, messageFound.Value), ReceiveMode.PeekLock);
                IMessageSession session = sessionClient.AcceptMessageSessionAsync(tokenID).GetAwaiter().GetResult();                
                while (true)
                {
                    Message message = session.ReceiveAsync(new TimeSpan(0, 0, 20)).GetAwaiter().GetResult();
                    if (message == null)
                    {
                        session.CloseAsync().GetAwaiter().GetResult();
                        sessionClient.CloseAsync().GetAwaiter().GetResult();
                        break;
                    }
                    else
                    {
                        session.CompleteAsync(message.SystemProperties.LockToken).GetAwaiter().GetResult();
                        listMessage.Add(message);
                    }
                }
            };

            Task[] taskArray = new Task[topicAndSubscriptionReceived.Count];
            List<KeyValuePair<string, string>> lstTopicAndSubscriptionReceived = new List<KeyValuePair<string, string>>();

            foreach (KeyValuePair<string, string> topicAndSubscription in topicAndSubscriptionReceived)
            {
                lstTopicAndSubscriptionReceived.Add(topicAndSubscription);
            }

            for (int i = 0; i < taskArray.Length; i++)
            {
                taskArray[i] = Task.Factory.StartNew(delegateGetMessages, lstTopicAndSubscriptionReceived[i]);
            }
            Task.WaitAll(taskArray);

            return listMessage;
        }

        public async Task<Dictionary<string, string>> CheckMessagesReceived(string subscriptionName, string tokenID)
        {
            List<Message> message = new List<Message>();
            Dictionary<string, string> topicAndSubscriptionFound = new Dictionary<string, string>();
            var topicsRegistered = await new ManageAzurePortal().CheckTopics(subscriptionName);

            if (topicsRegistered.Count > 0)
            {
                Action<object> delegateCheck = (topic) =>
                {
                    ISessionClient sessionClient = new SessionClient(appOptions.BusConnectionString, EntityNameHelper.FormatSubscriptionPath(topic.ToString(), subscriptionName), ReceiveMode.PeekLock);
                    IMessageSession session = sessionClient.AcceptMessageSessionAsync(tokenID).GetAwaiter().GetResult();
                    if (session != null)
                    {
                        message = (List<Message>)session.ReceiveAsync(1, new TimeSpan(0, 0, 20)).GetAwaiter().GetResult();
                        if (message == null)
                        {
                            session.CloseAsync().GetAwaiter().GetResult();
                            sessionClient.CloseAsync().GetAwaiter().GetResult();
                        }
                        else
                        {
                            topicAndSubscriptionFound.Add(topic.ToString(), subscriptionName);
                            session.CloseAsync().GetAwaiter().GetResult();
                            sessionClient.CloseAsync().GetAwaiter().GetResult();
                        }
                    }
                };

                Task[] taskArray = new Task[topicsRegistered.Count];
                for (int i = 0; i < taskArray.Length; i++)
                {
                    taskArray[i] = Task.Factory.StartNew(delegateCheck, topicsRegistered[i]);
                }
                Task.WaitAll(taskArray);
            }
            return topicAndSubscriptionFound;
        }
    }
}
