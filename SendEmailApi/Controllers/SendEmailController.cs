using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendEmailApi.Dtos;
using SendEmailApi.Services;

namespace SendEmailApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        private readonly IMailingService _mailingService;

        public SendEmailController(IMailingService mailingService)
        {
            _mailingService = mailingService;
        }

        [HttpPost("SendMailMailKit")]
        public async Task<IActionResult> SendMailMailKit([FromForm] MailRequestDto mail)
        {
            await _mailingService.SendEmailAsync(mail.ToEmail, mail.Subject,mail.Body, mail.Attachments);

            return Ok("Success");
        }
    }
}
