using Application.DTO;
using FluentValidation;
using Persistence.EF.DbContexts;

namespace Application.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(UserDbContext userDbContext) 
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.RepeatPassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match.");

            RuleFor(x => x.Email).Custom((value, context) =>
            {
                if(userDbContext.User.Any(u => u.Email == value))
                {
                    context.AddFailure("Email", "WrongEmail");
                }
            });
        }
    }
}