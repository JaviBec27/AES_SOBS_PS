using System;
using System.Collections.Generic;

namespace Message_Daemon.Models
{
    public partial class Producto
    {
        public Producto()
        {
            Cotizacion = new HashSet<Cotizacion>();
        }

        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public double? Precio { get; set; }
        public string Imagen { get; set; }
        public int? Cantidad { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? IdCategoria { get; set; }
        public int? IdUsuario { get; set; }
        public bool? TipoProducto { get; set; }
        public string Descripcion { get; set; }
        public string ReferenciaProductoProveedor { get; set; }

        public Categoria IdCategoriaNavigation { get; set; }
        public Usuario IdUsuarioNavigation { get; set; }
        public ICollection<Cotizacion> Cotizacion { get; set; }
    }
}
