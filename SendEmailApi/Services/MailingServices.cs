using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SendEmailApi.Extension.Settings;

namespace SendEmailApi.Services
{
    public class MailingServices : IMailingService
    {
        private readonly MailSettings _mailsettings;
        public MailingServices(IOptions<MailSettings> mailsettings)
        {
            _mailsettings = mailsettings.Value;
        }
        public async Task SendEmailAsync(string mailto, string subject, string body, IList<IFormFile> attachments = null)
        {

            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailsettings.Email),
                Subject = subject
            };
            email.To.Add(MailboxAddress.Parse(mailto));

            var builder = new BodyBuilder();
            if (attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in attachments)
                {
                    if (file.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mailsettings.DisplayName, _mailsettings.Email));

            using var smtp = new SmtpClient();
            smtp.Connect(_mailsettings.Host, _mailsettings.Port,SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailsettings.Email, _mailsettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
