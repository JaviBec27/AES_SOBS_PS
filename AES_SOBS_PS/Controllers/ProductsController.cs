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

namespace AES_SOBS_PS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductsController : ControllerBase
    {
        private readonly AesSobsDbContext _context;

        public ProductsController(AesSobsDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public IEnumerable<Producto> GetProducto()
        {
            return _context.Producto;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProducto([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var producto = await _context.Producto.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            return Ok(producto);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto([FromRoute] int id, [FromBody] Producto producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != producto.IdProducto)
            {
                return BadRequest();
            }

            producto.FechaModificacion = DateTime.Now;
            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
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

        // POST: api/Products
        [HttpPost]
        public async Task<IActionResult> AddProducto([FromBody] Producto producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            producto.FechaCreacion = DateTime.Now;
            var _user = _context.Usuario.Where(m => m.IdUsuario == producto.IdUsuario).FirstOrDefault();
            var _category = _context.Categoria.Where(m => m.IdCategoria == producto.IdCategoria).FirstOrDefault();
            var _existProduct = _context.Producto.Where(m => m.IdUsuario == producto.IdUsuario && m.IdCategoria == producto.IdCategoria).Any();

            try
            {
                _context.Producto.Add(producto);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (!_existProduct)
            {
                try
                {

                    ManageAzurePortal manageAzurePortal = new ManageAzurePortal();
                    await manageAzurePortal.CreateSubscription(_user.NombreSuscripcion, _category.NombreServicio);
                    await manageAzurePortal.CreateRule("SessionID", _user.NombreSuscripcion);
                }
                catch
                {
                    throw new Exception("Error al crear la suscripcion de Azure");
                }
            }

            return CreatedAtAction("GetProducto", new { id = producto.IdProducto }, producto);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var producto = await _context.Producto.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            try
            {
                _context.Producto.Remove(producto);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("321",ex.Message);
                return BadRequest(ModelState);
            }


            return Ok(producto);
        }

        private bool ProductoExists(int id)
        {
            return _context.Producto.Any(e => e.IdProducto == id);
        }
    }
}