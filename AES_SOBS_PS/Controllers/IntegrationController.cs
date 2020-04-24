using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AES_SOBS_PS.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using AES_Patrones_BusQueueService;
using AES_Patrones_BusQueueService.Entities;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using AES_SOBS_PS.Services.QuotationService;

namespace AES_SOBS_PS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ManageMessageController : ControllerBase
    {
        private readonly AesSobsDbContext _context;
        private readonly IConfiguration _configuration;

        public ManageMessageController(AesSobsDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("[action]/Read")]
        public async Task<List<Cotizacion>> ReadQuotation([FromBody] AppOptions appOptions)
        {
            var administradorMensajes = new ManageMessage();
            var _lstRespuestaCotizacion = new List<Cotizacion>();
            List<Message> _lstMessage = new List<Message>();
            var verificacionMensajesRecibidos = await administradorMensajes.CheckMessagesReceived(appOptions.SubscriptionName, appOptions.TokenID);

            if (verificacionMensajesRecibidos.Count <= 0)
                return _lstRespuestaCotizacion;

            _lstMessage = administradorMensajes.ReceiveMessages(verificacionMensajesRecibidos, appOptions.TokenID);

            foreach (Message message in _lstMessage)
            {
                var respuestaCotizacion = JsonConvert.DeserializeObject<Cotizacion>(Encoding.UTF8.GetString(message.Body));
                _lstRespuestaCotizacion.Add(respuestaCotizacion);
            }

            return _lstRespuestaCotizacion;
        }

        [HttpPost]
        [Route("[action]/Send")]
        public async Task<IActionResult> SendResponseQuotation([FromBody] BusMessage busMessage)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (busMessage.MessageBody.Length == 0)
                return StatusCode(501, "Debe enviar el arreglo de respuesta");

            var lstRespuestaCotizacion = JsonConvert.DeserializeObject<RespuestaCotizacion>(Encoding.UTF8.GetString(busMessage.MessageBody));
            await new ManageMessage().SendResponseMessagesAsync(busMessage);
            return Ok(busMessage);
        }

        [HttpPost]
        [Route("[action]/Reply")]

        public async Task<IActionResult> NotifyReplyQuote([FromBody]List<RespuestaCotizacion> respuestas)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            SettlementQuotes settlement =
                new SettlementQuotes(_context, _configuration);

            try
            {
                await settlement.NotifyEnding(respuestas).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("321", ex.Message);
                return BadRequest(ModelState);
            }
        }

    }
}