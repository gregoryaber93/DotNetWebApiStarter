using Application.Models;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Application.Services.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var message = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(to);

            using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
            {
                Credentials = new System.Net.NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
        }

        public async Task SendRegistrationConfirmationAsync(string to, string registerCode)
        {
            var subject = "Confirm Your Registration";
            var body = $@"
                <h2>Welcome to Our Application!</h2>
                <p>Thank you for registering. To complete your registration, please use the following code:</p>
                <h3 style='color: #007bff;'>{registerCode}</h3>
                <p>This code will be required to activate your account.</p>
                <p>If you did not request this registration, please ignore this email.</p>";

            await SendEmailAsync(to, subject, body);
        }

        public async Task SendPasswordResetAsync(string to, string resetCode)
        {
            var subject = "Reset Your Password";
            var body = $@"
                <h2>Password Reset Request</h2>
                <p>You have requested to reset your password. Please use the following code to reset your password:</p>
                <h3 style='color: #007bff;'>{resetCode}</h3>
                <p>This code will be required to reset your password.</p>
                <p>If you did not request this password reset, please ignore this email and ensure your account is secure.</p>";

            await SendEmailAsync(to, subject, body);
        }
    }
} 