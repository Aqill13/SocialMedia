using BusinessLayer.Abstract;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class EmailManager : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toName, string toEmail, string subject, string body)
        {
            string host = _configuration["Smtp:Host"]!;
            int port = int.Parse(_configuration["Smtp:Port"]!);
            string fromEmail = _configuration["Smtp:FromEmail"]!;
            bool enableSsl = bool.Parse(_configuration["Smtp:EnableSsl"]!);
            string username = _configuration["Smtp:Username"]!;
            string password = _configuration["Smtp:Password"]!;

            MimeMessage mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("Admin", fromEmail));
            mimeMessage.To.Add(new MailboxAddress(toName, toEmail));
            mimeMessage.Subject = subject;
            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(host, port, enableSsl);
            await client.AuthenticateAsync(username, password);
            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
        }
    }
}
