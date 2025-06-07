using System.ComponentModel.DataAnnotations;

namespace Application.DTO
{
    public class VerifyEmailDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string RegisterCode { get; set; }
    }
} 