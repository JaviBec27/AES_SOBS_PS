using System.Collections.Generic;

namespace AES_SOBS_PS.Services.PdfService.ModelsPdfService
{
    public interface IEstructuraDatosPDF
    {
        ClientePDF Cliente { get; set; }
        InfoGeneralCotizacion InfoGeneralCotizacion { get; set; }
        IEnumerable<RespuestaCotizacionPDF> Cotizaciones { get; set; }
    }

    public class EstructuraDatosPDF : IEstructuraDatosPDF
    {
        public ClientePDF Cliente { get; set; }
        public InfoGeneralCotizacion InfoGeneralCotizacion { get; set; }
        public IEnumerable<RespuestaCotizacionPDF> Cotizaciones { get; set; }
    }
}
