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
                bodyb.AppendFormat("<p><font size = '3' face = 'arial' color = 'blue'>El usuario {0} genero un PCR!</font></p>", usuario);
                bodyb.AppendLine(@"<p><font size = '3' face = 'arial'>El numero de PCR es: <b>" + comment + "</b></font></p>");
                bodyb.AppendLine(@"<p><font size = '3' face = 'arial'>En el siguiente link podras autorizar el PCR:</font></p>");
                var link = @"http://mexico-metales.midas/Ingenieria/PCRs/Review/" + id;
                bodyb.AppendLine("<p><font size = '3' face = 'arial'><a href=\"" + link+ "\" >"+link+ "</a></font></p>");
                bodyb.AppendLine(@"<font size = '2' face = 'arial' color = 'red'><p>Recuerda Revisar sus PCRs! </font></p>");
                bodyb.AppendLine(@"<i>No responder a este correo | Do not reply to this email </i>");

                var message = new MailMessage();
                foreach(var corr in Correo)
                {
                    message.To.Add(new MailAddress(corr));

                }
                message.From = new MailAddress("SJIM_Ingenieria@flexngate.com");
                message.Subject = "Nuevo PCR " + comment + " esperando su aprobación";
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
                bodyb.AppendFormat("<p><font size = '3' face = 'arial' color = 'blue'>El usuario {0} aprobó un PCR por su revisión!</font></p>", usuario);
                bodyb.AppendLine(@"<p><font size = '3' face = 'arial'>El numero de PCR es: <b>" + comment + "</b></font></p>");
                bodyb.AppendLine(@"<p><font size = '3' face = 'arial'>En el siguiente link podras autorizar el PCR:</font></p>");
                var link = @"http://mexico-metales.midas/Ingenieria/PCRs/Review/" + id;
                bodyb.AppendLine("<p><font size = '3' face = 'arial'><a href=\"" + link + "\" >" + link + "</a></font></p>");
                bodyb.AppendLine(@"<b><font size = '2' face = 'arial'>Al autorizar el PCR, será autorizado por el departamento de " + departamento + ".</font></b>");
                bodyb.AppendLine(@"<font size = '2' face = 'arial' color = 'red'><p>Recuerda Revisar sus PCRs! </font></p>");
                bodyb.AppendLine(@"<i>No responder a este correo | Do not reply to this email </i>");

                var message = new MailMessage();
                foreach (var corr in Correo)
                {
                    message.To.Add(new MailAddress(corr));
                }
                message.From = new MailAddress("SJIM_Ingenieria@flexngate.com"); 
                message.Subject = "Nuevo PCR " + comment + " esperando su aprobación";
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
        public void Arreglos(string[] Correo, string usuario, string comment, string id)
        {
            try
            {
                var bodyb = new StringBuilder();
                bodyb.AppendFormat("<p><font size = '3' face = 'arial' color = 'blue'>El usuario {0} dice su PCR ocupa arreglos!</font></p>", usuario);
                bodyb.AppendLine(@"<p><font size = '3' face = 'arial'>El numero de PCR es: <b>" + comment + "</b></font></p>");
                bodyb.AppendLine(@"<p><font size = '3' face = 'arial'>En el siguiente link podras arreglar el PCR:</font></p>");
                var link = @"http://mexico-metales.midas/Ingenieria/PCRs/Review/" + id;
                bodyb.AppendLine("<p><font size = '3' face = 'arial'><a href=\"" + link + "\" >" + link + "</a></font></p>");
                bodyb.AppendLine(@"<font size = '2' face = 'arial' color = 'red'><p>Recuerda Revisar sus PCRs! </font></p>");
                bodyb.AppendLine(@"<i>No responder a este correo | Do not reply to this email </i>");

                var message = new MailMessage();
                foreach (var corr in Correo)
                {
                    message.To.Add(new MailAddress(corr));
                }
                message.From = new MailAddress("SJIM_Ingenieria@flexngate.com");
                message.Subject = "Su Nuevo PCR " + comment + " está esperando arreglos";
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
        public void Arreglado(string[] Correo, string usuario, string comment, string id)
        {
            try
            {
                var bodyb = new StringBuilder();
                bodyb.AppendFormat("<p><font size = '3' face = 'arial' color = 'blue'>El usuario {0} modificó su PCR para que revisa de nuevo!</font></p>", usuario);
                bodyb.AppendLine(@"<p><font size = '3' face = 'arial'>El numero de PCR es: <b>" + comment + "</b></font></p>");
                bodyb.AppendLine(@"<p><font size = '3' face = 'arial'>En el siguiente link podras aprobar el PCR:</font></p>");
                var link = @"http://mexico-metales.midas/Ingenieria/PCRs/Review/" + id;
                bodyb.AppendLine("<p><font size = '3' face = 'arial'><a href=\"" + link + "\" >" + link + "</a></font></p>");
                bodyb.AppendLine(@"<font size = '2' face = 'arial' color = 'red'><p>Recuerda Revisar sus PCRs! </font></p>");
                bodyb.AppendLine(@"<i>No responder a este correo | Do not reply to this email </i>");

                var message = new MailMessage();
                foreach (var corr in Correo)
                {
                    message.To.Add(new MailAddress(corr));
                }
                message.From = new MailAddress("SJIM_Ingenieria@flexngate.com");
                message.Subject = "PCR " + comment + " fue modificado, revisarlo de nuevo";
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
        public void Rechazado(string[] Correo, string usuario, string comment, string id)
        {
            try
            {
                var bodyb = new StringBuilder();
                bodyb.AppendFormat("<p><font size = '3' face = 'arial' color = 'blue'>El usuario {0} rechazó su PCR!</font></p>", usuario);
                bodyb.AppendLine(@"<p><font size = '3' face = 'arial'>El numero de PCR es: <b>" + comment + "</b></font></p>");
                bodyb.AppendLine(@"<p><font size = '3' face = 'arial'>En el siguiente link podras modificar el PCR:</font></p>");
                var link = @"http://mexico-metales.midas/Ingenieria/PCRs/Edit/" + id;
                bodyb.AppendLine("<p><font size = '3' face = 'arial'><a href=\"" + link + "\" >" + link + "</a></font></p>");
                bodyb.AppendLine(@"<font size = '2' face = 'arial' color = 'red'><p>Recuerda Revisar sus PCRs! </font></p>");
                bodyb.AppendLine(@"<i>No responder a este correo | Do not reply to this email </i>");

                var message = new MailMessage();
                foreach (var corr in Correo)
                {
                    message.To.Add(new MailAddress(corr));
                }
                message.From = new MailAddress("SJIM_Ingenieria@flexngate.com");
                message.Subject = "PCR " + comment + " fue rechazado";
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
        public void comments(string[] Correo, string usuario, string comment, bool muy_bien, bool bien, bool mediocre, bool mal, bool muy_mal)
        {
            try
            {
                var bodyb = new StringBuilder();
                bodyb.AppendFormat("<p><font size = '3' face = 'arial' color = 'blue'>El usuario {0} hizo un comentario!</font></p>", usuario);
                if (muy_bien)
                {
                    bodyb.AppendLine(@"<p><font size = '3' face = 'arial'> Opinión de la aplicación es <b> Excelente </b></font></p>");
                }
                else if (bien)
                {
                    bodyb.AppendLine(@"<p><font size = '3' face = 'arial'> Opinión de la aplicación es <b> Buena </b></font></p>");
                }
                else if (mediocre)
                {
                    bodyb.AppendLine(@"<p><font size = '3' face = 'arial'> Opinión de la aplicación es <b> Regular </b></font></p>");
                }
                else if (mal)
                {
                    bodyb.AppendLine(@"<p><font size = '3' face = 'arial'> Opinión de la aplicación es <b> Mala </b></font></p>");
                }
                else if (muy_mal)
                {
                    bodyb.AppendLine(@"<p><font size = '3' face = 'arial'> Opinión de la aplicación es <b> Pésima </b></font></p>");
                }
                bodyb.AppendLine(@"<p><font size = '3' face = 'arial'>El comentario fue:  <b>" + comment + "</b></font></p>");

                var message = new MailMessage();
                foreach (var corr in Correo)
                {
                    message.To.Add(new MailAddress(corr));
                }
                message.From = new MailAddress("SJIM_Ingenieria@flexngate.com");
                message.Subject = "Comentario de: " + usuario;
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
		public void NotifyFinalApproval(string[] Correo, string usuario, string comment, string id)
		{
			try
			{
				var bodyb = new StringBuilder();
				bodyb.AppendFormat("<p><font size='3' face='arial' color='blue'>El usuario {0} ha completado la aprobación del PCR!</font></p>", usuario);
				bodyb.AppendLine(@"<p><font size='3' face='arial'>El número de PCR es: <b>" + comment + "</b></font></p>");
				bodyb.AppendLine(@"<p><font size='3' face='arial'>El proceso ha finalizado y no se requieren más acciones.</font></p>");
				bodyb.AppendLine(@"<p><font size='2' face='arial' color='green'><b>¡Felicidades, el PCR ha sido aprobado!</b></font></p>");
				bodyb.AppendLine(@"<i>No responder a este correo | Do not reply to this email </i>");

				var message = new MailMessage();
				foreach (var corr in Correo)
				{
					message.To.Add(new MailAddress(corr));
				}
				message.From = new MailAddress("SJIM_Ingenieria@flexngate.com");
				message.Subject = "PCR " + comment + " ha sido aprobado completamente";
				message.IsBodyHtml = true;
				message.Body = bodyb.ToString();

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