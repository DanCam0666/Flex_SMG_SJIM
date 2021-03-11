using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace Flex_SGM.emaildata
{
    public class EmailController 
    {
        // GET: Email

        public void updateoil(string Correo,string usuario, string idoil ,string comment)
        {
            try
            {
                var body = "<p>El usuario {0} Actualizo el OIL No. {1} </p><p> El comentario fue:</p><p>{2}</p><p>Recuerda Actualizar los OILS!</p><p>No Responder a este Correo|Do not Reply this Email </p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(Correo));  // replace with valid value 
                message.From = new MailAddress("SJIMBitacora@flexngate.com");  // replace with valid value
                message.Subject = "Informacion Bitacora, No Responder a este Correo";
                message.Body = string.Format(body, usuario, idoil, comment);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    /*
                    var credential = new NetworkCredential
                    {
                        UserName = "SJIMBitacora@flexngate.com",  // replace with valid value
                        Password = "Flex2020#"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    */
                    smtp.Host = "smtp.flexngate.local";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.Send(message);
                }
            }
             catch(Exception Ex)
            { var x = Ex; }
            
        }

        public void newoil(string Correo, string usuario, string comment)
        {
            try { 
            var body = "<p>El usuario {0} Genero un OIL y estas como responsable de la actividad!</p><p> La Actividad es:</p><p>{1}</p><p>Recuerda Actualizar los OILS!</p><p>No Responder a este Correo|Do not Reply this Email </p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress(Correo));  // replace with valid value 
            message.From = new MailAddress("SJIMBitacora@flexngate.com");  // replace with valid value
            message.Subject = "Informacion Bitacora, No Responder a este Correo";
            message.Body = string.Format(body, usuario, comment);
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                /*
                var credential = new NetworkCredential
                {
                    UserName = "SJIMBitacora@flexngate.com",  // replace with valid value
                    Password = "Flex2020#"  // replace with valid value
                };
                smtp.Credentials = credential;
                */
                smtp.Host = "smtp.flexngate.local";
                smtp.Port = 25;
                smtp.EnableSsl = false;
                smtp.Send(message);
            }
            }
            catch (Exception Ex) { var x = Ex; }


        }

        public void newpcr(string[] Correo, string usuario, string comment, string id)
        {
            try
            {
                var bodyb = new StringBuilder();
                bodyb.AppendFormat("El usuario {0} Genero un PCR!", usuario);
                bodyb.AppendLine(@"<p>El numero de PCR es: "+ comment + " </p>");
                var link = @"http://sjimsvap3/bitacora/pcrs/Details/" + id;
                bodyb.AppendLine("<p><a href=\"" + link+ "\" >"+link+ "</a></p>");
                bodyb.AppendLine(@"<p>Recuerda Verificar los PCRs! </p>");
                bodyb.AppendLine(@"<p>No Responder a este Correo|Do not Reply this Email</p>");

                var message = new MailMessage();
                foreach(var corr in Correo)
                {
                    message.To.Add(new MailAddress(corr));

                }
               // replace with valid value 
                message.From = new MailAddress("SJIMBitacora@flexngate.com");  // replace with valid value
                message.Subject = "Informacion Bitacora, No Responder a este Correo";
                message.IsBodyHtml = true;
                message.Body = bodyb.ToString();
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    /*
                    var credential = new NetworkCredential
                    {
                        UserName = "SJIMBitacora@flexngate.com",  // replace with valid value
                        Password = "Flex2020#"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    */
                    smtp.Host = "smtp.flexngate.local";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.Send(message);
                }
            }
            catch (Exception Ex) { var x = Ex; }


        }
        public void newReview(string[] Correo, string usuario, string comment, string id)
        {
            try
            {
                var bodyb = new StringBuilder();
                bodyb.AppendFormat("El usuario {0} Verifico un PCR!", usuario);
                bodyb.AppendLine(@"<p>El numero de PCR es: " + comment + " </p>");
                var link = @"http://sjimsvap3/bitacora/pcrs/Review/" + id;
                bodyb.AppendLine("<p><a href=\"" + link + "\" >" + link + "</a></p>");
                bodyb.AppendLine(@"<p>Se requiere de tu autorizacion! </p>");
                bodyb.AppendLine(@"<p>Recuerda Verificar los PCRs! </p>");
                bodyb.AppendLine(@"<p>No Responder a este Correo|Do not Reply this Email</p>");

                var message = new MailMessage();
                foreach (var corr in Correo)
                {
                    message.To.Add(new MailAddress(corr));

                }
                // replace with valid value 
                message.From = new MailAddress("SJIMBitacora@flexngate.com");  // replace with valid value
                message.Subject = "Informacion Bitacora, No Responder a este Correo";
                message.IsBodyHtml = true;
                message.Body = bodyb.ToString();
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    /*
                    var credential = new NetworkCredential
                    {
                        UserName = "SJIMBitacora@flexngate.com",  // replace with valid value
                        Password = "Flex2020#"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    */
                    smtp.Host = "smtp.flexngate.local";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.Send(message);
                }
            }
            catch (Exception Ex) { var x = Ex; }


        }
    }
}



//  <button onclick="location.href='@Url.Action("Index", "Email")';return false;">email</button>

    /*
     *         public void Index()
        {
            int i = 0;
            while (i!=1)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("lolivarez@flexngate.com"));  // replace with valid value 
                message.From = new MailAddress("flexbitacora@outlook.com");  // replace with valid value
                message.Subject = "Prueba";
                message.Body = "Prueba de correo";//string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "flexbitacora@outlook.com",  // replace with valid value
                        Password = "Flex2020#"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.office365.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Send(message);
                }
                i++;
            }
        }
    }
}
     * */