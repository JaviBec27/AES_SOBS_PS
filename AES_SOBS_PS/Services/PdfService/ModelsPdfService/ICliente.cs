using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AES_SOBS_PS.Services.PdfService.ModelsPdfService
{
    public interface ICliente
    {
        string Identificacion { get; set; }
        string Nombre { get; set; }
        string Email { get; set; }
        string Telefono { get; set; }
    }

    public class ClientePDF : ICliente
    {
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
    }
}
