using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AES_SOBS_PS.Models;

namespace AES_SOBS_PS.Services.SecurityServices
{
    public interface IUserService
    {
        /// <summary>
        /// Verifica la autenticación y genera un JWToken
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> AutenticateAndGenerateNewTokenAsync(UserInfo user);

        /// <summary>
        /// Autentica y genera un JWToken
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="idRol"></param>
        /// <returns></returns>
        Task<string> AutenticateAndGenerateNewTokenAsync(string user, string password);

        /// <summary>
        /// Autenticación para Login con user=mail, password y rol
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="idRol">número de rol</param>
        /// <returns></returns>
        Task<bool> AutenticateLoginAsync(string email, string password);
        
        bool AutenticateLogin(string email, string password);

    }
}
