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
using AES_Patrones_BusQueueService.Entities;
using Newtonsoft.Json;
using System.Text;
using AES_Patrones_BusQueueService;
using Microsoft.Extensions.Configuration;

namespace AES_SOBS_PS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class QuotationsController : ControllerBase
    {
        private readonly AesSobsDbContext _context;
        private readonly IConfiguration _configuration;

        public QuotationsController(AesSobsDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Cotizacions
        [HttpGet]
        public IEnumerable<Cotizacion> GetCotizacion()
        {
            return _context.Cotizacion;
        }

        // GET: api/Quotations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCotizacion([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cotizacion = await _context.Cotizacion.FindAsync(id);

            if (cotizacion == null)
            {
                return NotFound();
            }

            return Ok(cotizacion);
        }

        // PUT: api/Cotizacions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCotizacion([FromRoute] int id, [FromBody] Cotizacion cotizacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cotizacion.IdCotizacion)
            {
                return BadRequest();
            }

            _context.Entry(cotizacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CotizacionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cotizacions
        [HttpPost]
        public async Task<IActionResult> AddCotizacion([FromBody] List<Cotizacion> cotizacion)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            List<Cotizacion> lstCotizacion = new List<Cotizacion>();

            var _referencia = $"SOBS{5030}{DateTime.Now.ToString("ddMMyyyyhhmmss")}";
            cotizacion.ForEach((m) => { m.Referencia = _referencia; });

            try
            {
                _context.Cotizacion.AddRange(cotizacion);
                await _context.SaveChangesAsync();

                List<BusMessage> lstBusMessage = new List<BusMessage>();


                foreach (Cotizacion _quotation in cotizacion)
                {

                    _quotation.FechaCotizacion = DateTime.Now;
                    var _productInfo = (Producto)_context.Producto.FirstOrDefault(m => m.IdProducto == _quotation.IdProducto);
                    var _tokeUser = (TokenUser)_context.TokenUser.Where(m => m.IdUser == _productInfo.IdUsuario).FirstOrDefault();
                    var _userInfo = (Usuario)_context.Usuario.Where(m => m.IdUsuario == _productInfo.IdUsuario).FirstOrDefault();
                    var _categoriaInfo = (Categoria)_context.Categoria.Where(m => m.IdCategoria == _productInfo.IdCategoria).FirstOrDefault();

                    if (_userInfo.SistemaDeCotizacion && _userInfo.Activo)
                    {
                        _quotation.Referencia = _productInfo.ReferenciaProductoProveedor;
                        lstBusMessage.Add(new BusMessage(_tokeUser.SignToken, _categoriaInfo.NombreServicio, _userInfo.NombreSuscripcion, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_quotation))));
                    }

                    if (!_userInfo.SistemaDeCotizacion)
                        lstCotizacion.Add(_quotation);
                }

                cotizacion.ForEach((m) => { m.Referencia = _referencia; });
                new ManageMessage().SendMessages(lstBusMessage);

            }
            catch (Exception ex)
            {
                return StatusCode(501, ex);
            }

            await ReplyAutomaticsQuotes(lstCotizacion);
            return Ok(cotizacion);
        }


        /// <summary>
        /// Toma un listado de cotizaciones, crea la respuesta de manera automática y notifica que ya fueron procesadas
        /// </summary>
        /// <param name="cotizacionesAutomaticas">listado de cotizaciones que se responden de manera automática con info de base de datos</param>
        private async Task ReplyAutomaticsQuotes(List<Cotizacion> cotizacionesAutomaticas)
        {
            if (!ModelState.IsValid)
            {
                return;
            }
            if (!cotizacionesAutomaticas.Any())
                return;

            var respuestasCotizacion =
                new List<RespuestaCotizacion>();

            //Generamos la respuesta por cada cotización
            foreach (var cotizacion in cotizacionesAutomaticas)
            {
                var producto = _context.Producto.FirstOrDefault(x => x.IdProducto == cotizacion.IdProducto);
                var cantidadDisponible = cotizacion.Cantidad > producto.Cantidad ? cotizacion.Cantidad : producto.Cantidad;
                Nullable<DateTime> fechaDisponible = null;
                if (cantidadDisponible > 0)
                    fechaDisponible = DateTime.Now.AddHours(3);

                respuestasCotizacion.Add(new RespuestaCotizacion
                {
                    CantidadDisponible = cantidadDisponible,
                    Descripcion = producto.Descripcion,
                    DisponibilidadInmediata = producto.Cantidad > 0 ? true : false,
                    FechaDisponibilidad = fechaDisponible,
                    FechaRespuesta = DateTime.Now,
                    Precio = cantidadDisponible * producto.Precio,
                    IdCotizacion = cotizacion.IdCotizacion
                });
            }

            if (respuestasCotizacion.Any())
            {
                await _context.RespuestaCotizacion.AddRangeAsync(respuestasCotizacion).ConfigureAwait(false);
                _context.SaveChangesAsync().GetAwaiter().GetResult();
            }

            var settlementQuotes =
                new Services.QuotationService.SettlementQuotes(_context, _configuration);
            settlementQuotes.NotifyEnding(respuestasCotizacion).GetAwaiter().GetResult();

        }

        // DELETE: api/Cotizacions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCotizacion([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cotizacion = await _context.Cotizacion.FindAsync(id);
            if (cotizacion == null)
            {
                return NotFound();
            }

            _context.Cotizacion.Remove(cotizacion);
            await _context.SaveChangesAsync();

            return Ok(cotizacion);
        }

        private bool CotizacionExists(int id)
        {
            return _context.Cotizacion.Any(e => e.IdCotizacion == id);
        }
    }
}