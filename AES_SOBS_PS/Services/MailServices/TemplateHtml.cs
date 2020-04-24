using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AES_SOBS_PS.Services.MailServices
{
    public static class TemplateHtml
    {

        /// <summary>
        /// Devuelve la plantilla en HTML para cuerpo de correo de nuevo USUARIO (Cliente o Proveedor)
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string GetTemplateForNewUser(params string[] info)
        {
            var body = new StringBuilder();
            body.AppendFormat(@"<a href='{0}/{1}'>CONFIRMA TU EMAIL CON UN CLICK</a>
                                <br/>
                                <p><b>EQUIPO AES SOBS</b></p>", info[0], info[1]);

            return body.ToString();
        }

        /// <summary>
        /// Devuelve plantilla en HTML para cuerpo de correo de nuevo proveedor
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string GetTemplateForNewSupply(params string[] info)
        {
            var body = new StringBuilder();
            body.AppendFormat(@"Estimado(a) Cliente <b>{6}</b><br/><br/>
                                <p>A continuacion se listan los parámetros para la integración<b>{1}</b></p><br/>
                                    <h3>Para Recepción</h3>
                                    <ul>
                                      <li><b>tokenID:</b>{0}</li>
                                      <li><b>tokenAPI:</b>{1}</li>
                                      <li><b>subscriptionName:</b>{2}</li> 
                                    </ul>
                                    <br/>
                                    <h3>Para envío</h3>
                                    <ul>
                                      <li><b>tokenID:</b>{3}</li>
                                      <li><b>topicName:</b>{4}</li>
                                      <li><b>subscriptionName:</b>{4}</li> 
                                      <li><b>Estructura Json de respuesta:</b>{5}</li> 
                                    </ul>
                                <br/>
                                <p><b>EQUIPO AES SOBS</b></p>",
                                info[0],//Token id
                                info[1],//Full token
                                info[2],//userSuscripcion
                                info[3],//TokenRespuesta
                                info[4],// Respuest Proveedor
                                info[5],//
                                info[6]//
                                );

            return body.ToString();
        }


        /// <summary>
        /// Devuelve la plantilla en HTML para cuerpo de correo de la respuesta de la cotización al cliente
        /// </summary>
        /// <param name="info">parametrós {0}NombreCliente, {1}ReferenciaCotizacion</param>
        /// <returns></returns>
        public static string GetTemplateForRepplyQuote(string[] info)
        {
            var body = new StringBuilder();
            body.AppendFormat(@"Estimado(a) Cliente <b>{0}</b><br/><br/>
                                <p>En atención a su solicitud, adjuntamos la cotización referencia <b>{1}</b></p><br/><br/>
                                <p>Gracias por usar nuestro canal!</p><br/><br/>
                                <p>Atentamente,</p><br/>
                                <p><b>EQUIPO AES S.O.B.S</b></p>", info[0], info[1]);

            return body.ToString();
        }
    }
}
