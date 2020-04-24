using System;
using System.Collections.Generic;

namespace Message_Daemon.Models
{
    public partial class Rol
    {
        public Rol()
        {
            Usuario = new HashSet<Usuario>();
            Vista = new HashSet<Vista>();
        }

        public int IdRol { get; set; }
        public string NombreRol { get; set; }
        public string Descripcion { get; set; }

        public ICollection<Usuario> Usuario { get; set; }
        public ICollection<Vista> Vista { get; set; }
    }
}
