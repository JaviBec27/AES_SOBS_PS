using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Message_Daemon.Models
{
    public partial class Cotizacion
    {
        public Cotizacion()
        {
            RespuestaCotizacion = new HashSet<RespuestaCotizacion>();
        }

        public int IdCotizacion { get; set; }
        public DateTime FechaCotizacion { get; set; }
        public int IdUsuario { get; set; }
        public int IdProducto { get; set; }
        public string Referencia { get; set; }
        public int Cantidad { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public Producto IdProductoNavigation { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Usuario IdUsuarioNavigation { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<RespuestaCotizacion> RespuestaCotizacion { get; set; }
    }
}
