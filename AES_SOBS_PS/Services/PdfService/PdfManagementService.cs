using AES_SOBS_PS.Models;
using AES_SOBS_PS.Services.PdfService.ModelsPdfService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AES_SOBS_PS.Services.PdfService
{
    public class PdfManagementService
    {
        private readonly AesSobsDbContext _context;
        private readonly string _endPoint;

        public PdfManagementService(AesSobsDbContext context, string endPoint)
        {
            _context = context;
            _endPoint = endPoint;
        }


        /// <summary>
        /// Crea un PDF y devuelve la ruta de creación.
        /// </summary>
        /// <param name="respuestasProveedores"></param>
        /// <returns></returns>
        public async Task<string> CratePDF(List<Models.RespuestaCotizacion> respuestasProveedores)
        {
            var datosPdf = await GetEstructuraDatosPDF(respuestasProveedores);
            if (datosPdf == null)
            {
                //TODO: Debo resolver que pasa cuando es nulo
                return "";
            }

            //Obtenemos el json de la estructua a enviar y llamamos el API
            var jsonDatosPDF = Newtonsoft.Json.JsonConvert.SerializeObject(datosPdf, Newtonsoft.Json.Formatting.Indented);

            //Creamos el cliente http para consumir el api
            var httpClient = new HttpClient();
            HttpContent httpContent = new StringContent(jsonDatosPDF, Encoding.UTF8, "application/json");
            var messageResponse = await httpClient.PostAsync(_endPoint, httpContent).ConfigureAwait(false);

            var pdfPath = "";
            if (messageResponse.IsSuccessStatusCode)
            {
                //TODO: Responder la ruta del pdf
                pdfPath = await messageResponse.Content.ReadAsStringAsync();
            }

            return pdfPath;

        }

        /// <summary>
        /// Obtiene la estructura de la información necesaria para consumir el servicio de Creación de PDF
        /// </summary>
        /// <param name="respuestasProveedores"></param>
        /// <returns></returns>
        private async Task<EstructuraDatosPDF> GetEstructuraDatosPDF(List<RespuestaCotizacion> respuestasProveedores)
        {
            if (!respuestasProveedores.Any())
            {
                //TODO: generar informe de error.
                return null;
            }

            //Genero el cliente para el PDF
            var cliente = new ClientePDF();
            //Establezco la información general de la cotizacion
            var infoCotizacionPDF = new InfoGeneralCotizacion();

            var cotizaciones = await GetCotizacionesPDF(respuestasProveedores, infoCotizacionPDF, cliente);


            var datosPDF = new EstructuraDatosPDF()
            {
                Cliente = cliente,
                Cotizaciones = cotizaciones.ToList(),
                InfoGeneralCotizacion = infoCotizacionPDF
            };


            return datosPDF;
        }

        /// <summary>
        /// Obtiene la información de la cotizacionm por cada una de las respuesta de cotización
        /// </summary>
        /// <param name="respuestasProveedores"></param>
        /// <param name="infoGeneral"></param>
        /// <param name="cliente"></param>
        /// <returns></returns>
        private async Task<List<RespuestaCotizacionPDF>> GetCotizacionesPDF(List<RespuestaCotizacion> respuestasProveedores, InfoGeneralCotizacion infoGeneral, ClientePDF cliente)
        {
            var cotizacionesPDF = new List<RespuestaCotizacionPDF>();

            var i = 0;
            foreach (var respuesta in respuestasProveedores)
            {
                i++;
                var cotizacion = await _context.Cotizacion.FindAsync(respuesta.IdCotizacion);
                var producto = await _context.Producto.FindAsync(cotizacion.IdProducto);
                var proveedor = await _context.Usuario.FindAsync(producto.IdUsuario);
                if (i == 1)//Esto solo lo ejecuto una única vez para asignar la información del cliente y general de la cotizacion, asi nos evitamos consultar la misma información en base de datos
                {
                    var usuarioCliente = await _context.Usuario.FindAsync(cotizacion.IdUsuario);
                    SetClienteInfo(cliente, usuarioCliente);
                    SetCotizacionInfoGeneral(infoGeneral, cotizacion);
                }
                var precio = (float)respuesta.Precio;
                float precioUnitario = 0;
                if (respuesta.CantidadDisponible <= 0)
                    precioUnitario = 0;
                else
                    precioUnitario = precio / (float)respuesta.CantidadDisponible;

                precioUnitario = precioUnitario == 0 ? precio : precioUnitario;
                cotizacionesPDF.Add(new ModelsPdfService.RespuestaCotizacionPDF()
                {
                    CantidadDisponible = (int)respuesta.CantidadDisponible,
                    Descripcion = respuesta.Descripcion,
                    Disponibilidad = (bool)respuesta.DisponibilidadInmediata,
                    FechaDisponibildiad = respuesta.FechaDisponibilidad == null ? "" : ((DateTime)respuesta.FechaDisponibilidad).ToString("dd - MM - yyyy"),
                    Id = respuesta.IdCotizacion,
                    Item = producto.Nombre,
                    Precio = precio,
                    PrecioUnitario = precioUnitario,
                    Proveedor = proveedor.Nombre
                });
            }

            return cotizacionesPDF;

        }

        /// <summary>
        /// Establece la información general de la cotización para la generación de PDF
        /// </summary>
        /// <param name="infoGeneral"></param>
        /// <param name="cotizacion"></param>
        private void SetCotizacionInfoGeneral(InfoGeneralCotizacion infoGeneral, Cotizacion cotizacion)
        {
            infoGeneral.FechaCotizacion = cotizacion.FechaCotizacion.ToString("dd - MM - yyyy");
            infoGeneral.Referencia = cotizacion.Referencia;
        }

        /// <summary>
        /// Establece la información del cliente para la generación del PDF
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="usuarioCliente"></param>
        private void SetClienteInfo(ClientePDF cliente, Usuario usuarioCliente)
        {

            cliente.Email = usuarioCliente.Email;
            cliente.Identificacion = usuarioCliente.Identificacion;
            cliente.Nombre = usuarioCliente.Nombre;
            cliente.Telefono = usuarioCliente.Contacto;
        }
    }

}





