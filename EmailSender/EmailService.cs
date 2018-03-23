using EmailSender.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace EmailSender
{
    public interface IEmailService
    {
        void Send(EmailModel email);
    }

    public class EmailService : IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailService(IEmailConfiguration emailConfiguration) => _emailConfiguration = emailConfiguration;

        public void Send(EmailModel email)
        {
            var message = new MimeMessage();
            message.To.Add(new MailboxAddress(_emailConfiguration.Username));
            message.From.Add(new MailboxAddress(email.From));
            message.Body = new TextPart(TextFormat.Text) { Text = email.Body };

            using (var client = new SmtpClient())
            {
                TryConnect(client);
                client.Send(message);
                client.Disconnect(true);
            }
        }

        void TryConnect(SmtpClient client)
        {
            try
            {
                Connect(client);
            }
            catch (SslHandshakeException)
            {
                SetServerCertificateValidationCallback(client);
                Connect(client);
            }
        }

        void Connect(SmtpClient client)
        {
            client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(_emailConfiguration.Username, _emailConfiguration.Password);
        }

        // See https://github.com/jstedfast/MailKit/issues/307
        void SetServerCertificateValidationCallback(SmtpClient client)
        {
            client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
                true; // Remove validation completely... only thing that works so far...
        }
    }
}
