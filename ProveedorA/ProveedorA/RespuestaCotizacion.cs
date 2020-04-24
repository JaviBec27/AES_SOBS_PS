using System;
using System.Collections.Generic;
using System.Text;

namespace ProveedorA
{
    public partial class RespuestaCotizacion
    {
        public DateTime? FechaRespuesta { get; set; }
        public bool? DisponibilidadInmediata { get; set; }
        public DateTime? FechaDisponibilidad { get; set; }
        public string Descripcion { get; set; }
        public int? CantidadDisponible { get; set; }
        public int IdCotizacion { get; set; }
        public double? Precio { get; set; }
    }
}
