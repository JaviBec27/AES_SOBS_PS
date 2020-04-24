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

namespace AES_SOBS_PS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoriesController : ControllerBase
    {
        private readonly AesSobsDbContext _context;

        public CategoriesController(AesSobsDbContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public IEnumerable<Categoria> GetCategoria()
        {
            return _context.Categoria;
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoria([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var categoria = await _context.Categoria.Include(x => x.Producto).FirstOrDefaultAsync(x => x.IdCategoria == id);

            if (categoria == null)
            {
                return NotFound();
            }

            return Ok(categoria);
        }

        // PUT: api/Categories/2
        /// <summary>
        /// Permite obtener una categoría consultando por el id de usuario y la categoría
        /// </summary>
        /// <param name="id">identifica la categoría</param>
        /// <param name="CategoriaProductoProveedor">iduser: identificación de usuario del proveedor, idcategoria: identificacion de la categoría</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> GetCategoriaByProveedor([FromBody] CategoriaProductoProveedor categoria)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productos = _context.Producto.Where(x => x.IdUsuario == categoria.IdUser && x.IdCategoria == categoria.IdCategoria).ToList();

            if (productos == null || !productos.Any())
                return Ok(new List<Categoria>());

            var cat = await _context.Categoria.FindAsync(categoria.IdCategoria);
            cat.Producto = productos;
            return Ok(cat);
        }

    }
}