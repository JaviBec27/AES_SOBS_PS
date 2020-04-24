using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Message_Daemon.Models
{
    public partial class RespuestaCotizacion
    {
        public int IdRespuestaCotizacion { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        public bool? DisponibilidadInmediata { get; set; }
        public DateTime? FechaDisponibilidad { get; set; }
        public string Descripcion { get; set; }
        public int? CantidadDisponible { get; set; }
        public int IdCotizacion { get; set; }
        public double? Precio { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public Cotizacion IdCotizacionNavigation { get; set; }
    }
}
