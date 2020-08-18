using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.ComponentModel;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.SendCompleted+= SmtpClient_OnCompleted;
                mail.From = new MailAddress(Properties.EmailConfig.from);
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = body; 
                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(Properties.EmailConfig.from, Properties.EmailConfig.password);
                SmtpServer.EnableSsl = true;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

                   await SmtpServer.SendMailAsync(mail);
                    SmtpServer.Dispose();
                    mail.Dispose();
            }
            catch (Exception)
            {
            }
        }
        private void SmtpClient_OnCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //Get the Original MailMessage object
            var mess = e.UserState;
            if (e.Cancelled)
            {
                //  return false;
            }
            if (e.Error != null)
            {
                // return false;
            }
            else
            {
             
            }
        }
    }
}
