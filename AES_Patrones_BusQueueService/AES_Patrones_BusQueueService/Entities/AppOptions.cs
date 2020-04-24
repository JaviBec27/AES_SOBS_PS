namespace AES_Patrones_BusQueueService.Entities
{
    public class AppOptions
    {
        public string BusConnectionString { get; set; }
        public string TokenID { get; set; }
        public string QueueName { get; set; }
        public string TopicName { get; set; }
        public string SubscriptionName { get; set; }
        public string RuleName { get; set; }
        public string RuleFilterValue { get; set; }
    }
}
