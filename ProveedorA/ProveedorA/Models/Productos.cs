using System;
using System.Collections.Generic;

namespace ProveedorA.Models
{
    public partial class Productos
    {
        public string Referencia { get; set; }
        public string NombreDescripcion { get; set; }
        public int? ValorProducto { get; set; }
        public int? CantidadStock { get; set; }
        public string Descripcion { get; set; }
    }
}
