using System;
using System.Collections.Generic;

namespace AES_SOBS_PS.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Cotizacion = new HashSet<Cotizacion>();
            Producto = new HashSet<Producto>();
        }

        public int IdUsuario { get; set; }
        public string Email { get; set; }
        public string Identificacion { get; set; }
        public string Password { get; set; }
        public int IdRol { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public bool SistemaDeCotizacion { get; set; }
        public string TokenMedio { get; set; }
        public string Contacto { get; set; }
        public string NombreSuscripcion { get; set; }

        public Rol IdRolNavigation { get; set; }
        public ICollection<Cotizacion> Cotizacion { get; set; }
        public ICollection<Producto> Producto { get; set; }
    }
}
