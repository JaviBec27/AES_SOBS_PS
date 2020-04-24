using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AES_SOBS_PS.Models
{
    public class UserInfo : Usuario
    {
        public UserInfo()
        {

        }

        public UserInfo(Usuario usuario)
        {
            this.Activo = usuario.Activo;
            this.Email = usuario.Email;
            this.Identificacion = usuario.Identificacion;
            this.IdRol = usuario.IdRol;
            this.IdUsuario = usuario.IdUsuario;
            this.Nombre = usuario.Nombre;
            this.Password = usuario.Password;
        }
        public string Token { get; set; }
        public string Usuario { get; set; }
        public bool TieneSistemaCotizacion { get; set; }
    }
}
