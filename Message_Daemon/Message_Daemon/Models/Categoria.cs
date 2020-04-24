using System;
using System.Collections.Generic;

namespace Message_Daemon.Models
{
    public partial class Categoria
    {
        public Categoria()
        {
            Producto = new HashSet<Producto>();
        }

        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string NombreServicio { get; set; }

        public ICollection<Producto> Producto { get; set; }
    }
}
