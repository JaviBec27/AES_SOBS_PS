using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PdfGenerator.Models
{
    public interface IRespuestaCotizacion
    {
        int Id { get; set; }
        string Proveedor { get; set; }
        string Item { get; set; }
        int CantidadDisponible { get; set; }
        float Precio { get; set; }
        float PrecioUnitario { get; set; }
        bool Disponibilidad { get; set; }
        string FechaDisponibildiad { get; set; }
        string Descripcion { get; set; }
    }

    public class RespuestaCotizacion : IRespuestaCotizacion
    {
        public int Id { get; set; }
        public string Proveedor { get; set; }
        public string Item { get; set; }
        public int CantidadDisponible { get; set; }
        public float Precio { get; set; }
        public float PrecioUnitario { get; set; }
        public bool Disponibilidad { get; set; }
        public string FechaDisponibildiad { get; set; }
        public string Descripcion { get; set; }
    }
}
