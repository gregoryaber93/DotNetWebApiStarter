using System.ComponentModel.DataAnnotations;

namespace Application.DTO
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string RegisterCode { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
} 