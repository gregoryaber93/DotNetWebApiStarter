using System.ComponentModel.DataAnnotations;

namespace Application.DTO
{
    public class RequestPasswordResetDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
} 