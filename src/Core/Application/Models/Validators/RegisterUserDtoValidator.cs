using Application.DTO;
using FluentValidation;
using Persistence.EF.DbContexts;

namespace Application.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(UserDbContext userDbContext) 
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).MinimumLength(8);
            RuleFor(x => x.RepeatPassword).Equal(e => e.Password);
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