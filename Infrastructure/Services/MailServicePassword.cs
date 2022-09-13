using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using dotnet_2.Infrastructure.Data.Models;

namespace dotnet_2.Infrastructure.Data.Services
{
    class MailServicePassword : IMailServicePassword
    {
        private readonly MailSettings _mailSettings;
        public MailServicePassword(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendEmailPasswordAsync(MailRequestPassword mailRequestPassword)
        {

            Console.WriteLine(_mailSettings.host);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.from);
            email.To.Add(MailboxAddress.Parse(mailRequestPassword.to_email));
            email.Subject = mailRequestPassword.subject;
            var builder = new BodyBuilder();

            builder.HtmlBody = "Your New Password is "+ mailRequestPassword?.body?.ToString();
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.host, _mailSettings.port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.username, _mailSettings.password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}