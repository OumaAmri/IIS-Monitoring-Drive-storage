using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace Automation_Monitoring_MyOCP_1.Services.Emails
{
    public class EmailServices
    {
        #region Readonlys
        private readonly int port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SMTP_PORT"]);
        private readonly string displayName = System.Configuration.ConfigurationManager.AppSettings["EmailDisplayName"];
        private readonly string sourceEmail = System.Configuration.ConfigurationManager.AppSettings["EMAIL"];
        private readonly string host = System.Configuration.ConfigurationManager.AppSettings["SMTP_HOST"];
        private readonly string userName = System.Configuration.ConfigurationManager.AppSettings["SMTP_USER"];
        private readonly string password = System.Configuration.ConfigurationManager.AppSettings["SMTP_PASSWORD"];
        private readonly bool enableSsl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ENABLE_SSL"]);
        private readonly string listEmailTo = System.Configuration.ConfigurationManager.AppSettings["List_Email_To"];
        #endregion
        public void SendEmail(string Subject, string Body)
        {
            try
            {
                List<string> Tos = new List<string>();
                Tos = listEmailTo.Split(',').ToList();                              

                var message = new MailMessage();
                foreach (var to in Tos)
                {
                    message.To.Add(to);
                }               
                message.Subject = Subject;
                message.Body = Body;
                message.IsBodyHtml = true;
                message.From = new MailAddress(sourceEmail, displayName);
                SmtpClient smtpClient = new SmtpClient(host, port);
                smtpClient.EnableSsl = enableSsl;
                smtpClient.Credentials = new System.Net.NetworkCredential(userName, password);
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                Log.Error("Exception when sending Email : " + ex.Message + "\nInnerException : " + ex.InnerException.Message + "\n");
            }
        }
    }
}
