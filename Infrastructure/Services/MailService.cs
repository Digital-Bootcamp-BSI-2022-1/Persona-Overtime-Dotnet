using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using dotnet_2.Infrastructure.Data.Models;

namespace dotnet_2.Infrastructure.Data.Services
{
    class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {

            Console.WriteLine(_mailSettings.host);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.from);
            email.To.Add(MailboxAddress.Parse(mailRequest.to_email));
            email.Subject = mailRequest.subject;
            var builder = new BodyBuilder();

            builder.HtmlBody = "Your OTP is "+ mailRequest.body.ToString();
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.host, _mailSettings.port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.username, _mailSettings.password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}