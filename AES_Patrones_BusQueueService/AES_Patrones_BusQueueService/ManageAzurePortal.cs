using Microsoft.Azure.Management.ServiceBus.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Microsoft.Rest;
using AES_Patrones_BusQueueService.Entities;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.Azure.Management.ServiceBus;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Collections.Generic;

namespace AES_Patrones_BusQueueService
{
    public class ManageAzurePortal
    {
        private readonly IConfigurationRoot settingsCache;
        private AppAzureOptions appOptions;
        private string tokenValue = string.Empty;
        private DateTime tokenExpiresAtUtc = DateTime.MinValue;

        public ManageAzurePortal()
        {
            /*For testing Purposes
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appAzureSettings.json", false, true);

            settingsCache = builder.Build();
            appOptions = new AppAzureOptions();
            settingsCache.Bind(appOptions);
            */
            appOptions = new AppAzureOptions();
            appOptions.TenantId = "a4ed712e-2ab7-4726-ae2b-cf4c30860016";
            appOptions.ClientId = "268e2f7c-d88f-42ca-a31e-0a05763aaf20";
            appOptions.ClientSecret = "h?zJ17_upYkMsqETRgCHV2dKH3mehq?_";
            appOptions.SubscriptionId = "f3aeec7e-d2a1-47c2-9e3f-149565095787";
            appOptions.DataCenterLocation = "Oeste de EE. UU. 2";
            appOptions.ServiceBusSkuName = SkuName.Standard;
            appOptions.NamespaceName = "pub-sub-patrones";
            appOptions.ResourceGroupName = "Pub_Sub_Patrones";
            appOptions.QueueName = "cola_session";
            appOptions.TopicName = "solicitud_cotizacion";
            appOptions.RuleName = "SessionID";
        }

        public async Task CreateResourceGroup(string resourceGroupName)
        {
            try
            {
                var token = await GetToken();
                var creds = new TokenCredentials(token);
                using (var rmClient = new ResourceManagementClient(creds)
                {
                    SubscriptionId = appOptions.SubscriptionId
                })
                {
                    var resourceGroupParams = new ResourceGroup()
                    {
                        Location = appOptions.DataCenterLocation
                    };
                    await rmClient.ResourceGroups.CreateOrUpdateAsync(resourceGroupName, resourceGroupParams);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task CreateNamespace(string namespaceName)
        {
            try
            {
                using (var sbClient = await GetServiceManagementClient())
                {
                    var namespaceParams = new SBNamespace
                    {
                        Location = appOptions.DataCenterLocation,
                        Sku = new SBSku()
                        {
                            Tier = appOptions.ServiceBusSkuTier,
                            Name = appOptions.ServiceBusSkuName
                        }
                    };
                    var nameSpaceCreated = await sbClient.Namespaces.CreateOrUpdateAsync(appOptions.ResourceGroupName, namespaceName, namespaceParams);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task CreateQueue(string queueName)
        {
            try
            {
                using (var sbClient = await GetServiceManagementClient())
                {
                    var queueParams = new SBQueue
                    {
                        EnablePartitioning = true,
                        RequiresSession = true
                    };
                    await sbClient.Queues.CreateOrUpdateAsync(appOptions.ResourceGroupName, appOptions.NamespaceName, queueName, queueParams);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task CreateTopic(string topicName)
        {
            try
            {
                using (var sbClient = await GetServiceManagementClient())
                {
                    var topicParams = new SBTopic
                    {
                        DefaultMessageTimeToLive = new TimeSpan(5, 0, 0, 0),
                        MaxSizeInMegabytes = 1024,
                        RequiresDuplicateDetection = false,
                        EnablePartitioning = false,
                        EnableBatchedOperations = false,
                        EnableExpress = false
                    };
                    await sbClient.Topics.CreateOrUpdateAsync(appOptions.ResourceGroupName, appOptions.NamespaceName, topicName, topicParams);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not create a topic...");
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        public async Task CreateSubscription(string subscriptionName, string topicName = "")
        {
            try
            {
                using (var sbClient = await GetServiceManagementClient())
                {
                    var subscriptionParams = new SBSubscription
                    {
                        Status = EntityStatus.Active,
                        MaxDeliveryCount = 50,
                        RequiresSession = true
                    };
                    await sbClient.Subscriptions.CreateOrUpdateAsync(appOptions.ResourceGroupName, appOptions.NamespaceName, topicName, subscriptionName, subscriptionParams);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteSubscription(string subscriptionName, string topicName = "")
        {
            try
            {
                using (var sbClient = await GetServiceManagementClient())
                {                    
                    await sbClient.Subscriptions.DeleteAsync(appOptions.ResourceGroupName, appOptions.NamespaceName, topicName, subscriptionName);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task CreateRule(string ruleName, string subscriptionName = "", string topicName = "")
        {
            try
            {
                using (var sbClient = await GetServiceManagementClient())
                {
                    var ruleParams = new Rule
                    {
                        SqlFilter = new Microsoft.Azure.Management.ServiceBus.Models.SqlFilter("category='"+subscriptionName+"'")
                    };
                    await sbClient.Rules.CreateOrUpdateAsync(appOptions.ResourceGroupName, appOptions.NamespaceName, topicName, subscriptionName, ruleName, ruleParams);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<string>> CheckTopics(string subscriptionNameSearch)
        {
            List<string> lstSubscriptionsMade = new List<string>();
            using (var sbClient = await GetServiceManagementClient())
            {
                var lstTopics = sbClient.Topics.ListByNamespace(appOptions.ResourceGroupName, appOptions.NamespaceName);
                foreach (SBTopic topico in lstTopics)
                {
                    var lstSubscriptions = sbClient.Subscriptions.ListByTopic(appOptions.ResourceGroupName, appOptions.NamespaceName, topico.Name);
                    foreach(SBSubscription subscription in lstSubscriptions)
                    {
                        if(subscription.Name.Equals(subscriptionNameSearch))
                            lstSubscriptionsMade.Add(topico.Name);
                    }
                }
            }
            return lstSubscriptionsMade;
        }

        private async Task<ServiceBusManagementClient> GetServiceManagementClient(){
            var token = await GetToken();
            var creds = new TokenCredentials(token);
            return new ServiceBusManagementClient(creds)
            {
                SubscriptionId = appOptions.SubscriptionId,
            };
        }

        private async Task<string> GetToken()
        {
            try
            {
                // Check to see if the token has expired before requesting one.
                // We will go ahead and request a new one if we are within 2 minutes of the token expiring.
                if (tokenExpiresAtUtc < DateTime.UtcNow.AddMinutes(-2))
                {
                    var tenantId = appOptions.TenantId;
                    var clientId = appOptions.ClientId;
                    var clientSecret = appOptions.ClientSecret;
                    var context = new AuthenticationContext($"https://login.microsoftonline.com/{tenantId}");
                    var result = await context.AcquireTokenAsync(
                        "https://management.core.windows.net/",
                        new ClientCredential(clientId, clientSecret)
                    );
                    // If the token isn't a valid string, throw an error.
                    if (string.IsNullOrEmpty(result.AccessToken))
                    {
                        throw new Exception("Token result is empty!");
                    }
                    tokenExpiresAtUtc = result.ExpiresOn.UtcDateTime;
                    tokenValue = result.AccessToken;
                }
                return tokenValue;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
