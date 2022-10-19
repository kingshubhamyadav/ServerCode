using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using OnDemandCarWash.Dtos;

namespace OnDemandCarWash.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IConfiguration _config;
        public EmailRepository(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        #region SendEmailMethod

        public string? SendEmail(EmailDto request)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("MailSettings:EmailUsername").Value));
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;
                email.Body = new TextPart(TextFormat.Plain) { Text = request.Body };

                using(var smtp = new SmtpClient())
                {
                    //configure the smtp connection : host,port,tls encryption
                    smtp.Connect(_config.GetSection("MailSettings:EmailHost").Value,
                        Convert.ToInt32(_config.GetSection("MailSettings:Port").Value),
                        SecureSocketOptions.StartTls);
                    //configure the username and password of the "from" email address
                    smtp.Authenticate(_config.GetSection("MailSettings:EmailUsername").Value,
                        _config.GetSection("MailSettings:EmailPassword").Value);
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }

                return "Success!";

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Error occurred at SendEmail in EmailService");
                return null;
            }
            finally
            {
            }
            
        }
        #endregion
    }
}
