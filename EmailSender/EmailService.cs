using EmailSender.Models;

namespace EmailSender
{
    public interface IEmailService
    {
        void Send(EmailModel email);
    }

    public class EmailService : IEmailService
    {
        public void Send(EmailModel email)
        {
        }
    }
}
