using EnvioDeCorreos.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace EnvioDeCorreos.Helpers
{
    public static class Utils
    {
        public static bool validarInfo(Email email, out string mensaje)
        {
            bool res = true;
            mensaje = "";
            int cantidadDestinatario = email.destinatarios.Count;
            int cont = 0;
            //Validamos que la información requerida este contenida
            if (cantidadDestinatario > 0)
            {
                foreach (var destinatario in email.destinatarios)
                {
                    if (string.IsNullOrEmpty(destinatario))
                    {
                        cont++;
                    }
                }

                if (cont == cantidadDestinatario)
                {
                    mensaje = "Se requiere al menos un destinatario";
                    return res;
                }
            }
            else
            {
                mensaje = "Se requiere al menos un destinatario";
                return res;
            }

            if (string.IsNullOrEmpty(email.credenciales.email) || string.IsNullOrEmpty(email.credenciales.password))
            {
                mensaje = "Se necesita las credenciales del correo para autenticar en el servidor";
                return res;
            }
            return res;
        }       


        private static string obtenerEmailTo(List<string> destinatarios)
        {
            string emails = "";
            foreach(var destinaratio in destinatarios)
            {
                emails += destinaratio + ";";
            }
            return emails;
        }
        private static bool validarCC(List<string> cc)
        {
            bool res = false;
            int cantidadCC = cc.Count;
            int cont = 0;
            //Validamos que la información requerida este contenida
            if (cantidadCC > 0)
            {
                foreach (var c in cc)
                {
                    if (string.IsNullOrEmpty(c))
                    {
                        cont++;
                    }
                }

                if (cont != cantidadCC)
                {
                    res = true;
                }
            }
            return res;
        }


        public static bool enviarCorreo(Email email, out string mensaje)
        {
            bool res = validarInfo(email, out mensaje);
            string toEmails = "";
            string CCEmails = "";
            if (res)
            {
                try
                {
                    MailMessage Msg = new MailMessage();       
                    MailAddress fromMail = new MailAddress(email.credenciales.email, email.credenciales.nombre);
                    Msg.From = fromMail;
                    toEmails = obtenerEmailTo(email.destinatarios).TrimEnd(';');
                    foreach (var address in toEmails.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        Msg.To.Add(address);
                    }
                    if (validarCC(email.cc))
                    {
                        CCEmails = obtenerEmailTo(email.cc).TrimEnd(';');
                        foreach (var ccaddress in CCEmails.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            Msg.CC.Add(ccaddress);
                        }
                    }

                    string fromPassword = email.credenciales.password;
                    string subject = email.asunto;
                    string body = email.cuerpo;

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.office365.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromMail.Address, fromPassword)
                    };

                    Msg.Subject = subject;
                    Msg.Body = body;

                    smtp.Send(Msg);
                    mensaje = "Correo enviado";
                }catch(Exception ex)
                {
                    res = false;
                    mensaje = ex.Message;
                }
                
            }

            return res;

        }
    }
}
