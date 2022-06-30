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

        public void UpdateMetrico(string Correo, string usuario, string idoil, string comment, string User_Area )
        {
            try
            {
                var body =
                    "<h4>El usuario " + usuario + " actualizó el Metrico (" + User_Area + ") No. {1} </h4>" +
                    "<p> La descripción de la actividad es:</p>" +
                    "<h3>{2}</h3>" +
                    "<p>Recuerda reviar los Metricos!</p>" +
                    "<i>No responder a este correo | Do not reply to this email </i>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(Correo)); 
                message.From = new MailAddress("SJIM_Ingenieria@flexngate.com"); 
                message.Subject = "Informacion Bitacora, No Responder a este correo";
                message.Body = string.Format(body, usuario, idoil, comment);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.flexngate.local";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.Send(message);
                }
            }
            catch (Exception Ex)
            { var x = Ex; }
        }

        public void updateoil(string Correo,string usuario, string idoil ,string comment)
        {
            try
            {
                var body = 
                    "<h4>El usuario {0} actualizó el OIL No. {1} </h4>" +
                    "<p> La descripción de la actividad es:</p>" +
                    "<h3>{2}</h3>" +
                    "<p>Recuerda revisar los OILs!</p>" +
                    "<i>No responder a este correo | Do not reply to this email </i>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(Correo)); 
                message.From = new MailAddress("SJIM_Ingenieria@flexngate.com"); 
                message.Subject = "Informacion Bitacora, No Responder a este correo";
                message.Body = string.Format(body, usuario, idoil, comment);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.flexngate.local";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.Send(message);
                }
            }
             catch(Exception Ex)
            { var x = Ex; }            
        }

        public void NewMetrico(string Correo, string usuario, string comment, string User_Area)
        {
            try
            {
                var body =
                    "<h4>El usuario {0} generó un Metrico (" + User_Area + ") y estas como responsable de la actividad!</h4>" +
                    "<p> La descripción de la actividad es:</p>" +
                    "<h3>{1}</h3>" +
                    "<p>Recuerda revisar los Metricos!</p>" +
                    "<i>No responder a este correo | Do not reply to this email </i>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(Correo));  
                message.From = new MailAddress("SJIM_Ingenieria@flexngate.com");  
                message.Subject = "Informacion Bitacora, No Responder a éste Correo";
                message.Body = string.Format(body, usuario, comment);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.flexngate.local";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.Send(message);
                }
            }
            catch (Exception Ex) { var x = Ex; }
        }

        public void newoil(string Correo, string usuario, string comment)
        {
            try { 
                var body = 
                    "<h4>El usuario {0} generó un OIL y estas como responsable de la actividad!</h4>" +
                    "<p> La descripción de la actividad es:</p>" +
                    "<h3>{1}</h3>" +
                    "<p>Recuerda actualizar los OILs!</p>" +
                    "<i>No responder a este correo | Do not reply to this email </i>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(Correo));   
                message.From = new MailAddress("SJIM_Ingenieria@flexngate.com"); 
                message.Subject = "Informacion Bitacora, No Responder a éste Correo";
                message.Body = string.Format(body, usuario, comment);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
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
                bodyb.AppendLine(@"<p>El numero de PCR es: <b>" + comment + "</b></p>");
                var link = @"http://sjimsvap7/bitacora/PCRs/Review/" + id;
                bodyb.AppendLine("<p><a href=\"" + link+ "\" >"+link+ "</a></p>");
                bodyb.AppendLine(@"<p>Recuerda Verificar los PCRs! </p>");
                bodyb.AppendLine(@"<i>No responder a este correo | Do not reply to this email </i>");

                var message = new MailMessage();
                foreach(var corr in Correo)
                {
                    message.To.Add(new MailAddress(corr));

                }
                message.From = new MailAddress("SJIM_Ingenieria@flexngate.com"); 
                message.Subject = "Nuevo PCR para su revisión";
                message.IsBodyHtml = true;
                message.Body = bodyb.ToString();
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.flexngate.local";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.Send(message);
                }
            }
            catch (Exception Ex) { var x = Ex; }
        }

        public void newReview(string[] Correo, string usuario, string comment, string id, string departamento)
        {
            try
            {
                var bodyb = new StringBuilder();
                bodyb.AppendFormat("El usuario {0} Genero un PCR!", usuario);
                bodyb.AppendLine(@"<p>El numero de PCR es: <b>" + comment + "</b></p>");
                bodyb.AppendLine(@"<p>En el siguiente link podras autorizar el PCR:</p>");
                var link = @"http://sjimsvap7/bitacora/PCRs/Review/" + id;
                bodyb.AppendLine("<p><a href=\"" + link + "\" >" + link + "</a></p>");
                bodyb.AppendLine(@"<b>Al autorizar el PCR, será autorizado por el departamento de " + departamento + ".</b>");
                bodyb.AppendLine(@"<p>Recuerda Verificar los PCRs! </p>");
                bodyb.AppendLine(@"<i>No responder a este correo | Do not reply to this email </i>");

                var message = new MailMessage();
                foreach (var corr in Correo)
                {
                    message.To.Add(new MailAddress(corr));
                }
                message.From = new MailAddress("SJIM_Ingenieria@flexngate.com"); 
                message.Subject = "Nuevo PCR esperando su aprobación";
                message.IsBodyHtml = true;
                message.Body = bodyb.ToString();
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
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