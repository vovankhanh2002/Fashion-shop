using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BanDoWeb.Model.Models
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }
        public Task Execute(string email, string subject, string message)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Port = 587,
                Credentials = new NetworkCredential("khanhvo06062002@gmail.com", "ibppgegiozcymhsq"),
                EnableSsl = true,
                UseDefaultCredentials = false
        };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("khanhvo06062002@gmail.com"),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);
          
            return  smtpClient.SendMailAsync(mailMessage);
        }
    }
}
