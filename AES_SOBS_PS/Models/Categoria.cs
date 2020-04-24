using System;
using System.Collections.Generic;

namespace AES_SOBS_PS.Models
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
