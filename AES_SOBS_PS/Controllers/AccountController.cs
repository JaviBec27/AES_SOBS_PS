
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AES_SOBS_PS.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace AES_SOBS_PS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ControllerBase
    {
        private readonly AesSobsDbContext _context;
        private readonly IConfiguration _configuration;

        public AccountController(AesSobsDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserInfo user)
        {
            //Response.Headers.Add("access-control-allow-origin", "*");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = new Usuario()
            {
                Password = user.Password,
                Email = user.Email,
                Identificacion = user.Identificacion,
                Nombre = user.Nombre,
                Activo = user.Activo,
                IdRol = user.IdRol,
                Contacto = user.Contacto,
                SistemaDeCotizacion = user.TieneSistemaCotizacion,
                NombreSuscripcion = user.NombreSuscripcion
            };

            UsersController usersCtrl = new UsersController(_context, _configuration);
            try
            {
                return await usersCtrl.AddUsuario(usuario).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserInfo user)
        {
            //Response.Headers.Add("access-control-allow-origin", "*");
            Services.SecurityServices.UserService userService =
                    new Services.SecurityServices.UserService(_configuration, _context);
            try
            {
                var result = userService.AutenticateLogin(user.Email, user.Password);
                if (result)
                {
                    var usuarioActivo = await _context.Usuario.FirstOrDefaultAsync(x => x.Email == user.Email);
                    var rolActivo = await _context.Rol.Include(x=>x.Vista).Include(x=>x.Usuario).FirstOrDefaultAsync(x => x.IdRol == usuarioActivo.IdRol);
                    rolActivo.Usuario = new List<Usuario>() { usuarioActivo };            
                    
                    return Ok(rolActivo);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuario o Contraseña Inválido");
                    return BadRequest(ModelState);
                }
            }
            catch (ArgumentException aeg)
            {
                ModelState.AddModelError(string.Empty, aeg.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}