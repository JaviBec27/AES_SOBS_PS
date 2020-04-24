using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AES_SOBS_PS.Services.PdfService.ModelsPdfService
{
    public interface IInfoGeneralCotizacion
    {
        string Referencia { get; set; }
        string FechaCotizacion { get; set; }
        string Logo { get; set; }
    }

    public class InfoGeneralCotizacion : IInfoGeneralCotizacion
    {
        public string Referencia { get; set; }
        public string FechaCotizacion { get; set; }
        public string Logo { get; set; }
    }
}
