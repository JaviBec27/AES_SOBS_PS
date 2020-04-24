using AES_SOBS_PS.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AES_SOBS_PS.Services.SecurityServices
{
    public class UserService : IUserService
    {
        private enum AutenticationMode { Registro, Login }
        private readonly IConfiguration _configuration;
        private readonly AesSobsDbContext _context;

        public UserService(IConfiguration configuration, AesSobsDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<bool> AutenticateAndGenerateNewTokenAsync(UserInfo user)
        {
            try
            {
                var validate = await Task.Run(() => Validate(user.Email, user.Password, AutenticationMode.Registro)).ConfigureAwait(false);

                if (validate)
                {
                    BuildToken(user);
                    return true;
                }
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

            return false;
        }

        /// <summary>
        ///  Valida las credenciales para autenticación en Login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> AutenticateLoginAsync(string email, string password)
        {
            try
            {
                var validate = await Task.Run(() => Validate(email, password, AutenticationMode.Login)).ConfigureAwait(false);
                return (validate);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Genera un token despues de realizar la autenticación en Registro
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="idRol"></param>
        /// <returns></returns>
        public async Task<string> AutenticateAndGenerateNewTokenAsync(string user, string password)
        {
            try
            {
                var validate = await Task.Run(() => Validate(user, password, AutenticationMode.Registro)).ConfigureAwait(false);

                if (validate)
                {
                    var userInfo = new UserInfo() { Email = user, Password = password };
                    BuildToken(userInfo);
                    return userInfo.Token;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return "";
        }

        /// <summary>
        /// Realiza la validación para autenticación con usuario y contraseña
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="autenticationMode"></param>
        /// <returns></returns>
        private bool Validate(string email, string password, AutenticationMode autenticationMode)
        {

            var prov = _context.Usuario.FirstOrDefault(x => x.Email == email && x.Password == password);

            try
            {
                if (prov == null)
                    throw new ArgumentException("Usuario o Contraseña Inválido");

                switch (autenticationMode)
                {
                    case AutenticationMode.Registro:
                        if (prov.Activo)
                            throw new ArgumentException("El Usuario ya está activo");
                        break;
                    case AutenticationMode.Login:
                        if (!prov.Activo)
                            throw new ArgumentException("El Usuario no está Activo");
                        break;
                    default:
                        return false;
                }

                return true;
            }
            catch (ArgumentException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Ejecuta la construcción de un Token
        /// </summary>
        /// <param name="userInfo"></param>
        private void BuildToken(UserInfo userInfo)
        {
            TokenService tokenService = new TokenService(_configuration, 365);
            tokenService.BuildToken(userInfo);
        }

        public bool AutenticateLogin(string email, string password)
        {
            try
            {
                var validate = Validate(email, password, AutenticationMode.Login);
                return (validate);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
