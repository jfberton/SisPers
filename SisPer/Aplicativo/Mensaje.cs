using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SisPer.Aplicativo
{
    public partial class Mensaje
    {
        public string Encabezado()
        {

            /*
             <h3><span style="font-size:9px"><span style="color:#808080">lunes 14/04/2015 08:27:35</span></span></h3>    <h2><span style="color:rgb(128, 128, 128)"><span style="font-size:18px">INSAURRALDE, Ada Margarita&nbsp;</span></span></h2>    <h3><span style="color:rgb(128, 128, 128)"><span style="font-size:14px">Prueba de mensaje a distintos destinatarios</span></span></h3>    <h4><span style="color:rgb(128, 128, 128)"><span style="font-size:12px">INSAURRALDE, Ada Margarita; Mena, Gonzalo Matías; Rodriguez, Martín Alejandro; Montiel, Sebasti&aacute;n Omar; Bertoncini, Jos&eacute; Federico; Fernandez, Alejandro Martín;</span></span></h4>    <hr />  <p>aasdasdasd</p>    <p>a</p>    <p>sd</p>    <p>asd</p>    <p>as</p>    <p>da</p>    <p>sd</p>    <p>a</p>    <p>sdasd</p>  
             */
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<h3><span style=\"font-size:9px\"><span style=\"color:#808080\">" + this.FechaEnvio.ToLongDateString() + " " + this.FechaEnvio.ToLongTimeString() + "</span></span><br />");
            sb.AppendLine("<span style=\"color:#808080\"><span style=\"font-size:20px\">" + this.Agente.ApellidoYNombre + "</span></span><br />");
            sb.AppendLine("<span style=\"color:#808080\"><span style=\"font-size:16px\">" + this.Asunto + "</span></span><br />");
            sb.AppendLine("<span style=\"color:#808080\"><span style=\"font-size:12px\">Para: ");
            
            foreach (Destinatario destinatario in this.Destinatarios)
            {
                sb.Append(destinatario.Agente.ApellidoYNombre + "; ");
            }

            sb.AppendLine("</span></span></h3>");
            sb.AppendLine("<hr />");

            return sb.ToString();
        }
    }
}