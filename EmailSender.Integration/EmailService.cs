﻿using EmailSender.Models;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MimeKit.Text;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace EmailSender.Integration
{
    public interface IEmailService
    {
        List<EmailModel> SearchInboxByBody(string searchstring);
    }

    public class EmailService : IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailService(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public List<EmailModel> SearchInboxByBody(string searchstring)
        {
            using (var client = new ImapClient())
            {
                TryConnect(client);

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                return inbox.Search(SearchQuery.BodyContains(searchstring))
                     .Select(x => new EmailModel { Body = inbox.GetMessage(x).GetTextBody(TextFormat.Text) })
                     .ToList();
            }
        }

        void TryConnect(ImapClient client)
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

        // See https://github.com/jstedfast/MailKit/issues/307
        void SetServerCertificateValidationCallback(ImapClient client)
        {
            //if (_emailConfiguration.ImapServer == "imap.gmail.com")
            //  CheckForImapGmailKnownThumbprint(client);
            //else
            client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
                true; // Remove validation completely... only thing that works so far...

            //MailService.DefaultServerCertificateValidationCallback(sender, certificate, chain, sslPolicyErrors); // doesn't work
        }

        void CheckForImapGmailKnownThumbprint(ImapClient client)
        {
            client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
            {
                if (certificate is X509Certificate2 && ((X509Certificate2)certificate).Thumbprint == "71DFBE124D89ED9218F539A82D4127FBB4BC9997")
                    return true;

                return false;
            };
        }

        void Connect(ImapClient client)
        {
            client.Connect(_emailConfiguration.ImapServer, _emailConfiguration.ImapPort, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(_emailConfiguration.Username, _emailConfiguration.Password);
        }
    }
}
