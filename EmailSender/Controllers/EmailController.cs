using EmailSender.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmailSender.Controllers
{
    [Route("/[controller]")]
    public class EmailController : Controller
    {
        [HttpPost]
        public IActionResult Post(EmailModel email)
        {
            if (!IsValid(email))
                return BadRequest();

            return NoContent();
        }

        bool IsValid(EmailModel email)
        {
            return !string.IsNullOrWhiteSpace(email.Body);
        }
    }
}
