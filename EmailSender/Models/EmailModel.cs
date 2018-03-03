namespace EmailSender.Models
{
    public class EmailModel {
        public string Body { get; set; }

        public EmailModel(string body)
        {
            Body = body;
        }
    }
}
