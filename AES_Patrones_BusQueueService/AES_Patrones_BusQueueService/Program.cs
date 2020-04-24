using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AES_Patrones_BusQueueService
{
    class Program
    {
        static void Main(string[] args)
        {
            var implementing = new ImplemenationMessage();
            implementing.main();
        }
    }
}
