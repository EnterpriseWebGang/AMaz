
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using AMaz.Entity;
using AMaz.Common;


namespace AMaz.Service
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> logger;
        private readonly IHostingEnvironment evironment;
        private readonly EmailSetting _emailSetting;

        public EmailService(ILogger<EmailService> logger, IHostingEnvironment environment, IOptions<EmailSetting> emailSetting)
        {
            this.logger = logger;
            this.logger.LogInformation("Create MailService");
            _emailSetting = emailSetting.Value;
            evironment = environment;
        }
        public async Task SendCreateResetPasswordEmail(User user, ResetPasswordRequest request)
        {
            string message = await System.IO.File.ReadAllTextAsync(Path.Combine(evironment.ContentRootPath, "EmailHtmls/ResetPassword.html"));
            message = message.Replace("[[name]]", user.FirstName);
            message = message.Replace("[[email]]", user.Email);
            message = message.Replace("[[password]]", request.Password);

            await Send(
                to: user.Email,
                subject: "Your Account Information",
                html: message
            );

        }
        public async Task SendCreateAccountEmail(CreateAccountRequest request)
        {
            string message = await System.IO.File.ReadAllTextAsync(Path.Combine(evironment.ContentRootPath, "EmailHtmls/RegisteredEmail.html"));
            message = message.Replace("[[name]]", request.FirstName);
            message = message.Replace("[[email]]", request.Email);
            message = message.Replace("[[password]]", request.Password);

            await Send(
                to: request.Email,
                subject: "Your Account Information",
                html: message
            );
        }
        public async Task Send(string to, string subject, string html)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_emailSetting.DisplayName, _emailSetting.Mail);
            email.From.Add(new MailboxAddress(_emailSetting.DisplayName, _emailSetting.Mail));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;


            var builder = new BodyBuilder();
            builder.HtmlBody = html;
            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient(); // Using finished deleted so as not to slow down the system.

            try
            {
                smtp.Connect(_emailSetting.Host, _emailSetting.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailSetting.Mail, _emailSetting.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            smtp.Disconnect(true);

            logger.LogInformation("send mail to " + to);
        }

        public Task SendCreateContributionEmail(Contribution contribution)
        {
            throw new NotImplementedException();
        }
    }
}
