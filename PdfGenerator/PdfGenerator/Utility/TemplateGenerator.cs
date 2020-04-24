using PdfGenerator.Models;
using System;
using System.Text;

namespace PdfGenerator.Utility
{
    public class TemplateGenerator
    {
        private IEstructuraDatosPDF datos;

        public TemplateGenerator(EstructuraDatosPDF datos)
        {
            this.datos = datos;
        }
        public string GetHTMLString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat(@"
                        <html>
                            <head>
                            </head>
                            <body>
			                    <div class='header'>
                                <img src='{6}' alt='Logo'>
                                    <h1>COTIZACIÓN</h1>
				                    <h3>Referencia {0}</h2>
			                    </div>
			                    <div class='globalInfo'>
				                    <p><b>Fecha de Envío: {7}</p>
				                    <p><b>Fecha de Solicitud: {1}</p>
				                    <p><b>Nombre: </b>{2}</p>
				                    <p><b>Identificación: </b>{3}</p>
				                    <p><b>Email: </b>{4}</p>
				                    <p><b>Teléfono: </b>{5}</p>
				                    <br/><br/>
			                    </div>
			                    <table align='center'>
				                    <tr>
					                    <th>Proveedor</th>
					                    <th>Producto / Servicio</th>
					                    <th>Cantidad Disponible</th>
					                    <th>Precio</th>
					                    <th>Precio Unidad</th>
					                    <th>Disponibilidad Inmediata</th>					
					                    <th>Descripcion</th>
				                    </tr>",
                                    datos.InfoGeneralCotizacion.Referencia,
                                    datos.InfoGeneralCotizacion.FechaCotizacion,
                                    datos.Cliente.Nombre,
                                    datos.Cliente.Identificacion,
                                    datos.Cliente.Email,
                                    datos.Cliente.Telefono,
                                    datos.InfoGeneralCotizacion.Logo,
                                    DateTime.Now.ToString("dd - MM - yyyy")
                                    );

            foreach (var quot in datos.Cotizaciones)
            {
                sb.AppendFormat(@"<tr>
				                    <td>{0}</td>
				                    <td>{1}</td>
				                    <td>{2}</td>
				                    <td>{3}</td>
				                    <td>{4}</td>
				                    <td>{5}</td>				
				                    <td>{6}</td>
                                  </tr>",
                                  quot.Proveedor,
                                  quot.Item,
                                  quot.CantidadDisponible,
                                  quot.Precio,
                                  quot.PrecioUnitario,
                                  quot.Disponibilidad ? "SI" : "NO",
                                  quot.Descripcion
                                  );
            }

            sb.Append(@"
                                </table>
                            </body>
                        </html>");

            return sb.ToString();
        }
    }
}
