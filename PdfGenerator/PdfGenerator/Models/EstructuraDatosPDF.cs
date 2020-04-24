using System.Collections.Generic;

namespace PdfGenerator.Models
{
    public interface IEstructuraDatosPDF
    {
        Cliente Cliente { get; set; }
        InfoGeneralCotizacion InfoGeneralCotizacion { get; set; }
        IEnumerable<RespuestaCotizacion> Cotizaciones { get; set; }
    }

    public class EstructuraDatosPDF : IEstructuraDatosPDF
    {
        public Cliente Cliente { get; set; }
        public InfoGeneralCotizacion InfoGeneralCotizacion { get; set; }
        public IEnumerable<RespuestaCotizacion> Cotizaciones { get; set; }
    }
}
