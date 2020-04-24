using AES_Patrones_BusQueueService;
using AES_Patrones_BusQueueService.Entities;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using ProveedorA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedorA
{
    public class ImplementacionProveedor
    {
        private readonly string TokenID;
        private readonly string SubscriptionName;
        private readonly string SubscriptionSendName;
        private readonly string TopicSendMessage;
        private readonly string TokenIDSend;
        private readonly Proveedor_AContext context;
        private readonly ManageMessage administradorMensajes;
        
        public ImplementacionProveedor()
        {            
            TokenID = "C3bM7tTcF81Y429dVLWn8nNiwJugR4D6rNPnOXAuZOA";
            TokenIDSend = "H-jsdfbKLiRvc_vXshLQpC1QnUQlpwleDfFNU5wjnyMhu4";
            SubscriptionName = "german_rodrigo";
            TopicSendMessage = "respuesta_proveedores";
            SubscriptionSendName = "respuesta_proveedores";
            administradorMensajes = new ManageMessage();
            context = new Proveedor_AContext();
        }

        public async Task run()
        {
            await recibirMensajes();
        }

        public async Task recibirMensajes()
        {
            try
            {
                Console.WriteLine(DateTime.Now.ToString());
                Console.WriteLine("=========================================================");
                Console.WriteLine("Inicio Recepcion Mensajes................................");
                Console.WriteLine("=========================================================");

                var verificacionMensajesRecibidos = await administradorMensajes.CheckMessagesReceived(SubscriptionName, TokenID);
                var lstMensajes = administradorMensajes.ReceiveMessages(verificacionMensajesRecibidos, TokenID);
                RespuestaCotizacion respuestaCotizacion;

                foreach (Message mensaje in lstMensajes)
                {
                    var cotizacion = JsonConvert.DeserializeObject<Cotizacion>(Encoding.UTF8.GetString(mensaje.Body));

                    if (context.Productos.Where(m => m.CantidadStock >= cotizacion.Cantidad).Any())
                    {
                        respuestaCotizacion = armarRespuestaCotizacion(cotizacion, true);
                    }
                    else
                    {
                        respuestaCotizacion = armarRespuestaCotizacion(cotizacion, false);
                    }
                    await sendMessages(respuestaCotizacion);
                }

                Console.WriteLine("=========================================================");
                Console.WriteLine("Completed Receiving all messages... Press any key to exit");
                Console.WriteLine("=========================================================");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task sendMessages(RespuestaCotizacion respuestaCotizacion)
        {   
            BusMessage busMessage = new BusMessage(TokenIDSend, TopicSendMessage, SubscriptionSendName, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(respuestaCotizacion)));
            await administradorMensajes.SendResponseMessagesAsync(busMessage);
        }

        public RespuestaCotizacion armarRespuestaCotizacion(Cotizacion cotizacion, bool conExistencias)
        {
            RespuestaCotizacion respuestaCotizacion = new RespuestaCotizacion();
            Productos producto = context.Productos.Where(m => m.Referencia == cotizacion.Referencia).FirstOrDefault();
            respuestaCotizacion.CantidadDisponible = producto.CantidadStock;
            respuestaCotizacion.Descripcion = producto.Descripcion;
            respuestaCotizacion.DisponibilidadInmediata = true;
            respuestaCotizacion.FechaDisponibilidad = DateTime.Now.AddDays(2);
            respuestaCotizacion.FechaRespuesta = DateTime.Now;
            respuestaCotizacion.IdCotizacion = cotizacion.IdCotizacion;
            respuestaCotizacion.Precio = producto.ValorProducto;
            return respuestaCotizacion;
        }

    }
}
