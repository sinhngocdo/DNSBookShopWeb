using Google.Apis.Auth.OAuth2;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DNSBookShopWeb.Utility
{
    public class EmailSender : IEmailSender
    {
        

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            
            //logic to send email
            var emailToSend = new MimeMessage();
            emailToSend.From.Add(MailboxAddress.Parse("DNSBookShopWeb@gmail.com"));
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = subject;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };

            using(var emailClient = new SmtpClient())
            {
                emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate("DNSBookShopWeb@gmail.com", "qjgg jerm dsai cnhh");
                emailClient.Send(emailToSend);
                emailClient.Disconnect(true);
            }

            return Task.CompletedTask;
        }
    }
}
