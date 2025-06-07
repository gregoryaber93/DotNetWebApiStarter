using System.ComponentModel.DataAnnotations;

namespace Application.DTO
{
    public class VerifyEmailDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string RegisterCode { get; set; }
    }
} 