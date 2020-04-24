using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Message_Daemon.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;

namespace Message_Daemon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly MessageReceptionProvider _messageReceptionProvider;

        public ValuesController(MessageReceptionProvider _messageReceptionProvider)
        {
            this._messageReceptionProvider = _messageReceptionProvider;
        }

        //[HttpGet()]
        //public string Get()
        //{
        //    return _messageReceptionProvider.LstMessage;
        //}

        [HttpGet()]
        [Route("[action]")]
        public List<Message> GetMessage()
        {
            return _messageReceptionProvider.LstMessage;
        }

    }
}
