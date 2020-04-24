using Microsoft.Azure.Management.ServiceBus.Models;

namespace AES_Patrones_BusQueueService.Entities
{
    public class AppAzureOptions
    {
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string SubscriptionId { get; set; }
        public string DataCenterLocation { get; set; }
        public SkuName ServiceBusSkuName { get; set; }
        public SkuTier? ServiceBusSkuTier { get; set; }
        public string QueueName { get; set; }
        public string TopicName { get; set; }
        public string SubscriptionName { get; set; }
        public string RuleName { get; set; }
        public string NamespaceName { get; set; }
        public string ResourceGroupName { get; set; }
    }


}
