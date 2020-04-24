using AES_Patrones_BusQueueService;
using AES_Patrones_BusQueueService.Entities;
using Message_Daemon.Models;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Message_Daemon.Providers
{
    public class MessageReceptionProvider
    {
        private readonly IConfigurationRoot settingsCache;
        private readonly AppOptions appOptions;        
        private readonly AES_SOBS_PSContext aes_Sobs_PSContext;
        private readonly string AppURL;
        private List<Message> _lstMessage;

        public MessageReceptionProvider()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", false, true);

            settingsCache = builder.Build();
            appOptions = new AppOptions();
            settingsCache.Bind(appOptions);
            this.aes_Sobs_PSContext = new AES_SOBS_PSContext();
            AppURL = "";
        }

        public List<Message> LstMessage { get => _lstMessage; set => _lstMessage = value; }

        public async Task GetMessages(CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var administradorMensajes = new ManageMessage();
                    var _lstRespuestaCotizacion = new List<RespuestaCotizacion>();
                    var verificacionMensajesRecibidos = await administradorMensajes.CheckMessagesReceived(appOptions.SubscriptionName, appOptions.TokenID);

                    if (verificacionMensajesRecibidos.Count <= 0)
                        return;

                    _lstMessage = administradorMensajes.ReceiveMessages(verificacionMensajesRecibidos, appOptions.TokenID);

                    foreach (Message message in _lstMessage)
                    {
                        var respuestaCotizacion = JsonConvert.DeserializeObject<RespuestaCotizacion>(Encoding.UTF8.GetString(message.Body));
                        _lstRespuestaCotizacion.Add(respuestaCotizacion);
                    }

                    aes_Sobs_PSContext.RespuestaCotizacion.AddRange(_lstRespuestaCotizacion);
                    await aes_Sobs_PSContext.SaveChangesAsync();
                    await SendQuotation(_lstRespuestaCotizacion);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task SendQuotation(List<RespuestaCotizacion> lstRespuestaCotizacion)
        {
            try
            {                
                var jsonDatosPDF = Newtonsoft.Json.JsonConvert.SerializeObject(lstRespuestaCotizacion, Newtonsoft.Json.Formatting.Indented);

                //Creamos el cliente http para consumir el api
                var httpClient = new HttpClient();
                HttpContent httpContent = new StringContent(jsonDatosPDF, Encoding.UTF8, "application/json");
                var messageResponse = await httpClient.PostAsync("http://localhost:64293/api/ManageMessage/NotifyReplyQuote/Reply", httpContent).ConfigureAwait(false);

                //HttpClient _httpClient = new HttpClient();
                //var json = JsonConvert.SerializeObject(lstRespuestaCotizacion);
                //var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                //var response = await _httpClient.PostAsync("http://localhost:64293/api/ManageMessage/NotifyReplyQuote/Reply", stringContent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
