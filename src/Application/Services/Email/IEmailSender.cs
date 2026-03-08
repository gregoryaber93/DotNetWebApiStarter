using System.Threading.Tasks;

namespace Application.Services.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendRegistrationConfirmationAsync(string to, string registerCode, Guid userId);
        Task SendPasswordResetAsync(string to, string registerCode, Guid userId);
    }
} 