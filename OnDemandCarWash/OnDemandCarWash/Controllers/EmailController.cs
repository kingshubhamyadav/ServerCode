using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnDemandCarWash.Dtos;
using OnDemandCarWash.Services;

namespace OnDemandCarWash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly EmailService _email;
        public EmailController(EmailService email)
        {
            _email = email ?? throw new ArgumentNullException(nameof(email));
        }

        [HttpPost]
        public IActionResult SendMail(EmailDto request)
        {
            var res = _email.SendEmail(request);
            if(res == null)
            {
                return BadRequest("Error while sending mail");
            }
            return Ok(res);
        }
    }
}
