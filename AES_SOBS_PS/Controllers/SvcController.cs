using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AES_SOBS_PS.Models;
using AES_SOBS_PS.Services.HTTPServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AES_SOBS_PS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SvcController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly AesSobsDbContext context;

        public SvcController(IConfiguration configuration, AesSobsDbContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }

        // GET: api/Svc
        [HttpGet("{info}")]
        public IActionResult GetAPIAutorizationToken([FromRoute] string info)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var encriptEmail = Services.SecurityServices.CriptoSecurity.Encrypt(info);
            return Ok(encriptEmail);
        }

        [Route("Mail")]
        [HttpGet]
        public async Task<IActionResult> SendMail()
        {
            try
            {

                var urlAPI = @"http://localhost:61495/api/PdfCreator";
                Services.PdfService.PdfManagementService pdfCreator = new Services.PdfService.PdfManagementService(context, urlAPI);

                var listRespuestas = context.RespuestaCotizacion.ToList();
                if (listRespuestas == null || !listRespuestas.Any())
                    throw new Exception("no hay listas");

                var result = await pdfCreator.CratePDF(listRespuestas);

                if (!System.IO.File.Exists(result))
                    throw new Exception("no hay listas");

                var atts = new string[] { result };

                var sender = new Services.MailServices.MessageServiceMail(configuration);

                await sender.SendEmailAsync("Javier", "javierbecerra@javeriana.edu.co", "Prueba", "Por fin!", atts);
                return Ok("Mensaje Enviado");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return BadRequest(ModelState);
            }
        }

    }
}