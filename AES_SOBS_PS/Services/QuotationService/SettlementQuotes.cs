using AES_SOBS_PS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AES_SOBS_PS.Services.QuotationService
{
    public class SettlementQuotes
    {
        private readonly AesSobsDbContext _context;
        private readonly IConfiguration _configuration;
        public string EndPointAPIPdfCreator { get; set; }

        public SettlementQuotes(AesSobsDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            EndPointAPIPdfCreator = @"http://localhost:61495/api/PdfCreator";//Endpoint APi
        }

        public SettlementQuotes(IConfiguration configuration)
        {

            _configuration = configuration;
            DbContextOptionsBuilder<AesSobsDbContext> optionsBuilder = new DbContextOptionsBuilder<AesSobsDbContext>();
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("AES_SOBSConnection"));

            using (var db = new AesSobsDbContext())
            {
                var x = db.Usuario.FirstOrDefault();
            }


        }

        /// <summary>
        /// Liquida una cotizacion, hace el llamado para generar el PDF
        /// </summary>
        public async Task Settlement(string referencia)
        {
            if (!VerifyCompleted(referencia))
                return;

            var cotizaciones = await Task.Run(() => _context.Cotizacion.Include(x => x.RespuestaCotizacion).Where(x => x.Referencia == referencia));
            List<RespuestaCotizacion> respuestas = new List<RespuestaCotizacion>();
            foreach (var cotizacion in cotizaciones)
            {
                foreach (var respuesta in cotizacion.RespuestaCotizacion)
                {
                    respuesta.Precio = respuesta.Precio * 1.10;//Liquidamos con el 10% adicional
                    respuestas.Add(respuesta);
                }
            }

            try
            {
                if (respuestas == null || !respuestas.Any())
                    throw new Exception($"No hay respuestas para la cotizción {referencia}");

                var urlAPI = EndPointAPIPdfCreator;
                PdfService.PdfManagementService pdfCreator =
                    new PdfService.PdfManagementService(_context, urlAPI);

                var result = await pdfCreator.CratePDF(respuestas);

                if (!System.IO.File.Exists(result))
                    throw new Exception("no hay listas");
                var user = await _context.Usuario.FindAsync(cotizaciones.FirstOrDefault().IdUsuario);
                var atts = new string[] { result };
                var sender = new MailServices.MessageServiceMail(_configuration);
                await sender.SendEmailAsync(user.Nombre, user.Email, "Token", GetBodyToken(user, referencia), atts).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetBodyToken(Usuario user, string referencia)
        {
            var paramsMessage = new string[] { user.Nombre, referencia };
            return MailServices.TemplateHtml.GetTemplateForRepplyQuote(paramsMessage);
        }


        /// <summary>
        /// notifica un listado de respuestas de cotización realizadas
        /// </summary>
        /// <param name="respuestas"></param>
        public async Task NotifyEnding(List<RespuestaCotizacion> respuestas)
        {
            if (!respuestas.Any())
                return;

            foreach (var respuesta in respuestas)
            {
                //Obtenemos la cotizacion refente a la respuesta
                var cotizacion = await _context.Cotizacion.FindAsync(respuesta.IdCotizacion).ConfigureAwait(false);

                cotizacion.Procesada = true;//Establecemos que ya ha sido procesada.
                _context.Entry(cotizacion).State = EntityState.Modified;

                try
                {
                    _context.SaveChangesAsync().GetAwaiter().GetResult();
                    //verifica si ya se han completado las respuestas a las solicicitu
                    if (VerifyCompleted(cotizacion.Referencia))
                        Settlement(cotizacion.Referencia).GetAwaiter().GetResult();
                }
                catch (DbUpdateConcurrencyException dbex)
                {
                    throw dbex;
                }
            }


        }

        /// <summary>
        /// Verifica si ya está completa
        /// </summary>
        /// <param name="referenciaCotizacion"></param>
        /// <returns></returns>
        private bool VerifyCompleted(string referenciaCotizacion)
        {
            var cotizaciones = _context.Cotizacion.Where(x => x.Referencia == referenciaCotizacion).ToList();
            var cantidadProcesadas = cotizaciones.Where(x => x.Procesada == true).ToList();

            //Si son iguales ya ha terminado y debería generar el PDF
            return (cotizaciones.Count() == cantidadProcesadas.Count());
        }
    }
}
