using EmailSender.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmailSender.Controllers
{
    [Route("/[controller]")]
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult Post(EmailModel email)
        {
            if (!IsValid(email))
                return BadRequest();

            _emailService.Send(email);

            return NoContent();
        }

        bool IsValid(EmailModel email)
        {
            return !string.IsNullOrWhiteSpace(email.Body);
        }
    }
}
