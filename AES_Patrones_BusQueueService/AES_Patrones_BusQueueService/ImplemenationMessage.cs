using AES_Patrones_BusQueueService.Entities;
using System;
using System.Collections.Generic;

namespace AES_Patrones_BusQueueService
{
    public class ImplemenationMessage
    {
        public void main()
        {
            //categoria_hogar
            //var data = new List<BusMessage>()
            //{
            //    //new BusMessage("Token123456#","categoria_electrodomesticos", "Javier", new Cotizacion("Licuadoras", 3)),
            //    //new BusMessage("Token123456#","categoria_electrodomesticos", "Javier", new Cotizacion("Computadores", 4)),
            //    //new BusMessage("Token123456#","categoria_electrodomesticos", "Javier", new Cotizacion("Equipo de sonido", 8)),
            //    //new BusMessage("Token123456#","categoria_hogar", "Javier", new Cotizacion("Vajilla", 1)),
            //    //new BusMessage("Token123456#","categoria_hogar", "Javier", new Cotizacion("Portaretratos", 4)),
            //    //new BusMessage("Token123456#","categoria_hogar", "Javier", new Cotizacion("Mesas", 6)),
            //    //new BusMessage("Token123456#","categoria_hogar", "Javier", new Cotizacion("Olla Expres", 2)),
            //    //new BusMessage("Token123456#","categoria_hogar", "Javier", new Cotizacion("Cuadernos", 444)),
            //    //new BusMessage("Token123456#","categoria_hogar", "Javier", new Cotizacion("Estufa", 8)),
            //    //new BusMessage("Token123456#","categoria_electrodomesticos", "Javier", new Cotizacion("Microondas", 53))
            //};

            //Console.WriteLine(DateTime.Now.ToString());
            var administradorMensajes = new ManageMessage();
            //administradorMensajes.SendMessagesAsync(data);            
            var verificacionMensajesRecibidos = administradorMensajes.CheckMessagesReceived("Javier","").GetAwaiter().GetResult();
            var lstMensajes = administradorMensajes.ReceiveMessages(verificacionMensajesRecibidos,"");
            Console.WriteLine(DateTime.Now.ToString());

            //Administración del portal Azure
            var administradorPortalAzure = new ManageAzurePortal();
            //administradorPortalAzure.CreateSubscription("Test_New").GetAwaiter().GetResult(); //ConfigureAwait(false);
            //administradorPortalAzure.CreateRule("categories", "Test_New").GetAwaiter().GetResult(); //ConfigureAwait(false);
            var mirar = administradorPortalAzure.CheckTopics("Javier").GetAwaiter().GetResult();

            Console.WriteLine("=========================================================");
            Console.WriteLine("Completed Receiving all messages... Press any key to exit");
            Console.WriteLine("=========================================================");

            Console.ReadKey();
        }

    }


}
