
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using AMaz.Entity;
using AMaz.Common;
using Microsoft.AspNetCore.Identity;


namespace AMaz.Service
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> logger;
        private readonly IHostingEnvironment evironment;
        private readonly EmailSetting _emailSetting;
        private readonly UserManager<User> _userManager;


        public EmailService(ILogger<EmailService> logger, IHostingEnvironment environment, IOptions<EmailSetting> emailSetting, UserManager<User> userManager)
        {
            this.logger = logger;
            this.logger.LogInformation("Create MailService");
            _emailSetting = emailSetting.Value;
            evironment = environment;
            _userManager = userManager;
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
   
        public async Task SendCreateContributionEmail(Contribution contribution, string origin,string coordinatorEmail)
        {
            string message = await System.IO.File.ReadAllTextAsync(Path.Combine(evironment.ContentRootPath, "EmailHtmls/CreateContribution.html"));
            message = message.Replace("[[name]]", contribution.AuthorName);
            if (!string.IsNullOrEmpty(origin))
            {
                var verifyUrl = $"{origin}/contribution/{contribution.ContributionId}";
                message = message.Replace("[[link]]", verifyUrl);
            }
            else
            {
                message =
                    $@"<p>Please use the below token to verify your email address with the <code>/accounts/verify-email</code> api route:</p>
                    <p><code>{contribution.ContributionId}</code></p>";
            }

            await Send(coordinatorEmail, "Sign-up Verification API - Verify Email", message);
           
        }
    }
}
