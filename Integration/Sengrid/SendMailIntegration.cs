using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SendGrid;
using SendGrid.Helpers.Mail;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;


namespace appejemplo2.Integration.Sengrid
{
    public class SendMailIntegration
    {
         public const string SEND_SENDGRID="SENDGRID";
        public const string SEND_REST="REST";
        private string ACCESS_TOKEN ="";
         private string From = "jeanpierre_montalvo@usmp.pe"; // settings\sender authentification
        private string FromLabel = "Mail Service"; 

         private const string URL_API_SENDGRID = "https://api.sendgrid.com/v3/mail/send";


        public async Task SendMail(string correoDestino,string userDestino,string titulo, string contenido,string method){
            ACCESS_TOKEN = System.Environment.GetEnvironmentVariables()["SENDGRID_KEY"].ToString();
            if(method.Equals(SEND_SENDGRID)){
                SendMailSengrid(correoDestino, userDestino , titulo, contenido);
            }else{
                SendMailSengridRest(correoDestino, userDestino, titulo, contenido);
            }
        }
        private async Task SendMailSengrid(string correoDestino,string userDestino, string titulo, string contenido){ 

             var client = new SendGridClient(ACCESS_TOKEN);
            var from = new EmailAddress(From, FromLabel);
            var subject = titulo;
            var to = new EmailAddress(correoDestino, userDestino);
            var plainTextContent = contenido;
            var htmlContent = "";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

        }
        private async Task SendMailSengridRest(string correoDestino, string userDestino, string titulo, string contenido){ 
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(URL_API_SENDGRID);
             httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);
  
            var jsonObject = new StringBuilder();
            jsonObject.Append("{");
            jsonObject.Append("\"categories\": [");
            jsonObject.Append("\"demo\" ");
            jsonObject.Append("],");
            jsonObject.Append("\"from\": {");
            jsonObject.Append("\"email\": \"fduartej@usmp.pe\","); 
            jsonObject.Append("\"name\": \"Frederick \"");
            jsonObject.Append("},");
            jsonObject.Append("\"personalizations\": [");
            jsonObject.Append("{");
            jsonObject.Append("      \"to\": [");
            jsonObject.Append("        {");
            jsonObject.Append("\"email\": \""+correoDestino+"\",");
            jsonObject.Append("\"name\": \"Fred D\" ");
            jsonObject.Append("}");
            jsonObject.Append("],");
            jsonObject.Append("\"subject\": \"Hola\" ");
            jsonObject.Append("}");
            jsonObject.Append("],");
            jsonObject.Append("\"content\": [");
            jsonObject.Append("{");
            jsonObject.Append("\"type\": \"text/plain\",");
            jsonObject.Append("\"value\": \"Hola ahora ya uso sendgrid!\" ");
            jsonObject.Append("}");
            jsonObject.Append("],  ");
            jsonObject.Append("}");

            var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync(URL_API_SENDGRID, content);
        }
    }
}