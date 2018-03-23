using Microsoft.Extensions.Configuration;

namespace EmailSender.Integration
{
    public interface IIntegrationEmailConfiguration
    {
        string ImapServer { get; }
        int ImapPort { get; }
        string Username { get; }
        string Password { get; }
    }

    public class IntegrationEmailConfiguration : IIntegrationEmailConfiguration
    {
        public string ImapServer { get; set; }
        public int ImapPort { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }


}
