using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace ProjectX.Controllers
{
   

    public class EmailSender 
    {
        private readonly string smtpServer;
        private readonly int smtpPort;
        private readonly string smtpUsername;
        private readonly string smtpPassword;
        private readonly string senderEmail;
        private readonly string senderName;

        public EmailSender(IConfiguration configuration)
        {
            smtpServer = configuration.GetValue<string>("SmtpSettings:SmtpServer", "");
            smtpPort = configuration.GetValue<int>("SmtpSettings:SmtpPort", 0);
            smtpUsername = configuration.GetValue<string>("SmtpSettings:SmtpUsername", "");
            smtpPassword = configuration.GetValue<string>("SmtpSettings:SmtpPassword", "");
            senderEmail = configuration.GetValue<string>("SmtpSettings:SenderEmail", "no-reply@hrplatform.com");
            senderName = configuration.GetValue<string>("SmtpSettings:SenderName", "HR Recruitment Platform");
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(senderName, senderEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlBody
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(smtpUsername, smtpPassword);
            
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
