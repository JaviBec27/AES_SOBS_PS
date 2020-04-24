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
using Microsoft.Extensions.Configuration;
using System.Text;
using AES_SOBS_PS.Services.MailServices;

namespace AES_SOBS_PS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly AesSobsDbContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(AesSobsDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<Usuario> GetUsuario()
        {
            return _context.Usuario;
        }

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> AddUsuario([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            usuario.NombreSuscripcion = usuario.Nombre.ToLower()
                .Replace(" ", "_")
                .Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ú", "u")
                .Replace("ñ", "n");

            usuario.Activo = false;
            _context.Usuario.Add(usuario);

            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);


                var user = new UserInfo(usuario);
                Services.SecurityServices.IUserService userService =
                       new Services.SecurityServices.UserService(_configuration, _context);

                if (await userService.AutenticateAndGenerateNewTokenAsync(user).ConfigureAwait(false))
                {
                    usuario.TokenMedio = user.Token;
                    _context.TokenUser.Add(new TokenUser() { IdUser = user.IdUsuario, Email = user.Email, Token = user.Token, SignToken = user.Token.Split('.')[2] });
                    await _context.SaveChangesAsync().ConfigureAwait(false);

                    Services.MailServices.IMessagingServiceMail messaging = new Services.MailServices.MessageServiceMail(_configuration);
                    await messaging.SendEmailAsync(user.Nombre, user.Email, "Token", GetBodyToken(user), null).ConfigureAwait(false);
                }
                await UpdateUsuario(usuario.IdUsuario, usuario);
            }
            catch (ArgumentException aex)
            {
                ModelState.AddModelError("", aex.Message);
                return BadRequest(ModelState);
            }
            catch (DbUpdateException)
            {
                if (EmailExists(usuario.Email))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return CreatedAtAction("GetUsuario", new { id = usuario.IdUsuario }, usuario);
        }

        // GET: api/Clientes/5
        [HttpGet("{token}")]
        [AllowAnonymous]
        public ActionResult ValidateToken([FromRoute] string token)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_context.TokenUser.Any())
            {
                ModelState.AddModelError("321", "No se ha registrado Token");
                return BadRequest(ModelState);
            }

            var userToken = _context.TokenUser.LastOrDefault(x => x.SignToken == token && x.Activo == true);

            if (userToken == null)
            {
                ModelState.AddModelError("321", "El Token está Inactivo");
                return BadRequest(ModelState);
            }

            var client = _context.Usuario.FirstOrDefault(x => x.Email == userToken.Email);

            if (client == null)
            {
                ModelState.AddModelError("321", "Usuario Incorrecto!");
                return BadRequest(ModelState);
            }

            client.Activo = true;
            userToken.Activo = false;

            try
            {
                _context.SaveChanges();
                //TODO: revisar el redireccionamiento al Login
                if (client.IdRol == 1)//Si es proveedor envía correo de conexión
                    SendConexionEmail(client, userToken);
                return Redirect(_configuration["UrlApi"]);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SendConexionEmail(Usuario user, TokenUser userToken)
        {
            //Para recepción -ellos reciben
            //tokenID = (Token generado en el aplicativo)

            //subscriptionName = (Nombre de registro normalizado)
            //tokenAPI = (token full)
            var datos = new string[7];

            //Datos para envío
            datos[0] = userToken.SignToken;
            datos[1] = userToken.Token;
            datos[2] = user.NombreSuscripcion;

            //Datos para Respuesta
            datos[3] = "H-jsdfbKLiRvc_vXshLQpC1QnUQlpwleDfFNU5wjnyMhu4";
            datos[4] = "respuesta_proveedores";
            datos[5] = "{'topicName':'',subscriptionName':'',messageBody':'',tokenID':''}";

            datos[6] = user.Nombre;

            var body = TemplateHtml.GetTemplateForNewSupply(datos);

            MessageServiceMail serviceMail =
                new MessageServiceMail(_configuration);

            serviceMail.SendEmailAsync(user.Nombre, user.Email, "Parámetros de Configuración", body, null).GetAwaiter().GetResult();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok(usuario);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario([FromRoute] int id, [FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuario.IdUsuario)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.IdUsuario == id);
        }

        private bool EmailExists(string email)
        {
            return _context.Usuario.Any(e => e.Email == email);
        }

        private string GetBodyToken(UserInfo user)
        {
            var url = $"{_configuration["UrlApi"]}/api/Users";
            var body = new StringBuilder();

            body.Append($"<a href=\"{url}/{user.Token.Split('.')[2]}\">");
            body.Append("CONFIRMA TU EMAIL CON UN CLICK</a>");
            body.Append("<br/><p><b>EQUIPO AES SOBS</b></p>");
            return body.ToString();
        }
    }
}