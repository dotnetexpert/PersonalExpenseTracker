using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    [Route("email")]
    [ApiController]
    public class EmailController : ApiController
    {
        private readonly IEmailSender _emailSender;
        public EmailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpGet]
        public Task<ActionResult> SendMail(string email,string subject,string message)
        {
            email = "amanmor815@gmail.com";
            subject = "Null";
            message = "Null";
            var mail = _emailSender.SendEmailAsync(email, subject, message);
            return null;
        }
    }
}
