using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AES_SOBS_PS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AES_SOBS_PS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RolsController : ControllerBase
    {
        private readonly AesSobsDbContext _context;

        public RolsController(AesSobsDbContext context)
        {
            _context = context;
        }

        // GET: api/Rols
        [HttpGet]
        public IEnumerable<Rol> GetRol()
        {
            var a = Newtonsoft.Json.JsonConvert.SerializeObject(_context.Rol);
            //var b = Newtonsoft.Json.JsonConvert.SerializeObject(_context.Rol.Include(x=>x.Vista).ToList());
            
            return _context.Rol.Include(x=>x.Vista);
        }

        // GET: api/Rols/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRol([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rol = await _context.Rol.FindAsync(id);

            if (rol == null)
            {
                return NotFound();
            }

            return Ok(rol);
        }

        // PUT: api/Rols/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRol([FromRoute] int id, [FromBody] Rol rol)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rol.IdRol)
            {
                return BadRequest();
            }

            _context.Entry(rol).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolExists(id))
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

        // POST: api/Rols
        [HttpPost]
        public async Task<IActionResult> PostRol([FromBody] Rol rol)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Rol.Add(rol);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRol", new { id = rol.IdRol }, rol);
        }

        // DELETE: api/Rols/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rol = await _context.Rol.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }

            _context.Rol.Remove(rol);
            await _context.SaveChangesAsync();

            return Ok(rol);
        }

        private bool RolExists(int id)
        {
            return _context.Rol.Any(e => e.IdRol == id);
        }
    }
}