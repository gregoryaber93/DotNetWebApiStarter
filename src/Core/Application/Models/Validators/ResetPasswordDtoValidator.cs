using Application.DTO;
using FluentValidation;

namespace Application.Models.Validators
{
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.RegisterCode)
                .NotEmpty().WithMessage("Reset code is required")
                .Length(6).WithMessage("Reset code must be 6 digits")
                .Matches("^[0-9]*$").WithMessage("Reset code must contain only digits");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");

            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty().WithMessage("Please confirm your new password")
                .Equal(x => x.NewPassword).WithMessage("Passwords do not match");
        }
    }
} 